using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Services.BaoCao.Interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.BaoCao;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class BaoCaoNhapHangController : ControllerBase
{
    private readonly IBaoCaoNhapHangService _baoCaoNhapHangService;
    
    public BaoCaoNhapHangController(IBaoCaoNhapHangService baoCaoNhapHangService)
    {
        _baoCaoNhapHangService = baoCaoNhapHangService;
    }

    [HttpGet("get-thong-tin-nhap-hang")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetThongTinNhapHang([FromQuery] DateTime? ngayBatDau, DateTime?ngayKetThuc)
    {
        return Ok( await _baoCaoNhapHangService.GetThongTinGiaoHang(ngayBatDau, ngayKetThuc));
    }
    
    [HttpGet("get-tong-tien")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetTongTien([FromQuery] DateTime? ngayBatDau, DateTime? ngayKetThuc)
    {
        return Ok( await _baoCaoNhapHangService.GetTongTien(ngayBatDau, ngayKetThuc));
    }
    
    [HttpGet("get-chi-tieu-tuan")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetChiTieuTuan()
    {
        return Ok( await _baoCaoNhapHangService.GetChiTieuTuan());
    }
    
    [HttpGet("get-chi-tieu-thang")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetChiTieuThang()
    {
        return Ok( await _baoCaoNhapHangService.GetChiTieuThang());
    }
    
    [HttpGet("get-chi-tieu-nam")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetChiTieuNam()
    {
        return Ok( await _baoCaoNhapHangService.GetChiTieuNam());
    }
}