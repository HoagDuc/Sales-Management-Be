using AutoMapper;
using BT_MVC_Web.Exceptions;
using ClosedXML.Excel;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services;

public class CategoryService : BaseService,ICategoryService
{
    private readonly IMapper _mapper;
    
    public CategoryService(IUnitOfWork unitOfWork,
        IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<List<CategoryResp>> GetAll()
    {
        var entities = await UnitOfWork.CategoryRepo.GetAll();
        return _mapper.Map<List<CategoryResp>>(entities).OrderBy(x => x.CategoryId).ToList();
    }

    public async Task<CategoryResp> GetById(long id)
    {
        var entity = await UnitOfWork.CategoryRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Category " + id + " not found");
        
        return _mapper.Map<CategoryResp>(entity);
    }

    public async Task<long> CreateBy(CategoryReq req)
    {
        await ValidCreateBy(req);

        var entity = _mapper.Map<Category>(req);

        await UnitOfWork.CategoryRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.CategoryId;
    }

    public async Task<long> UpdateBy(CategoryReq req, long id)
    {
        await ValidUpdateBy(id, req);

        var entity = await UnitOfWork.CategoryRepo.GetByIdAsync(id);
        _mapper.Map(req, entity);

        UnitOfWork.CategoryRepo.Update(entity!);
        await UnitOfWork.CompleteAsync();
        return entity!.CategoryId;
    }
    
    private async Task ValidCreateBy(CategoryReq req)
    {
        await ValidExistsByCode(req);
    }

    private async Task ValidUpdateBy(long id, CategoryReq req)
    {
        await ValidExistsByCode(req, id);
    }
    
    private async Task ValidExistsByCode(CategoryReq req, long? id = null)
    {
        var existsBy = await UnitOfWork.CategoryRepo.ExistsByCode(req.Code!, id);
        if (existsBy)
        {
            throw new NotFoundException("Exit by code: " + req.Code);
        }
    }

    public async Task DeleteBy(long id)
    {
        var entity = await UnitOfWork.CategoryRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Category " + id + " not found");
        UnitOfWork.CategoryRepo.Remove(entity);
        await UnitOfWork.CompleteAsync();
    }

    public async Task ValidNotExistsByCategoryId(long? id)
    {
        var existsBy = await UnitOfWork.CategoryRepo.ExistsById(id);
        if (!existsBy)
        {
            throw new NotFoundException("Not exit by category id: " + id);
        }
    }

    public XLWorkbook Export(List<CategoryResp> categories)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Danh sách loại sản phẩm");
        worksheet.Columns().AdjustToContents();

        var titleRow = worksheet.Range("A1:H1");
        titleRow.Merge();
        worksheet.Cell(1, 1).Value = "Danh sách loại sản phẩm";
        titleRow.Style.Font.Bold = true;
        titleRow.Style.Font.FontColor = XLColor.Black;
        titleRow.Style.Font.FontSize = 30;

        var headerRow = worksheet.Range("A3:D3");
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRow.Style.Font.FontColor = XLColor.Black;

        worksheet.Cell(3, 1).Value = "ID";
        worksheet.Cell(3, 2).Value = "Tên sản phẩm";
        worksheet.Cell(3, 3).Value = "Tồn kho";
        worksheet.Cell(3, 4).Value = "Giá nhập";
        for (int col = 1; col <= 4; col++)
        {
            worksheet.Cell(3, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(3, col).Style.Border.OutsideBorderColor = XLColor.Black;
        }

        var row = 4;
        foreach (var item in categories)
        {
            worksheet.Cell(row, 1).Value = item.CategoryId;
            worksheet.Cell(row, 2).Value = item.Code;
            worksheet.Cell(row, 3).Value = item.Name;
            worksheet.Cell(row, 4).Value = item.Description;

            for (int col = 1; col <= 4; col++)
            {
                worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(row, col).Style.Border.OutsideBorderColor = XLColor.Black;
            }

            row++;
        }

        worksheet.Columns().AdjustToContents();
        return workbook;
    }
}