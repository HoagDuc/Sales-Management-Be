using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Services.interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.Product;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    [HasPermission(BrandPermission.View)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _brandService.GetAll());
    }
    
    [HttpGet("{id:long}")]
    [HasPermission(BrandPermission.View)]
    public async Task<IActionResult> GetById(long id)
    {
        return Ok(await _brandService.GetById(id));
    }

    [HttpPost]
    [HasPermission(BrandPermission.Create)]
    public async Task<ActionResult<long>> CreateBy([FromBody] BrandReq req)
    {
        var result = await _brandService.CreateBy(req);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [HasPermission(BrandPermission.Update)]
    public async Task<ActionResult<long>> UpdateBy(long id, [FromBody] BrandReq req)
    {
        var result = await _brandService.UpdateBy(req, id);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [HasPermission(BrandPermission.Delete)]
    public async Task<ActionResult<string>> DeleteById(long id)
    {
        await _brandService.DeleteBy(id);
        return Ok();
    }
}