using BT_MVC_Web.Exceptions;
using ClosedXML.Excel;
using ptdn_net.Const.Emun;
using ptdn_net.Data.Dto.Transaction;
using ptdn_net.Data.Entity.TransactionEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;
using ptdn_net.Utils;

namespace ptdn_net.Services;

public class RefundOrderService : BaseService, IRefundOrderService
{
    private readonly IInventoryService _inventoryService;
    private readonly ITransactionService _transactionService;        
    private readonly IOrderService _orderService;
    
    public RefundOrderService(IInventoryService inventoryService
        , IUnitOfWork unitOfWork
        , ITransactionService transactionService
        , IOrderService orderService)
        : base(unitOfWork)
    {
        _orderService = orderService;
        _inventoryService = inventoryService;
        _transactionService = transactionService;
    }

    public async Task<List<RefundOrderResp>> GetAll()
    {
        var refundOrders = await UnitOfWork.RefundOrderRepo.GetAll();
        return refundOrders.Select(x => new RefundOrderResp(x)).OrderByDescending(x => x.CreateAt).ToList();
    }

    public async Task<List<RefundOrderResp>> GetAllFromDate(DateTime? fromDate, DateTime? toDate)
    {
        var refundOrders = await UnitOfWork.RefundOrderRepo.GetAllFromDate(fromDate, toDate);
        return refundOrders.Select(x => new RefundOrderResp(x)).OrderByDescending(x => x.CreateAt).ToList();
    }

    public async Task<RefundOrderResp> GetById(Guid id)
    {
        var entity = await UnitOfWork.RefundOrderRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("RefundOrder " + id + " not found");
        return new RefundOrderResp(entity);
    }

    public async Task<Guid> CreateBy(RefundOrderReq req)
    {
        await ValidCreateBy(req);

        var entity = new RefundOrder();
        SetValue(entity, req);
        entity.Status = (short)Status.XuLy;
        entity.RefundOrderDetails = req.RefundOrderDetails!.Select(x => new RefundOrderDetail
        {
            ProductId = x.ProductId,
            RefundOrderId = entity.RefundOrderId,
            Quantity = x.Quantity,
            Price = x.Price,
            Note = x.Note
        }).ToList();
        
        var orders = await _orderService.GetById(req.OrderId);
        if (orders is null) throw new NotFoundException("Order " + req.OrderId + " not found");
        foreach (var item2 in req.RefundOrderDetails!)
        {
            foreach (var item in orders.OrderDetails)
            {
                if (item.Quantity < item2.Quantity && item.ProductId == item2.ProductId)
                    throw new BadRequestException(" Số lượng hoàn không được lớn hơn số lượng đặt hàng");
            }
        }
        
        await UnitOfWork.RefundOrderRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.RefundOrderId;
    }

    public async Task<Guid> UpdateBy(RefundOrderReq req, Guid id)
    {
        await ValidUpdateBy(req, id);

        var entity =  await UnitOfWork.RefundOrderRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("RefundOrder " + id + " not found");
        if (entity.Status == (short)Status.HoanThanh) throw new BadRequestException("RefundOrder " + id + " has been completed");
        SetValue(entity, req);
        
        var orders = await _orderService.GetById(req.OrderId);
        if (orders is null) throw new NotFoundException("Order " + req.OrderId + " not found");
        foreach (var item2 in req.RefundOrderDetails!)
        {
            foreach (var item in orders.OrderDetails)
            {
                if (item.Quantity < item2.Quantity && item.ProductId == item2.ProductId)
                    throw new BadRequestException(" Số lượng hoàn không được lớn hơn số lượng đặt hàng");
            }
        }

        var currentDetails = entity.RefundOrderDetails.ToList();
        foreach (var reqDetail in req.RefundOrderDetails!)
        {
            var entityDetail = entity.RefundOrderDetails
                .FirstOrDefault(x => x.ProductId == reqDetail.ProductId && x.RefundOrderId == entity.RefundOrderId);
            if (entityDetail is null)
            {
                var newDetail = new RefundOrderDetail()
                {
                    ProductId = reqDetail.ProductId,
                    Quantity = reqDetail.Quantity,
                    Note = reqDetail.Note,
                    Price = reqDetail.Price
                };
                currentDetails.Add(newDetail);
            }
            else
            {
                entityDetail.Quantity = reqDetail.Quantity;
                entityDetail.Note = reqDetail.Note;
                entityDetail.Price = reqDetail.Price;
            }
        }

        entity.RefundOrderDetails = currentDetails;

        if (req.Status == (short)Status.HoanThanh)
        {
            await _orderService.UpdateStatusRefundBy(entity.OrderId);
            await _transactionService.Create(entity.RefundOrderId.ToString(), 2, req.TotalAmount, DateTime.Now);
        }
        UnitOfWork.RefundOrderRepo.Update(entity);
        await UnitOfWork.CompleteAsync();
        return entity.RefundOrderId;
    }
    
    private async Task ValidCreateBy(RefundOrderReq req)
    {
        await ValidExistsByCode(req);
        await _orderService.ValidNotExistsByOrderId(req.OrderId);
    }

    private async Task ValidUpdateBy(RefundOrderReq req, Guid id)
    {
        await ValidExistsByCode(req, id);
        await _orderService.ValidNotExistsByOrderId(req.OrderId);
    }
    
    private async Task ValidExistsByCode(RefundOrderReq req, Guid? id = null)
    {
        var existsBy = await UnitOfWork.RefundOrderRepo.ExistsByCode(req.Code!, id);
        if (existsBy)
        {
            throw new NotFoundException("Exit by code: " + req.Code);
        }
    }
    
