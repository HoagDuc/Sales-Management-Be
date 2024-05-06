using ptdn_net.Data.Dto.BaoCao;

namespace ptdn_net.Services.BaoCao.Interfaces;

public interface IBaoCaoBanHangService
{
    Task<ThongTinGiaoHangResp> GetThongTinGiaoHang(DateTime? ngayBatDau, DateTime? ngayKetThuc);
    
    Task<string> GetTongTien(DateTime? ngayBatDau, DateTime? ngayKetThuc);
    
    Task<List<RevenueResp>> GetDoanhThuTuan();
    
    Task<List<RevenueResp>> GetDoanhThuThang();

    Task<List<RevenueResp>> GetDoanhThuNam();
    
    Task<List<RevenueV2Resp>> GetDoanhThuTuanV2();
    
    Task<List<RevenueV2Resp>> GetDoanhThuThangV2();
    
    Task<List<RevenueV2Resp>> GetDoanhThuNamV2();
}