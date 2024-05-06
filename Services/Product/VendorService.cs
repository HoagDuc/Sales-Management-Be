using System.Net;
using AutoMapper;
using BT_MVC_Web.Exceptions;
using ClosedXML.Excel;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services;

public class VendorService : BaseService, IVendorService
{
    private readonly IMapper _mapper;

    public VendorService(IUnitOfWork unitOfWork,
        IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<List<VendorResp>> GetAll()
    {
        var entities = await UnitOfWork.VendorRepo.GetAll();
        return _mapper.Map<List<VendorResp>>(entities).OrderBy(x => x.VendorId).ToList();
    }

    public async Task<VendorResp> GetById(long id)
    {
        var entity = await UnitOfWork.VendorRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Vendor " + id + " not found");

        return _mapper.Map<VendorResp>(entity);
    }

    public async Task<long> CreateBy(VendorReq req)
    {
        await ValidCreateBy(req);

        var entity = _mapper.Map<Vendor>(req);

        await UnitOfWork.VendorRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.VendorId;
    }

    public async Task<long> UpdateBy(VendorReq req, long id)
    {
        await ValidUpdateBy(id, req);

        var entity = await UnitOfWork.VendorRepo.GetByIdAsync(id);
        _mapper.Map(req, entity);

        UnitOfWork.VendorRepo.Update(entity!);
        await UnitOfWork.CompleteAsync();
        return entity!.VendorId;
    }

    private async Task ValidCreateBy(VendorReq req)
    {
        await ValidExistsByCode(req);
    }

    private async Task ValidUpdateBy(long id, VendorReq req)
    {
        await ValidExistsByCode(req, id);
    }

    private async Task ValidExistsByCode(VendorReq req, long? id = null)
    {
        var existsBy = await UnitOfWork.VendorRepo.ExistsByCode(req.Code!, id);
        if (existsBy)
        {
            throw new NotFoundException("Exit by code: " + req.Code);
        }
    }

    public async Task DeleteBy(long id)
    {
        var entity = await UnitOfWork.VendorRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Vendor " + id + " not found");
        UnitOfWork.VendorRepo.Remove(entity);
        await UnitOfWork.CompleteAsync();
    }

    public async Task ValidNotExistsByVendorId(long? vendorId)
    {
        var existsBy = await UnitOfWork.VendorRepo.ExistsById(vendorId);
        if (!existsBy)
        {
            throw new NotFoundException("Not exit by vendor id: " + vendorId);
        }
    }

    public async Task<XLWorkbook> ExportExcel()
    {
        throw new NotImplementedException();
    }
    
    public XLWorkbook Export(List<VendorResp> vendors)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Danh sách nhà cung cấp");
        
        var titleRow = worksheet.Range("A1:E1");
        titleRow.Merge();
        worksheet.Cell(1, 1).Value = "Danh sách nhà cung cấp";
        titleRow.Style.Font.Bold = true;
        titleRow.Style.Font.FontColor = XLColor.Black;
        titleRow.Style.Font.FontSize = 30;
        
        var headerRow = worksheet.Range("A3:H3");
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRow.Style.Font.FontColor = XLColor.White;

        worksheet.Cell(3, 1).Value = "ID";
        worksheet.Cell(3, 2).Value = "Mã nhà cung cấp";
        worksheet.Cell(3, 3).Value = "Tên nhà cung cấp";
        worksheet.Cell(3, 4).Value = "Email";
        worksheet.Cell(3, 5).Value = "Số điện thoại";
        worksheet.Cell(3, 6).Value = "Nợ";
        worksheet.Cell(3, 7).Value = "Website";
        worksheet.Cell(3, 8).Value = "Cụ thể";
        for (int col = 1; col <= 8; col++)
        {
            worksheet.Cell(3, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(3, col).Style.Border.OutsideBorderColor = XLColor.Black;
        }
        
        var row = 4;
        foreach (var vendor in vendors)
        {
            worksheet.Cell(row, 1).Value = vendor.VendorId;
            worksheet.Cell(row, 2).Value = vendor.Code;
            worksheet.Cell(row, 3).Value = vendor.Name;
            worksheet.Cell(row, 4).Value = vendor.Email;
            worksheet.Cell(row, 5).Value = vendor.Phone;
            worksheet.Cell(row, 6).Value = vendor.Debt;
            worksheet.Cell(row, 7).Value = vendor.Website;
            worksheet.Cell(row, 8).Value = vendor.Address;

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
}