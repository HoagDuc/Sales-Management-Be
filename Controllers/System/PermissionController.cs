using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Data.Dto.Auth;
using ptdn_net.Services.interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.System;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpPost]
    [HasPermission(PermissionPermission.Create)]
    public async Task<ActionResult<long>> CreateBy([FromBody] PermissionReq req)
    {
        var result = await _permissionService.CreateBy(req);
        return Ok(result);
    }
}