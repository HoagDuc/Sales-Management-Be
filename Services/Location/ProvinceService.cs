using AutoMapper;
using BT_MVC_Web.Exceptions;
using ptdn_net.Data.Dto.Location;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services;

public class ProvinceService : BaseService, IProvinceService
{
    private readonly IMapper _mapper;
    
    public ProvinceService(IUnitOfWork unitOfWork,
        IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<List<ProvinceResp>> GetAll()
    {
        var data = await UnitOfWork.ProvinceRepo.GetAll();
        return _mapper.Map<List<ProvinceResp>>(data);
    }
    
    public async Task ValidNotExistsByProvinceId(long? id)
    {
        var existsBy = await UnitOfWork.ProvinceRepo.ExistsById(id);
        if (!existsBy)
        {
            throw new NotFoundException("Province not exit by id: " + id);
        }
    }
}