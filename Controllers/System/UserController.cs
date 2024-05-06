using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Data.Dto.User;
using ptdn_net.Services.System.Interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.System;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [HasPermission(UserPermission.View)]
    public async Task<List<UserResp>> GetAll()
    {
        return await _userService.GetAllUsers();
    }

    [HttpGet("{id:long}")]
    [HasPermission(UserPermission.View)]
    public async Task<UserResp> GetById(long id)
    {
        return await _userService.GetById(id);
    }

    [HttpPost]
    [HasPermission(UserPermission.Create)]
    public async Task<ActionResult<long>> Create([FromBody] UserReq req)
    {
        var result = await _userService.Create(req);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [HasPermission(UserPermission.Update)]
    public async Task<ActionResult<long>> Update([FromBody] UserReq req, long id)
    {
        var result = await _userService.Update(id, req);
        return Ok(result);
    }
    
    [HttpPut("reset-password")]
    [HasPermission(UserPermission.Update)]
    public async Task<ActionResult<long>> ResetPassword([FromBody] ResetPass req)
    {
        var result = await _userService.ResetPassword(req);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [HasPermission(UserPermission.Delete)]
    public async Task<long> DeleteById(long id)
    {
        return await _userService.Delete(id);
    }
}