using System.Globalization;
using BT_MVC_Web.Exceptions;
using ClosedXML.Excel;
using Microsoft.AspNetCore.SignalR;
using ptdn_net.Const.Emun;
using ptdn_net.Data.Dto.BaoCao;
using ptdn_net.Data.Dto.Transaction;
using ptdn_net.Data.Entity.TransactionEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;
using ptdn_net.SignalR;
using ptdn_net.Utils;
using ptdn_net.Utils.Email;
using TransactionType = ptdn_net.Const.Emun.TransactionType;

namespace ptdn_net.Services;

public class OrderService : BaseService, IOrderService
{
    private readonly ICustomerService _customerService;
    private readonly IEmailService _emailService;
    private readonly IInventoryService _inventoryService;
    private readonly ITransactionService _transactionService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public OrderService(IUnitOfWork unitOfWork,
        ICustomerService customerService,
        IInventoryService inventoryService,
        ITransactionService transactionService,
        IHubContext<NotificationHub> hubContext,
        IEmailService emailService) : base(unitOfWork)
    {
        _transactionService = transactionService;
        _hubContext = hubContext;
        _emailService = emailService;
        _customerService = customerService;
        _inventoryService = inventoryService;
    }

    public async Task<List<OrderResp>> GetAll()
    {
        var orders = await UnitOfWork.OrderRepo.GetAll();
        return orders.Select(x => new OrderResp(x)).OrderByDescending(x => x.CreateAt).ToList();
    }
    
    public async Task<List<OrderResp>> GetByCustomerId(Guid customerId)
    {
        var orders = await UnitOfWork.OrderRepo.GetByCustomerId(customerId);
        return orders.Select(x => new OrderResp(x)).OrderByDescending(x => x.CreateAt).ToList();
    }
    
    public async Task<List<OrderResp>> GetAllFromDate(DateTime? fromDate, DateTime? toDate)
    {
        var orders = await UnitOfWork.OrderRepo.GetAllFromDate(fromDate, toDate);
        return orders.Select(x => new OrderResp(x)).OrderByDescending(x => x.CreateAt).ToList();
    }
    
    public async Task<List<OrderResp>> GetAllFromDateV2(DateTime? fromDate, DateTime? toDate)
    {
        var orders = await UnitOfWork.OrderRepo.GetAllFromDatev2(fromDate, toDate);
        return orders.Select(x => new OrderResp(x)).OrderByDescending(x => x.CreateAt).ToList();
    }
    
    public async Task<List<OrderResp>> GetAllDangVanChuyen()
    {
        var orders = await UnitOfWork.OrderRepo.GetAllDangVanChuyen();
        return orders.Select(x => new OrderResp(x)).OrderByDescending(x => x.CreateAt).ToList();
    }

    public async Task<OrderResp> GetById(Guid id)
    {
        var entity = await UnitOfWork.OrderRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("PurchaseOrder " + id + " not found");
        return new OrderResp(entity);
    }

    public async Task<Guid> CreateBy(OrderReq req)
    {
        await ValidCreateBy(req);

        var entity = new Order();
        SetValue(entity, req);

        entity.OrderDetails = req.OrderDetails.Select(x => new OrderDetail
        {
            ProductId = x.ProductId,
            OrderId = entity.OrderId,
            Quantity = x.Quantity,
            Discount = x.Discount,
            Price = x.Price
        }).ToList();

        if (req.Pay != 0) await _customerService.UpdateDebtBy(req.CustomerId, -req.Pay);

        await UnitOfWork.OrderRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.OrderId;
    }
    
    public async Task<Guid> CreateDonBanTaiQuayBy(OrderReq req)
    {
        await ValidCreateByV2(req);

        var entity = new Order();
        SetValue(entity, req);
        entity.Status = 5;
        entity.CustomerId = new Guid("44549576-3003-4b39-bb6a-2481997f0900");
        
        entity.OrderDetails = req.OrderDetails.Select(x => new OrderDetail
        {
            ProductId = x.ProductId,
            OrderId = entity.OrderId,
            Quantity = x.Quantity,
            Discount = x.Discount,
            Price = x.Price
        }).ToList();

        await UnitOfWork.OrderRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();

        foreach (var reqDetail in req.OrderDetails)
        {
            var entityDetail = entity.OrderDetails
                .FirstOrDefault(x => x.ProductId == reqDetail.ProductId && x.OrderId == entity.OrderId);

            await _inventoryService.OutInventory(entityDetail!.ProductId, entityDetail.Quantity);
        }

        await _transactionService.Create(entity.OrderId.ToString(), 1, req.TotalAmount, DateTime.Now);

        await UnitOfWork.CompleteAsync();
        var listProduct = await _inventoryService.GetAllSpHetHang();
        var inventories = listProduct!.ToList();
        if (inventories.Count > 0)
        {
           await _hubContext.Clients.All.SendAsync("ProductHetHang", inventories.Count);   
        }
        return entity.OrderId;
    }

