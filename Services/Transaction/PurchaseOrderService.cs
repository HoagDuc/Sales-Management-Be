using BT_MVC_Web.Exceptions;
using ClosedXML.Excel;
using ptdn_net.Const.Emun;
using ptdn_net.Data.Dto.Transaction;
using ptdn_net.Data.Entity.TransactionEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;
using ptdn_net.Utils;

namespace ptdn_net.Services;

public class PurchaseOrderService : BaseService, IPurchaseOrderService
{
    private readonly IVendorService _vendorService;
    private readonly IInventoryService _inventoryService;
    private readonly ITransactionService _transactionService;

    public PurchaseOrderService(IVendorService vendorService
        , IUnitOfWork unitOfWork
        , ITransactionService transactionService
        , IInventoryService inventoryService) 
        : base(unitOfWork)
    {
        _inventoryService = inventoryService;
        _vendorService = vendorService;
        _transactionService = transactionService;
    }

    public async Task<List<PurchaseOrderResp>> GetAll()
    {
        var purchaseOrders = await UnitOfWork.PurchaseOrderRepo.GetAll();
        return purchaseOrders.Select(x => new PurchaseOrderResp(x)).OrderByDescending(x => x.CreateAt).ToList();
    }

    public async Task<PurchaseOrderResp> GetById(Guid id)
    {
        var entity = await UnitOfWork.PurchaseOrderRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("PurchaseOrder " + id + " not found");
        return new PurchaseOrderResp(entity);
    }

    public async Task<Guid> CreateBy(PurchaseOrderReq req)
    {
        await ValidCreateBy(req);

        var entity = new PurchaseOrder();
        SetValue(entity, req);
        entity.PurchaseOrderDetails = req.PurchaseOrderDetails!.Select(x => new PurchaseOrderDetail
        {
            ProductId = x.ProductId,
            PurchaseOrderId = entity.PurchaseOrderId,
            Quantity = x.Quantity,
            Discount = x.Discount,
            Price = x.Price
        }).ToList();
        await UnitOfWork.PurchaseOrderRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.PurchaseOrderId;
    }

