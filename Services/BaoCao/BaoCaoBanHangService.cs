using ptdn_net.Const.Emun;
using ptdn_net.Data.Dto.BaoCao;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.BaoCao.Interfaces;
using ptdn_net.Services.interfaces;
using SuCoYKhoa.Data.Response.Dashboard;

namespace ptdn_net.Services.BaoCao;

public class BaoCaoBanHangService : BaseService, IBaoCaoBanHangService
{
    private readonly IOrderService _orderService;
    private readonly IRefundOrderService _refundOrderService;
    private readonly ITransactionService _transactionService;

    public BaoCaoBanHangService(IUnitOfWork unitOfWork
        , IOrderService orderService
        , IRefundOrderService refundOrderService
        , ITransactionService transactionService) 
        : base(unitOfWork)
    {
        _orderService = orderService;
        _refundOrderService = refundOrderService;
        _transactionService = transactionService;
    }

    public async Task<ThongTinGiaoHangResp> GetThongTinGiaoHang(DateTime? ngayBatDau, DateTime? ngayKetThuc)
    {
        var orderList = await _orderService.GetAllFromDate(ngayBatDau, ngayKetThuc);
        var xuLy = orderList.Count(x => x.Status == (short)Status.XuLy);
        var xacNhan = orderList.Count(x => x.Status == (short)Status.XacNhan);
        var dangVanChuyen = orderList.Count(x => x.Status == (short)Status.DangVanChuyen);
        var thanhToan = orderList.Count(x => x.Status == (short)Status.ThanhToan);
        var hoanThanh = orderList.Count(x => x.Status == (short)Status.HoanThanh);
        var huy = orderList.Count(x => x.Status == (short)Status.Huy);
        var khongXacNhan = orderList.Count(x => x.Status == (short)Status.KhongXacNhan);
        var daHoan = orderList.Count(x => x.Status == (short)Status.HoanHang);
        var tongSo = orderList.Count();

        return new ThongTinGiaoHangResp
        {
            XuLy = new QuantityAndPercentResp
            {
                Quantity = xuLy,
                Percent = tongSo == 0 ? 0 : (double)xuLy / tongSo * 100,
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
                Percent = tongSo == 0 ? 0 : (double)khongXacNhan / tongSo * 100,
                Name = "Không xác nhận"
            },
            DaHoan = new QuantityAndPercentResp
            {
                Quantity = daHoan,
                Percent = tongSo == 0 ? 0 : (double)daHoan / tongSo * 100,
                Name = "Hoàn hàng"
            },
        };
    }

    public async Task<string> GetTongTien(DateTime? ngayBatDau, DateTime? ngayKetThuc)
    {
        var orderList = await _orderService.GetAllFromDateV2(ngayBatDau, ngayKetThuc);
        var refundOrderList = await _refundOrderService.GetAllFromDate(ngayBatDau, ngayKetThuc);
        
        var tongTien = orderList.Sum(x => x.TotalAmount) - refundOrderList.Sum(x => x.TotalAmount);
        
        return tongTien.ToString() ?? ("0");
    }