    public async Task<Guid> UpdateBy(OrderReq req, Guid id)
    {
        await ValidUpdateBy(req, id);

        var entity = await UnitOfWork.OrderRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Order " + id + " not found");
        if (entity.Status is (short)Status.HoanThanh 
            or (short)Status.Huy 
            or (short)Status.GiaoThatBai 
            or (short)Status.HoanHang)
            throw new BadRequestException("Order " + id + " has been completed");
        SetValue(entity, req);

        var currentDetails = entity.OrderDetails.ToList();
        foreach (var reqDetail in req.OrderDetails)
        {
            var entityDetail = entity.OrderDetails
                .FirstOrDefault(x => x.ProductId == reqDetail.ProductId && x.OrderId == entity.OrderId);
            if (entityDetail is null)
            {
                var newDetail = new OrderDetail
                {
                    ProductId = reqDetail.ProductId,
                    Quantity = reqDetail.Quantity,
                    Discount = reqDetail.Discount,
                    Price = reqDetail.Price
                };
                currentDetails.Add(newDetail);
            }
            else
            {
                entityDetail.Quantity = reqDetail.Quantity;
                entityDetail.Discount = reqDetail.Discount;
                entityDetail.Price = reqDetail.Price;
            }
        }
        
        entity.OrderDetails = currentDetails;

        if (req.Pay != 0) await _customerService.UpdateDebtBy(req.CustomerId, -req.Pay);

        if (req.Status == (short)Status.HoanThanh)
            await _transactionService.Create(entity.OrderId.ToString(), 1, req.TotalAmount, DateTime.Now);

        UnitOfWork.OrderRepo.Update(entity);
        await UnitOfWork.CompleteAsync();
        return entity.OrderId;
    }

    public async Task<Guid> UpdateStatusRefundBy(Guid id)
    {
        var entity = await UnitOfWork.OrderRepo.GetByIdAsync(id);
        
        if (entity is null) throw new NotFoundException("Order " + id + " not found");
        if (entity.Status != (short)Status.HoanThanh)
            throw new BadRequestException("Order " + id + " dont refund");
        entity.Status = (short)Status.HoanHang;
        return entity.OrderId;
    }

    public async Task DeleteBy(Guid id)
    {
        var entity = await UnitOfWork.OrderRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("PurchaseOrder " + id + " not found");
        UnitOfWork.OrderRepo.Remove(entity);
        await UnitOfWork.CompleteAsync();
    }

    public async Task<Guid> OutInventory(OrderReq req, Guid id)
    {
        await ValidUpdateBy(req, id);

        var entity = await UnitOfWork.OrderRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Order " + id + " not found");
        if (entity.Status == (short)Status.HoanThanh)
            throw new BadRequestException("Order " + id + " has been completed");
        SetValue(entity, req);
        entity.Status = (short)Status.XacNhan;
        await SendEmailOrder(id);

        var currentDetails = entity.OrderDetails.ToList();
        foreach (var reqDetail in req.OrderDetails)
        {
            var entityDetail = entity.OrderDetails
                .FirstOrDefault(x => x.ProductId == reqDetail.ProductId && x.OrderId == entity.OrderId);
            if (entityDetail is null)
            {
                var newDetail = new OrderDetail
                {
                    ProductId = reqDetail.ProductId,
                    Quantity = reqDetail.Quantity,
                    Discount = reqDetail.Discount,
                    Price = reqDetail.Price
                };
                currentDetails.Add(newDetail);
            }
            else
            {
                entityDetail.Quantity = reqDetail.Quantity;
                entityDetail.Discount = reqDetail.Discount;
                entityDetail.Price = reqDetail.Price;
            }

            await _inventoryService.OutInventory(entityDetail!.ProductId, entityDetail.Quantity);
        }
        var listProduct = await _inventoryService.GetAllSpHetHang();
        var inventories = listProduct!.ToList();
        if (inventories.Count > 0)
        {
            await _hubContext.Clients.All.SendAsync("ProductHetHang", inventories.Count);   
        }

        entity.OrderDetails = currentDetails;

        if (req.Pay != 0) await _customerService.UpdateDebtBy(req.CustomerId, -req.Pay);

        UnitOfWork.OrderRepo.Update(entity);
        await UnitOfWork.CompleteAsync();
        return entity.OrderId;
    }

