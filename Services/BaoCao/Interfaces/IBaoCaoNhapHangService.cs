using ptdn_net.Data.Dto.BaoCao;

namespace ptdn_net.Services.BaoCao.Interfaces;

public interface IBaoCaoNhapHangService
{
    Task<ThongTinNhapHangResp?> GetThongTinGiaoHang(DateTime? ngayBatDau, DateTime? ngayKetThuc);
    
    Task<string> GetTongTien(DateTime? ngayBatDau, DateTime? ngayKetThuc);
    
    Task<List<RevenueEntryResp>> GetChiTieuTuan();
    
    Task<List<RevenueEntryResp>> GetChiTieuThang();

    Task<List<RevenueEntryResp>> GetChiTieuNam();
}