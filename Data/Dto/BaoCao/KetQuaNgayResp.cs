namespace ptdn_net.Data.Dto.BaoCao;

public class KetQuaNgayResp
{
    public decimal DoanhThu { get; set; }
    
    public int DonHangMoi { get; set; }
    
    public int DonTraHang { get; set; }
    
    public int DonHangHuy { get; set; }

    public override string ToString()
    {
        return "Đây là dữ liệu báo cáo ngày hôm nay : doanh thu là " + DoanhThu  + "đ , Đơn hàng mới " + DonHangMoi + " đơn, Đơn hàng đã trả " + DonTraHang + " đơn, Đơn hàng đã huỷ " + DonHangHuy + "đơn";
    }
}