    private static void SetValue(RefundOrder entity, RefundOrderReq req)
    {
        entity.OrderId = req.OrderId;
        entity.Code = req.Code!;
        entity.Adress = req.Adress!;
        entity.Quantity = req.Quantity;
        entity.Status = req.Status;
        entity.TotalAmount = req.TotalAmount;
        entity.AmountOther = req.AmountOther;
        entity.Note = req.Note;
    }

    public async Task DeleteBy(Guid id)
    {
        var entity = await UnitOfWork.RefundOrderRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("RefundOrder " + id + " not found");
        UnitOfWork.RefundOrderRepo.Remove(entity);
        await UnitOfWork.CompleteAsync();
    }

    public async Task<Guid> GoInventory(RefundOrderReq req, Guid id)
    {
        await ValidUpdateBy(req, id);

        var entity =  await UnitOfWork.RefundOrderRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("RefundOrder " + id + " not found");
        if (entity.Status == (short)Status.HoanThanh) throw new BadRequestException("RefundOrder " + id + " has been completed");
        SetValue(entity, req);
        entity.Status = (short)Status.HoanThanh;

        var currentDetails = entity.RefundOrderDetails.ToList();
        foreach (var reqDetail in req.RefundOrderDetails!)
        {
            var entityDetail = entity.RefundOrderDetails
                .FirstOrDefault(x => x.ProductId == reqDetail.ProductId && x.RefundOrderId == entity.RefundOrderId);
            if (entityDetail is null)
            {
                var newDetail = new RefundOrderDetail()
                {
                    ProductId = reqDetail.ProductId,
                    Quantity = reqDetail.Quantity,
                    Note = reqDetail.Note,
                    Price = reqDetail.Price
                };
                currentDetails.Add(newDetail);
            }
            else
            {
                entityDetail.Quantity = reqDetail.Quantity;
                entityDetail.Note = reqDetail.Note;
                entityDetail.Price = reqDetail.Price;
            }
            
            await _inventoryService.GoInventory2(entityDetail!.ProductId, entityDetail.Quantity);
        }
        await _orderService.UpdateStatusRefundBy(entity.OrderId);
        await _transactionService.Create(entity.RefundOrderId.ToString(), 2, req.TotalAmount, DateTime.Now);
        entity.RefundOrderDetails = currentDetails;

        UnitOfWork.RefundOrderRepo.Update(entity);
        await UnitOfWork.CompleteAsync();
        return entity.RefundOrderId;
    }

    public XLWorkbook Export(List<RefundOrderResp> refundOrders, DateTime fromDate, DateTime toDate)
    {
         var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Danh sách đơn hoàn");
        worksheet.Columns().AdjustToContents();

        var titleRow = worksheet.Range("A1:I1");
        titleRow.Merge();
        worksheet.Cell(1, 1).Value = $"Danh sách đơn hoàn từ {fromDate:dd/MM/yyyy} đến {toDate:dd/MM/yyyy}";
        titleRow.Style.Font.Bold = true;
        titleRow.Style.Font.FontColor = XLColor.Black;
        titleRow.Style.Font.FontSize = 30;

        var headerRow = worksheet.Range("A3:H3");
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRow.Style.Font.FontColor = XLColor.Black;

        worksheet.Cell(3, 1).Value = "ID";
        worksheet.Cell(3, 2).Value = "Mã đơn hoàn";
        worksheet.Cell(3, 3).Value = "Ngày tạo đơn";
        worksheet.Cell(3, 4).Value = "Trạng thái";
        worksheet.Cell(3, 5).Value = "Mã đơn nhập";
        worksheet.Cell(3, 6).Value = "Tổng tiền";
        worksheet.Cell(3, 7).Value = "Địa chỉ";
        worksheet.Cell(3, 8).Value = "Ghi chú";
        worksheet.Cell(3, 9).Value = "Người tạo đơn";
        
        for (int col = 1; col <= 9; col++)
        {
            worksheet.Cell(3, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(3, col).Style.Border.OutsideBorderColor = XLColor.Black;
        }

        var row = 4;
        foreach (var item in refundOrders)
        {
            worksheet.Cell(row, 1).Value = item.RefundOrderId.ToString();
            worksheet.Cell(row, 2).Value = item.Code;
            worksheet.Cell(row, 3).Value = item.CreateAt != null ? item.CreateAt.Value.ToString("dd/MM/yyyy") : "";
            worksheet.Cell(row, 4).Value = GetStatusForExcel(item.Status);
            worksheet.Cell(row, 5).Value = item.OrderId.ToString();
            worksheet.Cell(row, 6).Value = item.TotalAmount;
            worksheet.Cell(row, 7).Value = item.Adress;
            worksheet.Cell(row, 8).Value = item.Note;
            worksheet.Cell(row, 9).Value = item.CreateBy;

            for (int col = 1; col <= 9; col++)
            {
                worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(row, col).Style.Border.OutsideBorderColor = XLColor.Black;
            }

            row++;
        }

        worksheet.Columns().AdjustToContents();
        return workbook;
    }

    public async Task<List<RefundOrderResp>> GetAllDangVanChuyen()
    {
        var orders = await UnitOfWork.RefundOrderRepo.GetAllDangVanChuyen();
        return orders.Select(x => new RefundOrderResp(x)).OrderByDescending(x => x.CreateAt).ToList();
    }

    private static string GetStatusForExcel(short? status)
    {
        return status switch
        {
            (short)Status.XuLy => "Xử lý",
            (short)Status.XacNhan => "Xác nhận",
            (short)Status.DangVanChuyen => "Đang vận chuyển",
            (short)Status.ThanhToan => "Thanh toán",
            (short)Status.HoanThanh => "Hoàn thành",
            _ => status == (short)Status.Huy ? "Huỷ" : "Không xác định"
        };
    }
}