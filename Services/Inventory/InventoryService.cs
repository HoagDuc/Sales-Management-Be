using BT_MVC_Web.Exceptions;
using ClosedXML.Excel;
using ptdn_net.Data.Dto;
using ptdn_net.Data.Dto.BaoCao;
using ptdn_net.Data.Entity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services;

public class InventoryService : BaseService, IInventoryService
{
    private readonly ICapitailPriceVersionService _capitailPriceVersionService;
    
    public InventoryService(ICapitailPriceVersionService capitailPriceVersionService, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _capitailPriceVersionService = capitailPriceVersionService;
    }

    public async Task<long> CreateBy(long productId, long? minQuantity, long quantity, decimal price)
    {
        var entity = new Inventory();
        SetValue(entity, productId, minQuantity, quantity, price);

        await UnitOfWork.InventoryRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.InventoryId;
    }

    public async Task<long> UpdateBy(long productId, long? minQuantity, long quantity, decimal price)
    {
        var entity = await UnitOfWork.InventoryRepo.GetByProductId(productId);

        if (entity is null) throw new NotFoundException("Inventory " + productId + " not found");
        SetValue(entity!, productId, minQuantity, quantity, price);
        
        UnitOfWork.InventoryRepo.Update(entity!);
        await UnitOfWork.CompleteAsync();
        return entity.InventoryId;
    }

    public async Task GoInventory(long productId, short? quantity, decimal? price)
    {
        var entity = await UnitOfWork.InventoryRepo.GetByProductId(productId);
        
        if (entity is null) throw new NotFoundException("Inventory " + productId + " not found");
        entity!.Quantity += (long)quantity!;

        if (price != entity.CapitalPrice)
        {
            await _capitailPriceVersionService.CreateBy(quantity, entity.CapitalPrice, entity.InventoryId, DateTime.Now);
            entity.CapitalPrice = price; 
        }
        
        // Update cost price (Gia von) TODO 
        UnitOfWork.InventoryRepo.Update(entity!);
    }

    public async Task<IEnumerable<Inventory>?> GetAllSpHetHang()
    {
        return await UnitOfWork.InventoryRepo.GetAllByCondition(x => x.Quantity <= x.MinQuantity);
    }

    public async Task GoInventory2(long productId, short? quantity)
    {
        var entity = await UnitOfWork.InventoryRepo.GetByProductId(productId);
        
        if (entity is null) throw new NotFoundException("Inventory " + productId + " not found");
        entity!.Quantity += (long)quantity!;
        
        UnitOfWork.InventoryRepo.Update(entity!);
    }
    
    public async Task OutInventory(long productId, short? quantity)
    {
        var entity = await UnitOfWork.InventoryRepo.GetByProductId(productId);
        
        if (entity is null) throw new NotFoundException("Inventory " + productId + " not found");
        entity!.Quantity -= (long)quantity!;
        
        await _capitailPriceVersionService.OutInventory(entity.InventoryId, quantity);
        
        UnitOfWork.InventoryRepo.Update(entity!);
    }

    public async Task<InventoryResp> GetThongKeTonKho(long productId)
    {
        var entity = await UnitOfWork.InventoryRepo.GetByProductId(productId);

        if (entity is null) throw new NotFoundException("Inventory " + productId + " not found");
        var capitailPrice = await _capitailPriceVersionService.GetTotalPrice(entity.InventoryId);
        var resp = new InventoryResp
        {
            InventoryId = entity.InventoryId,
            ProductId = entity.ProductId,
            MinQuantity = entity.MinQuantity,
            Quantity = entity.Quantity + capitailPrice.Quantity,
            ReceiptDate = entity.ReceiptDate,
            DispatchDate = entity.DispatchDate,
            CapitalPrice = entity.CapitalPrice,
            TotalPrice = entity.CapitalPrice * (entity.Quantity - capitailPrice.Quantity) + capitailPrice.TotalPrice
        };

        return resp;
    }

    public async Task<List<InventoryResp>> GetAll()
    {
        var entities = await UnitOfWork.InventoryRepo.GetAll();
        return entities.Select(x => new InventoryResp(x)).ToList();
    }

