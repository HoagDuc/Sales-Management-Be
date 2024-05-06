using AutoMapper;
using BT_MVC_Web.Exceptions;
using ptdn_net.Data.Dto.Location;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services;

public class SubDistrictService : BaseService, ISubDistrictService
{
    private readonly IMapper _mapper;

    public SubDistrictService(IUnitOfWork unitOfWork,
        IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<List<SubDistrictResp>> GetByDistrictCode(string districtCode)
    {
        var data = await UnitOfWork.SubDistrictRepo.GetByDistrictCode(districtCode);
        return _mapper.Map<List<SubDistrictResp>>(data);
    }

    public async Task ValidNotExistsBySubDistrictId(long? id)
    {
        var existsBy = await UnitOfWork.SubDistrictRepo.ExistsById(id);
        if (!existsBy)
        {
            throw new NotFoundException("SubDistrict not exit by id: " + id);
        }
    }
}