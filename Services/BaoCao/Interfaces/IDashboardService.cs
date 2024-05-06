using ptdn_net.Data.Dto.BaoCao;

namespace ptdn_net.Services.BaoCao.Interfaces;

public interface IDashboardService
{
    Task<KetQuaNgayResp?> GetKetQuaNgay();
    
    Task<DonHangChoXuLyResp?> GetDonHangChoXuLy(DateTime fromDate, DateTime toDate);
}