using Microsoft.AspNetCore.Mvc;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Controllers.Product;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class UnitController : ControllerBase
{
    private readonly IUnitService _unitService;

    public UnitController(IUnitService unitService)
    {
        _unitService = unitService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _unitService.GetAll());
    }
    
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        return Ok(await _unitService.GetById(id));
    }

    [HttpPost]
    public async Task<ActionResult<long>> CreateBy([FromBody] UnitReq req)
    {
        var result = await _unitService.CreateBy(req);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<long>> UpdateBy(long id, [FromBody] UnitReq req)
    {
        var result = await _unitService.UpdateBy(req, id);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<string>> DeleteById(long id)
    {
        await _unitService.DeleteBy(id);
        return Ok();
    }
}