    public async Task<Guid> UpdateBy(PurchaseOrderReq req, Guid id)
    {
        await ValidUpdateBy(req, id);

        var entity =  await UnitOfWork.PurchaseOrderRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("PurchaseOrder " + id + " not found");
        if (entity.Status == (short)Status.HoanThanh) throw new BadRequestException("PurchaseOrder " + id + " has been completed");
        SetValue(entity, req);

        var currentDetails = entity.PurchaseOrderDetails.ToList();
        foreach (var reqDetail in req.PurchaseOrderDetails!)
        {
            var entityDetail = entity.PurchaseOrderDetails
                .FirstOrDefault(x => x.ProductId == reqDetail.ProductId && x.PurchaseOrderId == entity.PurchaseOrderId);
            if (entityDetail is null)
            {
                var newDetail = new PurchaseOrderDetail
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

        entity.PurchaseOrderDetails = currentDetails;
        
        if (req.Status == (short)Status.HoanThanh)
        {
            await _transactionService.Create(entity.PurchaseOrderId.ToString(), 2, entity.TotalAmount, DateTime.Now);
        }
        UnitOfWork.PurchaseOrderRepo.Update(entity);
        await UnitOfWork.CompleteAsync();
        return entity.PurchaseOrderId;
    }
    
    private async Task ValidCreateBy(PurchaseOrderReq req)
    {
        await ValidExistsByCode(req);
        await _vendorService.ValidNotExistsByVendorId(req.VendorId); 
    }

    private async Task ValidUpdateBy(PurchaseOrderReq req, Guid id)
    {
        await ValidExistsByCode(req, id);
        await _vendorService.ValidNotExistsByVendorId(req.VendorId); 
    }
    
    private async Task ValidExistsByCode(PurchaseOrderReq req, Guid? id = null)
    {
        var existsBy = await UnitOfWork.PurchaseOrderRepo.ExistsByCode(req.Code!, id);
        if (existsBy)
        {
            throw new NotFoundException("Exit by code: " + req.Code);
        }
    }
    
    private static void SetValue(PurchaseOrder entity, PurchaseOrderReq req)
    {
        entity.VendorId = req.VendorId;
        entity.Code = req.Code!;
        entity.OrderDate = req.OrderDate;
        entity.DeliveryDate = req.DeliveryDate;
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
        entity.AmountOther = req.AmountOther;
        entity.Note = req.Note;
    }

    public async Task DeleteBy(Guid id)
    {
        var entity = await UnitOfWork.PurchaseOrderRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("PurchaseOrder " + id + " not found");
        UnitOfWork.PurchaseOrderRepo.Remove(entity);
        await UnitOfWork.CompleteAsync();
    }

    public async Task<Guid> GoInventory(PurchaseOrderReq req, Guid id)
    {
        await ValidUpdateBy(req, id);

        var entity =  await UnitOfWork.PurchaseOrderRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("PurchaseOrder " + id + " not found");
        if (entity.Status == (short)Status.HoanThanh) throw new BadRequestException("PurchaseOrder " + id + " has been completed");
        SetValue(entity, req);
        if (req.AmountPaid == entity.TotalAmount)
            entity.Status = (short)Status.HoanThanh;
        else
            entity.Status = (short)Status.DangVanChuyen;
        
        var currentDetails = entity.PurchaseOrderDetails.ToList();
        foreach (var reqDetail in req.PurchaseOrderDetails!)
        {
            var entityDetail = entity.PurchaseOrderDetails
                .FirstOrDefault(x => x.ProductId == reqDetail.ProductId);
            if (entityDetail is null)
            {
                var newDetail = new PurchaseOrderDetail
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

            await _inventoryService.GoInventory(entityDetail!.ProductId, entityDetail.Quantity, entityDetail.Price );
            if (entity.Status == (short)Status.HoanThanh)
            {
                await _transactionService.Create(entity.PurchaseOrderId.ToString(), 2, entity.TotalAmount, DateTime.Now);
            }
        }
        entity.PurchaseOrderDetails = currentDetails;

        UnitOfWork.PurchaseOrderRepo.Update(entity);
        await UnitOfWork.CompleteAsync();
        return entity.PurchaseOrderId;
    }

    public XLWorkbook Export(List<PurchaseOrderResp> purchaseOrders, DateTime fromDate, DateTime toDate)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Danh sách đơn nhập");
        worksheet.Columns().AdjustToContents();

        var titleRow = worksheet.Range("A1:H1");
        titleRow.Merge();
        worksheet.Cell(1, 1).Value = $"Danh sách đơn nhập từ {fromDate:dd/MM/yyyy} đến {toDate:dd/MM/yyyy}";
        titleRow.Style.Font.Bold = true;
        titleRow.Style.Font.FontColor = XLColor.Black;
        titleRow.Style.Font.FontSize = 30;

        var headerRow = worksheet.Range("A3:H3");
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRow.Style.Font.FontColor = XLColor.Black;

        worksheet.Cell(3, 1).Value = "ID";
        worksheet.Cell(3, 2).Value = "Mã đơn nhập";
        worksheet.Cell(3, 3).Value = "Ngày tạo đơn";
        worksheet.Cell(3, 4).Value = "Trạng thái";
        worksheet.Cell(3, 5).Value = "Nhà cung cấp";
        worksheet.Cell(3, 6).Value = "Tổng tiền";
        worksheet.Cell(3, 7).Value = "Ghi chú";
        worksheet.Cell(3, 8).Value = "Người tạo đơn";
        
        for (int col = 1; col <= 8; col++)
        {
            worksheet.Cell(3, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(3, col).Style.Border.OutsideBorderColor = XLColor.Black;
        }

        var row = 4;
        foreach (var item in purchaseOrders)
        {
            worksheet.Cell(row, 1).Value = item.PurchaseOrderId.ToString();
            worksheet.Cell(row, 2).Value = item.Code;
            worksheet.Cell(row, 3).Value = item.CreateAt!.Value.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 4).Value = GetStatusForExcel(item.Status);
            worksheet.Cell(row, 5).Value = item.Vendor?.Name;
            worksheet.Cell(row, 6).Value = item.TotalAmount;
            worksheet.Cell(row, 7).Value = item.Note;
            worksheet.Cell(row, 8).Value = item.CreateBy;

            for (int col = 1; col <= 8; col++)
            {
                worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(row, col).Style.Border.OutsideBorderColor = XLColor.Black;
            }

            row++;
        }

        worksheet.Columns().AdjustToContents();
        return workbook;
    }

    public async Task<List<PurchaseOrderResp>> GetAllDangVanChuyen()
    {
        var orders = await UnitOfWork.PurchaseOrderRepo.GetAllDangVanChuyen();
        return orders.Select(x => new PurchaseOrderResp(x)).OrderByDescending(x => x.CreateAt).ToList();
    }

    public async Task<List<PurchaseOrderResp>> GetAllFromDate(DateTime? ngayBatDau, DateTime? ngayKetThuc)
    {
        var purchaseOrders = await UnitOfWork.PurchaseOrderRepo.GetAllFromDate(ngayBatDau, ngayKetThuc);
        return purchaseOrders.Select(x => new PurchaseOrderResp(x)).OrderByDescending(x => x.CreateAt).ToList();
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