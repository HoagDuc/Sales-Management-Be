using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Services.BaoCao.Interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.BaoCao;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class BaoCaoBanHangController : ControllerBase
{
    private readonly IBaoCaoBanHangService _baoCaoBanHangService;
    
    public BaoCaoBanHangController(IBaoCaoBanHangService baoCaoBanHangService)
    {
        _baoCaoBanHangService = baoCaoBanHangService;
    }

    [HttpGet("get-thong-tin-giao-hang")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetThongTinGiaoHang([FromQuery] DateTime? ngayBatDau, DateTime ?ngayKetThuc)
    {
        return Ok( await _baoCaoBanHangService.GetThongTinGiaoHang(ngayBatDau, ngayKetThuc));
    }
    
    [HttpGet("get-tong-tien")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetTongTien([FromQuery] DateTime? ngayBatDau, DateTime? ngayKetThuc)
    {
        return Ok( await _baoCaoBanHangService.GetTongTien(ngayBatDau, ngayKetThuc));
    }
    
    [HttpGet("get-chi-tieu-tuan")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetChiTieuTuan()
    {
        return Ok( await _baoCaoBanHangService.GetDoanhThuTuan());
    }
    
    [HttpGet("get-chi-tieu-thang")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetChiTieuThang()
    {
        return Ok( await _baoCaoBanHangService.GetDoanhThuThang());
    }
    
    [HttpGet("get-chi-tieu-nam")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetChiTieuNam()
    {
        return Ok( await _baoCaoBanHangService.GetDoanhThuNam());
    }
    
    [HttpGet("get-chi-tieu-tuan-v2")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetChiTieuTuanV2()
    {
        return Ok( await _baoCaoBanHangService.GetDoanhThuTuanV2());
    }
    
    [HttpGet("get-chi-tieu-thang-v2")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetChiTieuThangV2()
    {
        return Ok( await _baoCaoBanHangService.GetDoanhThuThangV2());
    }
    
    [HttpGet("get-chi-tieu-nam-v2")]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetChiTieuNamV2()
    {
        return Ok( await _baoCaoBanHangService.GetDoanhThuNamV2());
    }
}