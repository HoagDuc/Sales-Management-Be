using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Services.interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.Product;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class OriginController : ControllerBase
{
    private readonly IOriginService _originService;

    public OriginController(IOriginService originService)
    {
        _originService = originService;
    }

    [HttpGet]
    [HasPermission(OriginPermission.View)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _originService.GetAll());
    }
    
    [HttpGet("{id:long}")]
    [HasPermission(OriginPermission.View)]
    public async Task<IActionResult> GetById(long id)
    {
        return Ok(await _originService.GetById(id));
    }

    [HttpPost]
    [HasPermission(OriginPermission.Create)]
    public async Task<ActionResult<long>> CreateBy([FromBody] OriginReq req)
    {
        var result = await _originService.CreateBy(req);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [HasPermission(OriginPermission.Update)]
    public async Task<ActionResult<long>> UpdateBy(long id, [FromBody] OriginReq req)
    {
        var result = await _originService.UpdateBy(req, id);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [HasPermission(OriginPermission.Delete)]
    public async Task<ActionResult<string>> DeleteById(long id)
    {
        await _originService.DeleteBy(id);
        return Ok();
    }
}