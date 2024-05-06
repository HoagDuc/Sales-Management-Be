using BT_MVC_Web.Exceptions;
using ClosedXML.Excel;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services.Product;

public class ProductService : BaseService, IProductService
{
    private readonly IBrandService _brandService;
    private readonly IOriginService _originService;
    private readonly ICategoryService _categoryService;
    private readonly IUnitService _unitService;
    private readonly IVendorService _vendorService;
    private readonly IImageService _imageService;
    private readonly IInventoryService _inventoryService;

    public ProductService(IUnitOfWork unitOfWork,
        IBrandService brandService,
        IOriginService originService,
        ICategoryService categoryService,
        IUnitService unitService,
        IVendorService vendorService,
        IImageService imageService,
        IInventoryService inventoryService)
        : base(unitOfWork)
    {
        _brandService = brandService;
        _originService = originService;
        _categoryService = categoryService;
        _unitService = unitService;
        _vendorService = vendorService;
        _imageService = imageService;
        _inventoryService = inventoryService;
    }

    public async Task<List<ProductResp>> GetAll()
    {
        var entities = await UnitOfWork.ProductRepo.GetAll();
        return entities.Select(x => new ProductResp(x)).OrderByDescending(x => x.ProductId).ToList();
    }

    public async Task<ProductResp> GetById(long id)
    {
        var entity = await UnitOfWork.ProductRepo.GetByIdAsync(id);

        if (entity is null) throw new NotFoundException("Product " + id + " not found");

        return new ProductResp(entity);
    }

    public async Task<long> CreateBy(ProductReq req, List<IFormFile> a)
    {
        var productId = await CreateProduct(req);

        foreach (var image in a)
        {
            await _imageService.CreateBy(productId, image);
        }

        await _inventoryService.CreateBy(productId, req.MinQuantity, req.Quantity, (decimal)req.CostPrice!);

        return productId;
    }

    public async Task<long> CreateByV2(ProductReq req)
    {
        var productId = await CreateProduct(req);

        await _inventoryService.CreateBy(productId, req.MinQuantity, req.Quantity, (decimal)req.CostPrice!);

        return productId;
    }
    
    private async Task<long> CreateProduct(ProductReq req)
    {
        await ValidCreateBy(req);
        var entity = new Data.Entity.ProductEntity.Product();
        SetValue(entity, req);

        await UnitOfWork.ProductRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.ProductId;
    }

    public async Task<long> UpdateBy(ProductReq req, long id)
    {
        await ValidUpdateBy(id, req);

        var entity = await UnitOfWork.ProductRepo.GetByIdAsync(id);
        SetValue(entity!, req);
        if (req.Images.Count > 0)
        {
            if (entity!.Images.Count > 0)
            {
                    await _imageService.DeleteBy(entity.ProductId);
            }
            foreach (var item in req.Images)
            {
                await _imageService.CreateBy(id, item);
            }
        }
        
        await _inventoryService.UpdateBy(id, req.MinQuantity, req.Quantity, (decimal)req.CostPrice!);
        
        UnitOfWork.ProductRepo.Update(entity!);
        await UnitOfWork.CompleteAsync();
        return entity!.ProductId;
    }

    private async Task ValidCreateBy(ProductReq req)
    {
        await ValidExistsByCode(req);
        await _brandService.ValidNotExistsByBrandId(req.BrandId);
        await _originService.ValidNotExistsByOriginId(req.OriginId);
        await _categoryService.ValidNotExistsByCategoryId(req.CategoryId);
        await _unitService.ValidNotExistsByUnitId(req.UnitId);
        await _vendorService.ValidNotExistsByVendorId(req.VendorId);
    }

    private async Task ValidUpdateBy(long id, ProductReq req)
    {
        await ValidExistsByCode(req, id);
        await _brandService.ValidNotExistsByBrandId(req.BrandId);
        await _originService.ValidNotExistsByOriginId(req.OriginId);
        await _categoryService.ValidNotExistsByCategoryId(req.CategoryId);
        await _unitService.ValidNotExistsByUnitId(req.UnitId);
        await _vendorService.ValidNotExistsByVendorId(req.VendorId);
    }

    private async Task ValidExistsByCode(ProductReq req, long? id = null)
    {
        var existsBy = await UnitOfWork.ProductRepo.ExistsByCode(req.Code!, id);
        if (existsBy)
        {
            throw new NotFoundException("Exit by code: " + req.Code);
        }
    }

    private static void SetValue(Data.Entity.ProductEntity.Product entity, ProductReq req)
    {
        entity.Code = req.Code!;
        entity.Name = req.Name!;
        entity.ShortDescription = req.ShortDescription;
        entity.OriginId = req.OriginId;
        entity.CategoryId = req.CategoryId;
        entity.BrandId = req.BrandId;
        entity.VendorId = req.VendorId;
        entity.Description = req.Description;
        entity.Volume = req.Volume;
        entity.UnitId = req.UnitId;
        entity.Discount = req.Discount;
        entity.GiaNiemYet = req.GiaNiemYet;
        entity.ChietKhauNhap = req.ChietKhauNhap;
        entity.CostPrice = req.CostPrice;
        entity.Vat = req.Vat;
        entity.Barcode = req.Barcode!;
        entity.IsActive = req.IsActive;
    }

