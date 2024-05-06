using AutoMapper;
using BT_MVC_Web.Exceptions;
using ClosedXML.Excel;
using ptdn_net.Const;
using ptdn_net.Const.Emun;
using ptdn_net.Data.Dto.Customer;
using ptdn_net.Data.Entity.CustomerEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;
using ptdn_net.Utils;

namespace ptdn_net.Services;

public class CustomerService : BaseService, ICustomerService
{
    private readonly ICustomerGroupService _customerGroupService;
    private readonly IProvinceService _provinceService;
    private readonly IDistrictService _districtService;
    private readonly ISubDistrictService _subDistrictService;
    private readonly IMapper _mapper;

    public CustomerService(IUnitOfWork unitOfWork,
        ICustomerGroupService customerGroupService,
        IProvinceService provinceService,
        IDistrictService districtService,
        ISubDistrictService subDistrictService,
        IMapper mapper)
        : base(unitOfWork)
    {
        _customerGroupService = customerGroupService;
        _provinceService = provinceService;
        _districtService = districtService;
        _subDistrictService = subDistrictService;
        _mapper = mapper;
    }

    public async Task<List<CustomerResp>> GetAll()
    {
        var entities = await UnitOfWork.CustomerRepo.GetAll();
        return _mapper.Map<List<CustomerResp>>(entities).OrderByDescending(x => x.CreateAt).ToList();
    }

    public async Task<CustomerResp> GetById(Guid id)
    {
        var entity = await UnitOfWork.CustomerRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Customer " + id + " not found");

        return _mapper.Map<CustomerResp>(entity);
    }

    public async Task<Guid> CreateBy(CustomerReq req)
    {
        await ValidCreateBy(req);

        var entity = _mapper.Map<Customer>(req);

        await UnitOfWork.CustomerRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.CustomerId;
    }

    public async Task<Guid> UpdateBy(CustomerReq req, Guid id)
    {
        await ValidUpdateBy(id, req);

        var entity = await UnitOfWork.CustomerRepo.GetByIdAsync(id);
        _mapper.Map(req, entity);

        UnitOfWork.CustomerRepo.Update(entity!);
        await UnitOfWork.CompleteAsync();
        return entity!.CustomerId;
    }

    private async Task ValidCreateBy(CustomerReq req)
    {
        await ValidExistsByCode(req);
        if (req.CustomerGroupId != null)
            await _customerGroupService.ValidNotExistsByCustomerGroupId(req.CustomerGroupId);
        if (req.ProvinceId != null) await _provinceService.ValidNotExistsByProvinceId(req.ProvinceId);
        if (req.DistrictId != null) await _districtService.ValidNotExistsByDistrictId(req.DistrictId);
        if (req.SubDistrictId != null) await _subDistrictService.ValidNotExistsBySubDistrictId(req.SubDistrictId);
    }

    private async Task ValidUpdateBy(Guid id, CustomerReq req)
    {
        await ValidExistsByCode(req, id);
        if (req.CustomerGroupId != null)
            await _customerGroupService.ValidNotExistsByCustomerGroupId(req.CustomerGroupId);
        if (req.ProvinceId != null) await _provinceService.ValidNotExistsByProvinceId(req.ProvinceId);
        if (req.DistrictId != null) await _districtService.ValidNotExistsByDistrictId(req.DistrictId);
        if (req.SubDistrictId != null) await _subDistrictService.ValidNotExistsBySubDistrictId(req.SubDistrictId);
    }

    private async Task ValidExistsByCode(CustomerReq req, Guid? id = null)
    {
        var existsBy = await UnitOfWork.CustomerRepo.ExistsByCode(req.Code!, id);
        if (existsBy)
        {
            throw new NotFoundException("Exit by code: " + req.Code);
        }
    }

    public async Task ValidNotExistsByCustomerId(Guid? id)
    {
        var existsBy = await UnitOfWork.CustomerRepo.ExistsById(id);
        if (!existsBy)
        {
            throw new NotFoundException("Customer not exit by id: " + id);
        }
    }