    public XLWorkbook Export(List<OrderResp> orders, DateTime fromDate, DateTime toDate)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Danh sách đơn bán");
        worksheet.Columns().AdjustToContents();

        var titleRow = worksheet.Range("A1:H1");
        titleRow.Merge();
        worksheet.Cell(1, 1).Value = $"Danh sách đơn bán (từ {fromDate:dd/MM/yyyy} đến {toDate:dd/MM/yyyy})";
        titleRow.Style.Font.Bold = true;
        titleRow.Style.Font.FontColor = XLColor.Black;
        titleRow.Style.Font.FontSize = 30;

        var headerRow = worksheet.Range("A3:K3");
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRow.Style.Font.FontColor = XLColor.Black;

        worksheet.Cell(3, 1).Value = "ID";
        worksheet.Cell(3, 2).Value = "Mã đơn bán";
        worksheet.Cell(3, 3).Value = "Ngày tạo đơn";
        worksheet.Cell(3, 4).Value = "Trạng thái";
        worksheet.Cell(3, 5).Value = "Khách hàng";
        worksheet.Cell(3, 6).Value = "Số điện thoại khách hàng";
        worksheet.Cell(3, 7).Value = "Địa chỉ";
        worksheet.Cell(3, 8).Value = "Phương thức thanh toán";
        worksheet.Cell(3, 9).Value = "Tổng tiền";
        worksheet.Cell(3, 10).Value = "Ghi chú";
        worksheet.Cell(3, 11).Value = "Người tạo đơn";

        for (int col = 1; col <= 11; col++)
        {
            worksheet.Cell(3, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(3, col).Style.Border.OutsideBorderColor = XLColor.Black;
        }

        var row = 4;
        foreach (var item in orders)
        {
            worksheet.Cell(row, 1).Value = item.OrderId.ToString();
            worksheet.Cell(row, 2).Value = item.Code;
            worksheet.Cell(row, 3).Value = item.CreateAt == null ? "" : item.CreateAt.Value.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 4).Value = GetStatusForExcel(item.Status);
            worksheet.Cell(row, 5).Value = item.Customer?.Name;
            worksheet.Cell(row, 6).Value = item.Customer?.Phone;
            worksheet.Cell(row, 7).Value = item.Address;
            worksheet.Cell(row, 8).Value = item.PaymentMethod == 0 ? "Tien mat" : "Chuyen khoan";
            worksheet.Cell(row, 9).Value = item.TotalAmount;
            worksheet.Cell(row, 10).Value = item.Note;
            worksheet.Cell(row, 11).Value = item.CreateBy;

            for (int col = 1; col <= 11; col++)
            {
                worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(row, col).Style.Border.OutsideBorderColor = XLColor.Black;
            }

            row++;
        }