    public async Task DeleteBy(long id)
    {
        var entity = await UnitOfWork.ProductRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Product " + id + " not found");
        entity.IsActive = false;

        UnitOfWork.ProductRepo.Update(entity);
        await UnitOfWork.CompleteAsync();
    }

    public XLWorkbook Export(List<ProductResp> products)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Danh sách sản phẩm");
        worksheet.Columns().AdjustToContents();

        var titleRow = worksheet.Range("A1:H1");
        titleRow.Merge();
        worksheet.Cell(1, 1).Value = "Danh sách sản phẩm";
        titleRow.Style.Font.Bold = true;
        titleRow.Style.Font.FontColor = XLColor.Black;
        titleRow.Style.Font.FontSize = 30;

        var headerRow = worksheet.Range("A3:L3");
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRow.Style.Font.FontColor = XLColor.Black;

        worksheet.Cell(3, 1).Value = "ID";
        worksheet.Cell(3, 2).Value = "Tên sản phẩm";
        worksheet.Cell(3, 3).Value = "Mã sản phẩm";
        worksheet.Cell(3, 4).Value = "Loại sản phẩm";
        worksheet.Cell(3, 5).Value = "Xuất xứ";
        worksheet.Cell(3, 6).Value = "Nhãn hiêu";
        worksheet.Cell(3, 7).Value = "Nhà cung cấp";
        worksheet.Cell(3, 8).Value = "Giá bán lẻ";
        worksheet.Cell(3, 9).Value = "Giá sỉ";
        worksheet.Cell(3, 10).Value = "Mã vạch";
        worksheet.Cell(3, 11).Value = "Mô tả";
        worksheet.Cell(3, 12).Value = "Trạng thái";
        
        for (int col = 1; col <= 12; col++)
        {
            worksheet.Cell(3, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(3, col).Style.Border.OutsideBorderColor = XLColor.Black;
        }

        var row = 4;
        foreach (var item in products)
        {
            worksheet.Cell(row, 1).Value = item.ProductId;
            worksheet.Cell(row, 2).Value = item.Name;
            worksheet.Cell(row, 3).Value = item.Code;
            worksheet.Cell(row, 4).Value = item.Category.Name;
            worksheet.Cell(row, 5).Value = item.Origin?.Name;
            worksheet.Cell(row, 6).Value = item.Brand?.Name;
            worksheet.Cell(row, 7).Value = item.Vendor?.Name;
            worksheet.Cell(row, 8).Value = item.ChietKhauNhap;
            worksheet.Cell(row, 9).Value = item.Category.Name;
            worksheet.Cell(row, 10).Value = item.Barcode;
            worksheet.Cell(row, 11).Value = item.Description;
            worksheet.Cell(row, 12).Value = item.IsActive ? "Hoạt động" : "Vô hiệu hoá";

            for (int col = 1; col <= 12; col++)
            {
                worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(row, col).Style.Border.OutsideBorderColor = XLColor.Black;
            }

            row++;
        }

        worksheet.Columns().AdjustToContents();
        return workbook;
    }

    public async Task ImportExcel(IFormFile file)
    {
        if (file == null || file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            throw new BadRequestException("File không đúng định dạng");
        }
        
        using var stream = file.OpenReadStream();
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet(1);
        var rows = worksheet.RowsUsed().Skip(4);
        foreach (var row in rows)
        {
            var product = new ProductReq();
        
            product.Code = row.Cell(2).Value.ToString();
            product.Name = row.Cell(3).Value.ToString();
            product.ShortDescription = row.Cell(4).Value.ToString();
            product.OriginId = (long?)row.Cell(5).Value;
            product.CategoryId = (long)(row.Cell(6)).Value;
            product.BrandId = (long)(row.Cell(7)).Value;
            product.VendorId = (long)(row.Cell(8)).Value;
            product.Description = row.Cell(9).ToString();
            product.Volume = (float)(row.Cell(10)).Value;
            product.UnitId = (long)(row.Cell(11)).Value;
            product.Discount = (short)(row.Cell(12)).Value;
            product.Vat = (row.Cell(13).Value.ToString() == string.Empty
                ? 0
                : decimal.Parse((row.Cell(13)).Value.ToString()));
            product.Barcode = row.Cell(14).ToString();
            product.GiaNiemYet = row.Cell(15).Value.ToString() == string.Empty
                ? 0
                : decimal.Parse((row.Cell(15)).Value.ToString());
            product.ChietKhauNhap = (row.Cell(16).Value.ToString() == string.Empty
                ? 0
                : decimal.Parse((row.Cell(16)).Value.ToString()));
            product.CostPrice = product.GiaNiemYet - (product.GiaNiemYet * product.ChietKhauNhap / 100);
            product.Quantity = (int)(row.Cell(17)).Value;
            product.MinQuantity = (int)(row.Cell(18)).Value;
            product.IsActive = true;

            await CreateByV2(product);
        }
    }
}