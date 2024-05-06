using AutoMapper;
using BT_MVC_Web.Exceptions;
using ptdn_net.Data.Dto.Location;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services;

public class DistrictService : BaseService, IDistrictService
{
    private readonly IMapper _mapper;

    public DistrictService(IUnitOfWork unitOfWork,
        IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<List<DistrictResp>> GetByProvinceCode(string provinceCode)
    {
        var data = await UnitOfWork.DistrictRepo.GetByProvinceCode(provinceCode);
        return _mapper.Map<List<DistrictResp>>(data);
    }

    public async Task ValidNotExistsByDistrictId(long? id)
    {
        var existsBy = await UnitOfWork.DistrictRepo.ExistsById(id);
        if (!existsBy)
        {
            throw new NotFoundException("District not exit by id: " + id);
        }
    }
}