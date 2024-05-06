using Microsoft.AspNetCore.Mvc;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class ProvinceController : ControllerBase
{
    private readonly IProvinceService _provinceService;

    public ProvinceController(IProvinceService provinceService)
    {
        _provinceService = provinceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _provinceService.GetAll());
    }
}