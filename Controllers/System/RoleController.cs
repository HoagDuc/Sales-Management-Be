using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Data.Dto.Auth;
using ptdn_net.Services.System.Interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.System;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    [HasPermission(RolePermission.View)]
    public async Task<ActionResult<List<RoleResp>>> GetAll()
    {
        var result = await _roleService.GetAll();
        return Ok(result);
    }
    
    [HttpGet("{id:long}")]
    [HasPermission(RolePermission.View)]
    public async Task<ActionResult<RoleResp>> GetById(long id)
    {
        var result = await _roleService.GetByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPost]
    [HasPermission(RolePermission.Create)]
    public async Task<ActionResult<long>> CreateBy([FromBody] RoleReq req)
    {
        var result = await _roleService.CreateBy(req);
        return Ok(result);
    }
    
    [HttpPut("{id:long}")]
    [HasPermission(RolePermission.Update)]
    public async Task<ActionResult<long>> UpdateBy([FromBody] RoleReq req, long id)
    {
        var result = await _roleService.UpdateBy(id, req);
        return Ok(result);
    }
}