namespace ptdn_net.Data.Dto.BaoCao;

public class BaoCaoKhoResp
{
    public long Id { get; set; }
    
    public string? MaSanPham { get; set; }
    
    public Entity.ProductEntity.Product Product { get; set; }
    
    public string? TenSanPham { get; set; }
    
    public string? DonViTinh { get; set; }
    
    public string? BarCode { get; set; }
    
    public long? TonKho { get; set; }
    
    public decimal? GiaNhap { get; set; }
    
    public decimal? GiaVon { get; set; }
    
    public decimal? GiaBanLe { get; set; }

    public decimal? GiaSi { get; set; }

    public decimal? TongGiaHang { get; set; }
    
    public string? TrangThai { get; set; }
}