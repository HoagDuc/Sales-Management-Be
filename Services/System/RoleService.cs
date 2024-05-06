using System.Diagnostics;
using AutoMapper;
using BT_MVC_Web.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using ptdn_net.Data.Dto.Auth;
using ptdn_net.Data.Entity.SystemEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.System.Interfaces;

namespace ptdn_net.Services.System;

public class RoleService : BaseService, IRoleService
{
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    public RoleService(IUnitOfWork unitOfWork,
        IMapper mapper, IMemoryCache memoryCache) : base(unitOfWork)
    {
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public async Task<List<RoleResp>> GetAll()
    {
        var result = await UnitOfWork.RoleRepo.GetAll();

        return _mapper.Map<List<RoleResp>>(result);
    }

    public async Task<Role> GetById(long id)
    {
        var entity = await UnitOfWork.RoleRepo.GetById(id);
        if (entity is null) throw new NotFoundException("Role " + id + " not found");

        return entity;
    }
    
    public async Task<RoleResp> GetByIdAsync(long id)
    {
        var entity = await GetById(id);
        return new RoleResp
        {
            RoleId = entity.RoleId,
            Description = entity.Description,
            IsActive = entity.IsActive,
            Name = entity.Name,
            Permissions = entity.Permissions.Select(x => x.PermissionId).ToList()
        };
    }

    public async Task<long> CreateBy(RoleReq req)
    {
        
        await ValidCreateBy(req);

        var entity = new Role
        {
            Name = req.Name!,
            Description = req.Description,
            IsActive = true
        };
        await UnitOfWork.RoleRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.RoleId;
    }

    public async Task<long> UpdateBy(long roleId, RoleReq req)
    {
        await ValidUpdateBy(req, roleId);

        var entity = await GetById(roleId);
        entity.Description = req.Description;
        entity.Name = req.Name!;
        entity.IsActive = req.IsActive;

        entity.RolePermissions = req.PermissionIds!.Select(x => new RolePermission
        {
            PermissionId = x,
            RoleId = entity.RoleId
        }).ToList();

        UnitOfWork.RoleRepo.Update(entity);
        await UnitOfWork.CompleteAsync();
        
        var users = await UnitOfWork.UserRepo.GetAllByCondition(x => x.RoleId == entity.RoleId);

        foreach (var user in users!)
        {
            var userUpdate = await UnitOfWork.UserRepo.GetByUsernameAsync(user.Username);
            if (_memoryCache.TryGetValue<User>(user.Username, out _))
            {
                _memoryCache.Set(user.Username, userUpdate, DateTimeOffset.Now.AddHours(1));
            }
        }

        
        return entity.RoleId;
    }


    private async Task ValidCreateBy(RoleReq req)
    {
        await ValidExistsByName(req);
    }

    private async Task ValidUpdateBy(RoleReq req, long id)
    {
        await ValidExistsByName(req, id);
    }
    
    private async Task ValidExistsByName(RoleReq req, long? id = null)
    {
        var existsBy = await UnitOfWork.RoleRepo.ExistsByUserName(req.Name!, id);
        if (existsBy) throw new NotFoundException("Exit by name: " + req.Name);
    }
}