    public async Task<List<RevenueResp>> GetDoanhThuTuan()
    {
        var currentDate = DateTime.Now;
        var ngayBatDauTuan = GetStartOfWeek(currentDate);
        var ngayKetThucTuan = ngayBatDauTuan.AddDays(6);
    
        var transactionList = await _orderService.GetAllFromDate(ngayBatDauTuan, ngayKetThucTuan);

        var revenueLast7Days = new List<RevenueResp>();

        var ngayHienTai = ngayKetThucTuan;
        for (var i = 0; i < 7; i++)
        {
            var tongTienThu = transactionList
                .Where(x => x.CreateAt.HasValue 
                            && x.CreateAt.Value.Date == ngayHienTai.Date)
                .Sum(x => x.TotalAmount);
            var tongTienChi = transactionList
                .Where(x => x.CreateAt.HasValue 
                            && x.CreateAt.Value.Date == ngayHienTai.Date)
                .Sum(x => x.TotalNhap);

            revenueLast7Days.Insert(0, new RevenueResp
            {
                Ngay = ngayHienTai.Date,
                DoanhThu = tongTienThu,
                LoiNhuan = tongTienThu - tongTienChi
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

    public async Task<List<RevenueResp>> GetDoanhThuThang()
    {
        var currentDate = DateTime.Now;
        var ngayBatDauThang = new DateTime(currentDate.Year, currentDate.Month, 1);
        var ngayKetThucThang = ngayBatDauThang.AddMonths(1).AddDays(-1);
    
        var revenues = new List<RevenueResp>();

        var currentWeekStart = ngayBatDauThang;
        while (currentWeekStart <= ngayKetThucThang)
        {
            var currentWeekEnd = currentWeekStart.AddDays(6);

            var doanhThu = await GetTotalPriceOfWeek(currentWeekStart, currentWeekEnd);
            var soChi = await GetTotalNhapOfWeek(currentWeekStart, currentWeekEnd);

            revenues.Add(new RevenueResp
            {
                Ngay = currentWeekStart,
                DoanhThu = doanhThu,
                LoiNhuan = doanhThu - soChi
            });

            currentWeekStart = currentWeekStart.AddDays(7);
        }

        return revenues;
    }
    
    private async Task<decimal?> GetTotalPriceOfWeek(DateTime startOfWeek, DateTime endOfWeek)
    {
        var orderList = await _orderService.GetAllFromDate(startOfWeek, endOfWeek);
        return orderList
            .Where(x => x.Status == (short)Status.HoanThanh)
            .Sum(x => x.TotalAmount);
    }
    
    private async Task<decimal?> GetTotalNhapOfWeek(DateTime startOfWeek, DateTime endOfWeek)
    {
        var orderList = await _orderService.GetAllFromDate(startOfWeek, endOfWeek);
        return orderList
            .Where(x => x.Status == (short)Status.HoanThanh)
            .Sum(x => x.TotalNhap);
    }

    public async Task<List<RevenueResp>> GetDoanhThuNam()
    {
        var currentDate = DateTime.Now;
        var ngayBatDauNam = new DateTime(currentDate.Year, 1, 1);
        var ngayKetThucNam = new DateTime(currentDate.Year, 12, 31);
    
        var revenueEntries = new List<RevenueResp>();

        var currentMonthStart = ngayBatDauNam;
        while (currentMonthStart <= ngayKetThucNam)
        {
            var currentMonthEnd = currentMonthStart.AddMonths(1).AddDays(-1);

            var doanhThu = await GetTotalPriceOfMonth(currentMonthStart, currentMonthEnd);
            var soChi = await GetTotalNhapOfMonth(currentMonthStart, currentMonthEnd);
            var loiNhuan = doanhThu - soChi;

            revenueEntries.Add(new RevenueResp
            {
                Ngay = currentMonthStart,
                DoanhThu = doanhThu,
                LoiNhuan = loiNhuan
            });

            currentMonthStart = currentMonthStart.AddMonths(1);
        }

        return revenueEntries;
    }
    
    private async Task<decimal?> GetTotalPriceOfMonth(DateTime startOfMonth, DateTime endOfMonth)
    {
        var orderList = await _orderService.GetAllFromDate(startOfMonth, endOfMonth);
        return orderList
            .Where(x => x.Status == (short)Status.HoanThanh)
            .Sum(x => x.TotalAmount);
    }
    
    private async Task<decimal?> GetTotalNhapOfMonth(DateTime startOfMonth, DateTime endOfMonth)
    {
        var orderList = await _orderService.GetAllFromDate(startOfMonth, endOfMonth);
        return orderList
            .Where(x => x.Status == (short)Status.HoanThanh)
            .Sum(x => x.TotalNhap);
    }

    public async Task<List<RevenueV2Resp>> GetDoanhThuTuanV2()
    {
        var currentDate = DateTime.Now;
        var ngayBatDauTuan = GetStartOfWeek(currentDate);
        var ngayKetThucTuan = ngayBatDauTuan.AddDays(6);
    
        var transactionList = await _transactionService.GetAllFromDate(ngayBatDauTuan, ngayKetThucTuan);

        var revenueLast7Days = new List<RevenueV2Resp>();

        var ngayHienTai = ngayKetThucTuan;
        for (var i = 0; i < 7; i++)
        {
            var tongTienThu = transactionList
                .Where(x => x.Date.HasValue 
                            && x.Date.Value.Date == ngayHienTai.Date
                            && x.TransactionTypeId == (short)TransactionType.PhieuThu)
                .Sum(x => x.Price);
            var tongTienChi = transactionList
                .Where(x => x.Date.HasValue 
                            && x.Date.Value.Date == ngayHienTai.Date
                            && x.TransactionTypeId == (short)TransactionType.PhieuChi)
                .Sum(x => x.Price);

            revenueLast7Days.Insert(0, new RevenueV2Resp
            {
                Ngay = ngayHienTai.Date,
                DoanhThu = tongTienThu,
                SoTienChiTieu = tongTienChi,
                LoiNhuan = tongTienThu - tongTienChi
            });

            ngayHienTai = ngayHienTai.AddDays(-1);
        }

        // Sắp xếp lại theo thứ tự tăng dần của ngày
        revenueLast7Days = revenueLast7Days.OrderBy(entry => entry.Ngay).ToList();

        return revenueLast7Days;
    }

    public async Task<List<RevenueV2Resp>> GetDoanhThuThangV2()
    {
        var currentDate = DateTime.Now;
        var ngayBatDauThang = new DateTime(currentDate.Year, currentDate.Month, 1);
        var ngayKetThucThang = ngayBatDauThang.AddMonths(1).AddDays(-1);
    
        var revenues = new List<RevenueV2Resp>();

        var currentWeekStart = ngayBatDauThang;
        while (currentWeekStart <= ngayKetThucThang)
        {
            var currentWeekEnd = currentWeekStart.AddDays(6);

            var doanhThu = await GetTotalPriceOfWeek(currentWeekStart, currentWeekEnd);
            var soChi = await GetTotalPriceOfWeek(currentWeekStart, currentWeekEnd);

            revenues.Add(new RevenueV2Resp
            {
                Ngay = currentWeekStart,
                DoanhThu = doanhThu,
                SoTienChiTieu = soChi,
                LoiNhuan = doanhThu - soChi
            });

            currentWeekStart = currentWeekStart.AddDays(7);
        }

        return revenues;
    }

    public async Task<List<RevenueV2Resp>> GetDoanhThuNamV2()
    {
        var currentDate = DateTime.Now;
        var ngayBatDauNam = new DateTime(currentDate.Year, 1, 1);
        var ngayKetThucNam = new DateTime(currentDate.Year, 12, 31);
    
        var revenueEntries = new List<RevenueV2Resp>();

        var currentMonthStart = ngayBatDauNam;
        while (currentMonthStart <= ngayKetThucNam)
        {
            var currentMonthEnd = currentMonthStart.AddMonths(1).AddDays(-1);

            var doanhThu = await GetTotalPriceOfMonth(currentMonthStart, currentMonthEnd);
            var soChi = await GetTotalPriceOfMonth(currentMonthStart, currentMonthEnd);
            var loiNhuan = doanhThu - soChi;

            revenueEntries.Add(new RevenueV2Resp
            {
                Ngay = currentMonthStart,
                DoanhThu = doanhThu,
                SoTienChiTieu = soChi,
                LoiNhuan = loiNhuan
            });

            currentMonthStart = currentMonthStart.AddMonths(1);
        }

        return revenueEntries;
    }
}