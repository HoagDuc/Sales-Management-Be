using ptdn_net.Repository.interfaces;

namespace ptdn_net.Services;

public class BaseService
{
    protected readonly IUnitOfWork UnitOfWork;

    protected BaseService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
}