using ptdn_net.Const.Emun;
using ptdn_net.Data.Dto.BaoCao;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.BaoCao.Interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services.BaoCao;

public class DashboardService : BaseService, IDashboardService
{
    private readonly IOrderService _orderService;
    private readonly IRefundOrderService _refundOrderService;

    public DashboardService(IUnitOfWork unitOfWork
        , IOrderService orderService
        , IRefundOrderService refundOrderService)
        : base(unitOfWork)
    {
        _orderService = orderService;
        _refundOrderService = refundOrderService;
    }
    
    public async Task<KetQuaNgayResp?> GetKetQuaNgay()
    {
        var donXuat = await _orderService.GetAllFromDateV2(DateTime.Now.Date, DateTime.Now.Date);
        var donTra = await _refundOrderService.GetAllFromDate(DateTime.Now.Date, DateTime.Now.Date);
        
        return new KetQuaNgayResp
        {
            DoanhThu = donXuat.Sum(x => x.TotalAmount),
            DonHangMoi = donXuat.Count(),
            DonTraHang = donTra.Count(),
            DonHangHuy = donXuat.Where(x => x.Status == (short)Status.Huy).Count()
        };
    }

    public async Task<DonHangChoXuLyResp?> GetDonHangChoXuLy(DateTime fromDate, DateTime toDate)
    {
        var donXuat = await _orderService.GetAllFromDate(fromDate, toDate);
        var donTra = await _refundOrderService.GetAllFromDate(fromDate, toDate);
        
        return new DonHangChoXuLyResp
        {
            ChoXuLy = donXuat.Where(x => x.Status == (short)Status.XuLy).Count(),
            XacNhan = donXuat.Where(x => x.Status == (short)Status.XacNhan).Count(),
            DangGiaoHang = donXuat.Where(x => x.Status == (short)Status.DangVanChuyen).Count(),
            ThanhToan = donXuat.Where(x => x.Status == (short)Status.ThanhToan).Count(),
            Huy = donXuat.Where(x => x.Status == (short)Status.Huy).Count(),
            HoanHang = donTra.Count()
        };
    }
}