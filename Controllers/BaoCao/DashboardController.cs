using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Services.BaoCao.Interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.BaoCao;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("get-ket-qua-ngay")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetKetQuaNgay()
    {
        return Ok( await _dashboardService.GetKetQuaNgay());
    }
    
    [HttpGet("get-don-hang-cho-xu-ly")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetDonHangChoXuLy([FromQuery] DateTime fromDate, DateTime toDate)
    {
        return Ok( await _dashboardService.GetDonHangChoXuLy(fromDate, toDate));
    }
}