        worksheet.Columns().AdjustToContents();
        return workbook;
    }

    public async Task<List<OrderResp>> GetAllOrderDone()
    {
        var orders = await UnitOfWork.OrderRepo.GetAllOrderDone();
        return orders.Select(x => new OrderResp(x)).OrderByDescending(x => x.CreateAt).ToList();
    }

    private static string GetStatusForExcel(short? status)
    {
        if (status == (short)Status.XuLy)
        {
            return "Xử lý";
        }

        if (status == (short)Status.XacNhan)
        {
            return "Xác nhận";
        }

        if (status == (short)Status.DangVanChuyen)
        {
            return "Đang vận chuyển";
        }

        if (status == (short)Status.ThanhToan)
        {
            return "Thanh toán";
        }

        if (status == (short)Status.HoanThanh)
        {
            return "Hoàn thành";
        }

        return status == (short)Status.Huy ? "Huỷ" : "Không xác định";
    }

    private async Task ValidCreateBy(OrderReq req)
    {
        await ValidSoLuongTrongKho(req);
        await ValidExistsByCode(req);
        await _customerService.ValidNotExistsByCustomerId(req.CustomerId);
    }

    private async Task ValidCreateByV2(OrderReq req)
    {
        await ValidSoLuongTrongKho(req);
        await ValidExistsByCode(req);
    }
    
    private async Task ValidUpdateBy(OrderReq req, Guid id)
    {
        await ValidExistsByCode(req, id);
        await _customerService.ValidNotExistsByCustomerId(req.CustomerId);
    }

    private static void SetValue(Order entity, OrderReq req)
    {
        entity.CustomerId = req.CustomerId;
        entity.Code = req.Code!;
        entity.Address = req.Address!;
        entity.PaymentMethod = req.PaymentMethod;
        entity.Status = req.Status;
        entity.Discount = req.Discount;
        entity.Vat = req.Vat;
        entity.Tax = req.Tax;
        entity.AmountDue = req.AmountDue;
        entity.AmountRemaining = req.AmountRemaining;
        entity.AmountPaid = req.AmountPaid;
        entity.TotalAmount = req.TotalAmount;
        entity.Phone = req.Phone!;
        entity.DeliveryMethod = req.DeliveryMethod;
        entity.Note = req.Note;
        entity.TotalNhap = req.TotalNhap;
    }

    private async Task ValidExistsByCode(OrderReq req, Guid? id = null)
    {
        var existsBy = await UnitOfWork.OrderRepo.ExistsByCode(req.Code!, id);
        if (existsBy) throw new NotFoundException("Exit by code: " + req.Code);
    }

    private async Task ValidSoLuongTrongKho(OrderReq req, Guid? id = null)
    {
        foreach (var orderDetail in req.OrderDetails)
        {
            var existsBy = await _inventoryService.GetByProductId(orderDetail.ProductId);
            if (existsBy != null && existsBy.Quantity < orderDetail.Quantity)
            {
                throw new NotFoundException($"Sản phẩm với ID {orderDetail.ProductId} vượt quá số lượng trong kho");
            }
        }
    }

    public async Task ValidNotExistsByOrderId(Guid? id)
    {
        var existsBy = await UnitOfWork.OrderRepo.ExistsById(id);
        if (!existsBy) throw new NotFoundException("Order not exit by id: " + id);
    }

    public async Task<Guid> UpdateStatusGiaoThatBai(Guid id)
    {
        var entity = await UnitOfWork.OrderRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Order " + id + " not found");
        if (entity.Status is (short)Status.HoanThanh 
            or (short)Status.Huy 
            or (short)Status.GiaoThatBai 
            or (short)Status.HoanHang)
            throw new BadRequestException("Order " + id + " has been completed");
        
        var req = new OrderReq
        {
            CustomerId = entity.CustomerId,
            Code = entity.Code,
            Address = entity.Address,
            PaymentMethod = entity.PaymentMethod,
            Discount = entity.Discount,
            Vat = entity.Vat,
            Tax = entity.Tax,
            AmountDue = entity.AmountDue,
            AmountRemaining = entity.AmountRemaining,
            AmountPaid = entity.AmountPaid,
            TotalAmount = entity.TotalAmount,
            Phone = entity.Phone,
            DeliveryMethod = entity.DeliveryMethod,
            Note = entity.Note,
            OrderDetails = entity.OrderDetails.Select(x => new OrderDetailReq
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Discount = x.Discount,
                Price = x.Price
            }).ToList()
        };
        
        await GoInventory(req, id);

        if (entity.AmountPaid > 0)
        {
            await _transactionService.Create(entity.OrderId.ToString(), (long)TransactionType.PhieuChi, entity.AmountPaid, DateTime.Now);
        }
        entity.Status = (short)Status.GiaoThatBai;

        UnitOfWork.OrderRepo.Update(entity);
        await UnitOfWork.CompleteAsync();
        return entity.OrderId;
    }
    
    public async Task GoInventory(OrderReq req, Guid id)
    {
        await ValidUpdateBy(req, id);

        var entity = await UnitOfWork.OrderRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Order " + id + " not found");
        if (entity.Status == (short)Status.HoanThanh)
            throw new BadRequestException("Order " + id + " has been completed");
        SetValue(entity, req);
        entity.Status = (short)Status.XacNhan;
        await SendEmailOrder(id);

        var currentDetails = entity.OrderDetails.ToList();
        foreach (var reqDetail in req.OrderDetails)
        {
            var entityDetail = entity.OrderDetails
                .FirstOrDefault(x => x.ProductId == reqDetail.ProductId && x.OrderId == entity.OrderId);
            if (entityDetail is null)
            {
                var newDetail = new OrderDetail
                {
                    ProductId = reqDetail.ProductId,
                    Quantity = reqDetail.Quantity,
                    Discount = reqDetail.Discount,
                    Price = reqDetail.Price
                };
                currentDetails.Add(newDetail);
            }
            else
            {
                entityDetail.Quantity = reqDetail.Quantity;
                entityDetail.Discount = reqDetail.Discount;
                entityDetail.Price = reqDetail.Price;
            }

            await _inventoryService.GoInventory2(entityDetail!.ProductId, entityDetail.Quantity);
        }

        entity.OrderDetails = currentDetails;

        UnitOfWork.OrderRepo.Update(entity);
        await UnitOfWork.CompleteAsync();
    }

    private async Task SendEmailOrder(Guid id)
    {
        var oder = await GetById(id);
        var mailRequest = new MailRequest
        {
            ToEmail = oder.Customer!.Email,
            Subject = $"[PTDN_XACNHAN] Xác nhận đơn hàng: {oder.Code?.ToUpper()}",
            Body = GetHtmlContent(oder)
        };
        await _emailService.SendEmailAsync(mailRequest);
    }

    private string GetHtmlContent(OrderResp orderResp)
    {
        // Tạo chuỗi HTML
        var response = "<div style=\"font-size: large\">";
        response += "<p>Kính gửi Quý khách hàng,</p>";
        response +=
            "<p>Chúng tôi xin trân trọng thông báo rằng đơn hàng của Quý khách đã được xác nhận thành công. Dưới đây là thông tin chi tiết về đơn hàng của Quý khách:</p>";
        response += "<ul>";
        response += $"<li>Mã đơn hàng: {orderResp.Code}</li>";
        var dateTime = DateTime.Parse(orderResp.CreateAt.Value.ToString(CultureInfo.CurrentCulture));
        var formattedDate = dateTime.ToString("dd 'tháng' MM 'năm' yyyy");
        response += $"<li>Ngày đặt hàng: {formattedDate}</li>";
        response += $"<li>Tổng cộng: {orderResp.TotalAmount} VND</li>";
        response += "</ul>";
        response += "<p><strong>Chi tiết sản phẩm:</strong></p>";
        response += "<ol>";

        response += "<table border=\"1\" cellspacing=\"0\" cellpadding=\"5\">";
        response += "<tr><th>Tên sản phẩm</th><th>Số lượng</th><th>Giá sản phẩm</th><th>Tổng giá sản phẩm</th></tr>";

        foreach (var product in orderResp.OrderDetails)
            response +=
                $"<tr><td>{product.Product.Name}</td><td>{product.Quantity}</td><td>{product.Price}</td><td>{product.Price * product.Quantity}</td></tr>";

        response += "</table>";
        response += "</ol>";
        response += $"<p><strong>Tổng cộng:</strong> {orderResp.TotalAmount} VND</p>";
        response += "<p><strong>Phương thức thanh toán:</strong> Tiền mặt</p>";
        response +=
            "<p>Chúng tôi sẽ tiếp tục xử lý đơn hàng của Quý khách và sẽ thông báo khi đơn hàng được vận chuyển.</p>";
        response +=
            "<p>Nếu Quý khách có bất kỳ câu hỏi hoặc cần hỗ trợ thêm, vui lòng liên hệ với chúng tôi qua email hoặc số điện thoại dưới đây.</p>";
        response += "<ul>";
        response += "<li>Hotline: <a href=\"tel:0348701696\">034 8701 696</a></li>";
        response += "<li>Email: <a href=\"ptdn@gmail.com\">ptdn@gmail.com</a></li>";
        response +=
            "<li>Fanpage: <a href=\"https://www.facebook.com/phongQhdnFpolyHN\">https://www.facebook.com/phongQlPtdn</a></li>";
        response += "</ul>";
        response += "<p>Thân mến!</p>";
        response += "</div>";

        return response;
    }
}