using ptdn_net.Const.Emun;
using ptdn_net.Data.Dto.BaoCao;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.BaoCao.Interfaces;
using ptdn_net.Services.interfaces;
using SuCoYKhoa.Data.Response.Dashboard;

namespace ptdn_net.Services.BaoCao;

public class BaoCaoNhapHangService : BaseService, IBaoCaoNhapHangService
{
    private readonly IPurchaseOrderService _purchaseOrderService;

    public BaoCaoNhapHangService(IUnitOfWork unitOfWork
        , IPurchaseOrderService purchaseOrderService) 
        : base(unitOfWork)
    {
        _purchaseOrderService = purchaseOrderService;
    }

    public async Task<ThongTinNhapHangResp?> GetThongTinGiaoHang(DateTime? ngayBatDau, DateTime? ngayKetThuc)
    {
        var purchaseOrderList = await _purchaseOrderService.GetAllFromDate(ngayBatDau, ngayKetThuc);
        var xuLy = purchaseOrderList.Count(x => x.Status == (short)Status.XuLy);
        var xacNhan = purchaseOrderList.Count(x => x.Status == (short)Status.XacNhan);
        var dangVanChuyen = purchaseOrderList.Count(x => x.Status == (short)Status.DangVanChuyen);
        var thanhToan = purchaseOrderList.Count(x => x.Status == (short)Status.ThanhToan);
        var hoanThanh = purchaseOrderList.Count(x => x.Status == (short)Status.HoanThanh);
        var huy = purchaseOrderList.Count(x => x.Status == (short)Status.Huy);
        var khongXacNhan = purchaseOrderList.Count(x => x.Status == (short)Status.KhongXacNhan);
        var tongSo = purchaseOrderList.Count();

        return new ThongTinNhapHangResp
        {
            XuLy = new QuantityAndPercentResp
            {
                Quantity = xuLy,
                Percent = tongSo == 0 ? 0 :(double)xuLy / tongSo * 100,
                Name = "Xử lý"
            },
            XacNhan = new QuantityAndPercentResp
            {
                Quantity = xacNhan,
                Percent = tongSo == 0 ? 0 : (double)xacNhan / tongSo * 100,
                Name = "Xác nhận"
            },
            DangVanChuyen = new QuantityAndPercentResp
            {
                Quantity = dangVanChuyen,
                Percent = tongSo == 0 ? 0 : (double)dangVanChuyen / tongSo * 100,
                Name = "Đang vận chuyển"
            },
            ThanhToan = new QuantityAndPercentResp
            {
                Quantity = thanhToan,
                Percent = tongSo == 0 ? 0 : (double)thanhToan / tongSo * 100,
                Name = "Thanh toán"
            },
            HoanThanh = new QuantityAndPercentResp
            {
                Quantity = hoanThanh,
                Percent = tongSo == 0 ? 0 : (double)hoanThanh / tongSo * 100,
                Name = "Hoàn thành"
            },
            Huy = new QuantityAndPercentResp
            {
                Quantity = huy,
                Percent = tongSo == 0 ? 0 : (double)huy / tongSo * 100,
                Name = "Huỷ"
            },
            KhongXacNhan = new QuantityAndPercentResp
            {
                Quantity = khongXacNhan,
                Percent = tongSo == 0 ? 0 : (double)huy / tongSo * 100,
                Name = "Không xác nhận"
            },
        };
    }

    public async Task<string> GetTongTien(DateTime? ngayBatDau, DateTime? ngayKetThuc)
    {
        var purchaseOrderList = await _purchaseOrderService.GetAllFromDate(ngayBatDau, ngayKetThuc);
        
        var tongTien = purchaseOrderList.Sum(x => x.TotalAmount);
        
        return tongTien.ToString() ?? ("0");
    }

    public async Task<List<RevenueEntryResp>> GetChiTieuTuan()
    {
        var currentDate = DateTime.Now;
        var ngayBatDauTuan = GetStartOfWeek(currentDate);
        var ngayKetThucTuan = ngayBatDauTuan.AddDays(6);
    
        var purchaseOrderList = await _purchaseOrderService.GetAllFromDate(ngayBatDauTuan, ngayKetThucTuan);
    
        var revenueLast7Days = new List<RevenueEntryResp>();

        var ngayHienTai = ngayKetThucTuan;
        for (int i = 0; i < 7; i++)
        {
            var tongTien = purchaseOrderList
                .Where(x => x.CreateAt.HasValue && x.CreateAt.Value.Date == ngayHienTai.Date)
                .Sum(x => x.TotalAmount);

            revenueLast7Days.Insert(0, new RevenueEntryResp
            {
                Ngay = ngayHienTai.Date,
                TongTien = tongTien
            });

            ngayHienTai = ngayHienTai.AddDays(-1);
        }

        // Sắp xếp lại theo thứ tự tăng dần của ngày
        revenueLast7Days = revenueLast7Days.OrderBy(entry => entry.Ngay).ToList();

        return revenueLast7Days;
    }
    
    private static DateTime GetStartOfWeek(DateTime dateTime)
    {
        var diff = (7 + (dateTime.DayOfWeek - DayOfWeek.Monday)) % 7;
        return dateTime.AddDays(-1 * diff).Date;
    }

    public async Task<List<RevenueEntryResp>> GetChiTieuThang()
    {
        var currentDate = DateTime.Now;
        var ngayBatDauThang = new DateTime(currentDate.Year, currentDate.Month, 1);
        var ngayKetThucThang = ngayBatDauThang.AddMonths(1).AddDays(-1);
    
        var revenueEntries = new List<RevenueEntryResp>();

        var currentWeekStart = ngayBatDauThang;
        while (currentWeekStart <= ngayKetThucThang)
        {
            var currentWeekEnd = currentWeekStart.AddDays(6);

            var tongTien = await GetTotalAmountOfWeek(currentWeekStart, currentWeekEnd);

            revenueEntries.Add(new RevenueEntryResp
            {
                Ngay = currentWeekStart,
                TongTien = tongTien
            });

            currentWeekStart = currentWeekStart.AddDays(7);
        }

        return revenueEntries;
    }
    
    private async Task<decimal> GetTotalAmountOfWeek(DateTime startOfWeek, DateTime endOfWeek)
    {
        var purchaseOrderList = await _purchaseOrderService.GetAllFromDate(startOfWeek, endOfWeek);
        return purchaseOrderList.Sum(x => x.TotalAmount);
    }
    
    public async Task<List<RevenueEntryResp>> GetChiTieuNam()
    {
        var currentDate = DateTime.Now;
        var ngayBatDauNam = new DateTime(currentDate.Year, 1, 1);
        var ngayKetThucNam = new DateTime(currentDate.Year, 12, 31);
    
        var revenueEntries = new List<RevenueEntryResp>();

        var currentMonthStart = ngayBatDauNam;
        while (currentMonthStart <= ngayKetThucNam)
        {
            var currentMonthEnd = currentMonthStart.AddMonths(1).AddDays(-1);

            var tongTien = await GetTotalAmountOfMonth(currentMonthStart, currentMonthEnd);

            revenueEntries.Add(new RevenueEntryResp
            {
                Ngay = currentMonthStart,
                TongTien = tongTien
            });

            currentMonthStart = currentMonthStart.AddMonths(1);
        }

        return revenueEntries;
    }
    
    private async Task<decimal> GetTotalAmountOfMonth(DateTime startOfMonth, DateTime endOfMonth)
    {
        var purchaseOrderList = await _purchaseOrderService.GetAllFromDate(startOfMonth, endOfMonth);
        return purchaseOrderList.Sum(x => x.TotalAmount);
    }
}