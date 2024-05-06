using System.Data.Common;
using SuCoYKhoa.Data.Response.Dashboard;

namespace ptdn_net.Data.Dto.BaoCao;

public class ThongTinGiaoHangResp
{
    public QuantityAndPercentResp XuLy { get; set; }

    public QuantityAndPercentResp XacNhan { get; set; }

    public QuantityAndPercentResp DangVanChuyen { get; set; }

    public QuantityAndPercentResp ThanhToan { get; set; }

    public QuantityAndPercentResp HoanThanh { get; set; }

    public QuantityAndPercentResp Huy { get; set; }

    public QuantityAndPercentResp KhongXacNhan { get; set; }
    
    public QuantityAndPercentResp DaHoan { get; set; }

    public ThongTinGiaoHangResp()
    {
    }
}