    public async Task UpdateDebtBy(Guid id, decimal? debt)
    {
        var entity = await UnitOfWork.CustomerRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Customer " + id + " not found");
        
        entity.Debt += debt;
        UnitOfWork.CustomerRepo.Update(entity);
    }

    public XLWorkbook Export(List<CustomerResp> customers)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Danh sách khách hàng");
        worksheet.Columns().AdjustToContents();

        var titleRow = worksheet.Range("A1:L1");
        titleRow.Merge();
        worksheet.Cell(1, 1).Value = "Danh sách khách hàng";
        titleRow.Style.Font.Bold = true;
        titleRow.Style.Font.FontColor = XLColor.Black;
        titleRow.Style.Font.FontSize = 30;

        var headerRow = worksheet.Range("A3:L3");
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRow.Style.Font.FontColor = XLColor.Black;

        worksheet.Cell(3, 1).Value = "STT";
        worksheet.Cell(3, 2).Value = "ID";
        worksheet.Cell(3, 3).Value = "Mã khách hàng";
        worksheet.Cell(3, 4).Value = "Tên khách hàng";
        worksheet.Cell(3, 5).Value = "Ngày sinh";
        worksheet.Cell(3, 6).Value = "Giới tính";
        worksheet.Cell(3, 7).Value = "Email";
        worksheet.Cell(3, 8).Value = "Số điện thoại";
        worksheet.Cell(3, 9).Value = "Địa chỉ";
        worksheet.Cell(3, 10).Value = "Website";
        worksheet.Cell(3, 11).Value = "Nợ";
        worksheet.Cell(3, 12).Value = "Loại khách hàng";
        
        for (int col = 1; col <= 12; col++)
        {
            worksheet.Cell(3, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(3, col).Style.Border.OutsideBorderColor = XLColor.Black;
        }

        var stt = 1;
        var row = 4;
        foreach (var item in customers)
        {
            worksheet.Cell(row, 1).Value = stt++;
            worksheet.Cell(row, 2).Value = item.CustomerId.ToString();
            worksheet.Cell(row, 3).Value = item.Code;
            worksheet.Cell(row, 4).Value = item.Name;
            worksheet.Cell(row, 5).Value = item.Dob?.Date.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 6).Value = item.Gender switch
            {
                Gender.Male => "Nam",
                Gender.Female => "Nữ",
                _ => "Khác"
            };
            worksheet.Cell(row, 7).Value = item.Email;
            worksheet.Cell(row, 8).Value = item.Phone;
            worksheet.Cell(row, 9).Value = item.Address;
            worksheet.Cell(row, 10).Value = item.Website;
            worksheet.Cell(row, 11).Value = item.Debt;
            worksheet.Cell(row, 12).Value = item.CustomerGroupId != null ? item.CustomerGroup.Name : string.Empty;

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

    public async Task<Guid> UpdateNoBy(Guid id)
    {
        var entity = await UnitOfWork.CustomerRepo.GetByIdAsync(id);

        if (entity is null) throw new NotFoundException("Customer " + id + " not found");
        entity!.Debt = 0;

        var orders = await UnitOfWork.OrderRepo.GetByCustomerId(id);
        foreach (var item in orders)
        {
            item.AmountDue = 0;
            item.AmountPaid = item.TotalAmount;
            item.AmountRemaining = 0;

            if (item.Status == (short)Status.ThanhToan)
            {
                item.Status = (short)Status.HoanThanh;
            }
            
            UnitOfWork.OrderRepo.Update(item);
        }

        UnitOfWork.CustomerRepo.Update(entity);
        await UnitOfWork.CompleteAsync();
        return entity.CustomerId;
    }

    public async Task DeleteBy(Guid id)
    {
        var entity = await UnitOfWork.CustomerRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Customer " + id + " not found");
        UnitOfWork.CustomerRepo.Remove(entity);
        await UnitOfWork.CompleteAsync();
    }
}