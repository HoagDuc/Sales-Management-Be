using Microsoft.AspNetCore.Mvc;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class SubDistrictController : ControllerBase
{
    private readonly ISubDistrictService _subDistrictService;

    public SubDistrictController(ISubDistrictService subDistrictService)
    {
        _subDistrictService = subDistrictService;
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByDistrictCode(string code)
    {
        return Ok(await _subDistrictService.GetByDistrictCode(code));
    }
}