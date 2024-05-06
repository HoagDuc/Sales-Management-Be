using AutoMapper;
using BT_MVC_Web.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using ptdn_net.Data.Dto.User;
using ptdn_net.Data.Entity.SystemEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.System.Interfaces;
using ptdn_net.Utils;

namespace ptdn_net.Services.System;

public class UserService : BaseService, IUserService
{
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache) : base(unitOfWork)
    {
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public async Task<UserResp> Update(long id, UserReq req)
    {
        await ValidUpdateBy(id, req);
        var entity = await UnitOfWork.UserRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("User " + id + " not found");
        await SetValue(entity!, req);

        UnitOfWork.UserRepo.Update(entity);
        await UnitOfWork.CompleteAsync();
        
        var user = await UnitOfWork.UserRepo.GetByUsernameAsync(entity.Username);

        if (_memoryCache.TryGetValue<User>(entity.Username, out _))
        {
            _memoryCache.Set(entity.Username, user, DateTimeOffset.Now.AddHours(1));
        }

        if (!entity.IsActive)
        {
            _memoryCache.Remove(entity.Username);
        }

        return _mapper.Map<UserResp>(entity);
    }

    public async Task<User> GetByUsername(string username)
    {
        var user = await UnitOfWork.UserRepo.GetByUsernameAsync(username);
        if (user is null) throw new NotFoundException("User " + username + " not found");
        return user;
    }

    public async Task<List<UserResp>> GetAllUsers()
    {
        var users = await UnitOfWork.UserRepo.GetAllUser();
        return _mapper.Map<List<UserResp>>(users.ToList());
    }

    public async Task<long> Create(UserReq req)
    {
        await ValidCreateBy(req);
        var entity = new User();
        await SetValue(entity, req);
        entity.Password = BCrypt.Net.BCrypt.HashPassword(req.Password);
        await UnitOfWork.UserRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();

        return entity.UserId;
    }

    public async Task<UserResp> GetById(long id)
    {
        var users = await UnitOfWork.UserRepo.GetAllUser();
        var user = users.Find(x => x.UserId == id);
        if (user is null) throw new NotFoundException("User " + id + " not found");

        return _mapper.Map<UserResp>(user);
    }

    public async Task<long> ResetPassword(ResetPass req)
    {
        if(req.PasswordNew != req.PasswordConfirm) throw new BadRequestException("Password not match");
        var currUsername = SessionUtil.GetCurrUsername();
        var user = await GetByUsername(currUsername);
        if (!BCrypt.Net.BCrypt.Verify(req.PasswordOld, user.Password))
            throw new BadRequestException("Password old is incorrect!!!");
        user.Password = BCrypt.Net.BCrypt.HashPassword(req.PasswordNew);
        UnitOfWork.UserRepo.Update(user);
        await UnitOfWork.CompleteAsync();
        return user.UserId;
    } 

    public async Task<long> Delete(long id)
    {
        var entity = await UnitOfWork.UserRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("User " + id + " not found");
        entity.IsActive = false;
        UnitOfWork.UserRepo.Update(entity);
        await UnitOfWork.CompleteAsync();
        if (_memoryCache.TryGetValue<User>(entity.Username, out _))
        { 
            _memoryCache.Remove(entity.Username);
        }
        return entity.UserId;
    }

    public Task<User?> GetByEmail(string email)
    {
        return UnitOfWork.UserRepo.GetByCondition(x => x.Email == email);
    }

    private static Task SetValue(User entity, UserReq req)
    {
        entity.Username = req.Username!;
        entity.Email = req.Email!;
        entity.Phone = req.Phone;
        entity.Fullname = req.Fullname;
        entity.Dob = req.Dob;
        entity.Address = req.Address;
        entity.Avatar = req.Avatar;
        entity.RoleId = (long)req.RoleId!;
        entity.Gender = req.Gender;
        entity.IsActive = req.IsActive;
        return Task.CompletedTask;
    }

    private async Task ValidCreateBy(UserReq req)
    {
        await ValidExistsByUserName(req);
    }

    private async Task ValidUpdateBy(long id, UserReq req)
    {
        await ValidExistsByUserName(req, id);
    }

    private async Task ValidExistsByUserName(UserReq req, long? id = null)
    {
        var existsBy = await UnitOfWork.UserRepo.ExistsByUserName(req.Username!, id);
        if (existsBy) throw new NotFoundException("Exit by username: " + req.Username);
    }
}