    public async Task<List<BaoCaoKhoResp>> ThongKeKho()
    {
        var entities = await UnitOfWork.InventoryRepo.GetAll();
        var resp = new List<BaoCaoKhoResp>();

        foreach (var item in entities)
        {
           var capitailPrice = await _capitailPriceVersionService.GetTotalPrice(item.InventoryId);
           var totalPrice = item.CapitalPrice * (item.Quantity - capitailPrice.Quantity) + capitailPrice.TotalPrice;
           var respItem = new BaoCaoKhoResp
           {
               Id = item.ProductId,
               MaSanPham = item.Product.Code,
               Product = item.Product,
               TenSanPham = item.Product.Name,
               DonViTinh = item.Product.Unit.Name,
               BarCode = item.Product.Barcode,
               TonKho = item.Quantity,
               TongGiaHang = totalPrice,
               GiaNhap = item.Product.CostPrice,
               GiaVon = item.Quantity == 0 ? 0 : totalPrice/item.Quantity,
               GiaBanLe = item.Product.GiaNiemYet - (item.Product.GiaNiemYet * item.Product.Discount / 100),
               TrangThai = item.Product.IsActive ? "Hoạt động" : "Ngưng hoạt động"
           };
           
           resp.Add(respItem);
        }
        
        return resp.ToList();
    }

    public async Task<InventoryResp?> GetByProductId(long orderDetailProductId)
    {
        var entity = await UnitOfWork.InventoryRepo.GetByProductId(orderDetailProductId);
        if (entity == null) 
            throw new NotFoundException("Inventory " + orderDetailProductId + " not found");
        return new InventoryResp(entity);
    }

    public async Task<XLWorkbook> Export(List<InventoryResp> inventories)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Danh sách tồn kho");
        worksheet.Columns().AdjustToContents();

        var titleRow = worksheet.Range("A1:E1");
        titleRow.Merge();
        worksheet.Cell(1, 1).Value = "Danh sách tồn kho";
        titleRow.Style.Font.Bold = true;
        titleRow.Style.Font.FontColor = XLColor.Black;
        titleRow.Style.Font.FontSize = 30;

        var headerRow = worksheet.Range("A3:H3");
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRow.Style.Font.FontColor = XLColor.Black;

        worksheet.Cell(3, 1).Value = "ID";
        worksheet.Cell(3, 2).Value = "Tên sản phẩm";
        worksheet.Cell(3, 3).Value = "Tồn kho";
        worksheet.Cell(3, 4).Value = "Giá nhập";
        worksheet.Cell(3, 5).Value = "Giá vốn";
        worksheet.Cell(3, 6).Value = "Giá niêm yết";
        worksheet.Cell(3, 7).Value = "Chiết khấu nhập";
        worksheet.Cell(3, 8).Value = "Tổng giá hàng tồn trong kho";
        worksheet.Cell(3, 9).Value = "Trạng thái";
        for (var col = 1; col <= 9; col++)
        {
            worksheet.Cell(3, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(3, col).Style.Border.OutsideBorderColor = XLColor.Black;
        }

        var row = 4;
        foreach (var item in inventories)
        {
            var capitailPrice = await _capitailPriceVersionService.GetTotalPrice(item.InventoryId);
            var totalPrice = item.CapitalPrice * (item.Quantity - capitailPrice.Quantity) + capitailPrice.TotalPrice;
            worksheet.Cell(row, 1).Value = item.ProductId;
            worksheet.Cell(row, 2).Value = item.Product.Name;
            worksheet.Cell(row, 3).Value = item.Quantity;
            worksheet.Cell(row, 4).Value = item.Product.CostPrice;
            worksheet.Cell(row, 5).Value = totalPrice/item.Quantity;
            worksheet.Cell(row, 6).Value = item.Product.GiaNiemYet;
            worksheet.Cell(row, 7).Value = item.Product.ChietKhauNhap;
            worksheet.Cell(row, 8).Value = totalPrice;
            worksheet.Cell(row, 9).Value = item.Product.IsActive ? "Đang sử dụng" : "Vô hiệu hoá";

            for (var col = 1; col <= 9; col++)
            {
                worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(row, col).Style.Border.OutsideBorderColor = XLColor.Black;
            }

            row++;
        }

        worksheet.Columns().AdjustToContents();
        return workbook;
    }

    private static void SetValue(Inventory entity, long productId, long? minQuantity, long quantity, decimal price)
    {
        entity.ProductId = productId;
        entity.MinQuantity = minQuantity;
        entity.Quantity = quantity;
        entity.ReceiptDate = DateTime.Now;
        entity.DispatchDate = DateTime.Now;
        entity.CapitalPrice = price;
    }
}