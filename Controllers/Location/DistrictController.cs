using Microsoft.AspNetCore.Mvc;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class DistrictController : ControllerBase
{
    private readonly IDistrictService _districtService;

    public DistrictController(IDistrictService districtService)
    {
        _districtService = districtService;
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByProvinceCode(string code)
    {
        return Ok(await _districtService.GetByProvinceCode(code));
    }
}