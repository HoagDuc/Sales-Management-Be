namespace ptdn_net.Const.Emun;

public enum Status : short
{
    // dang giao dich
    XuLy = 1, // nhân viên bán hàng tạo đơn hàng
    XacNhan = 2, // nhân viên kho xác nhận đơn hàng
    DangVanChuyen = 3, // nhân viên kho xác nhận chuyển hàng
    ThanhToan = 4, // khách hàng(nha cung cap) thanh toán => nhân viên tài chính xác nhận thanh toán
    
    // Hoan thanh
    HoanThanh = 5, // trang thai don hang hoan thanh
    Huy = 6,
    KhongXacNhan = 7,
    HoanHang = 8,
    GiaoThatBai = 9,
}