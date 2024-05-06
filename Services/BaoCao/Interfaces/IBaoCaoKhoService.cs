using ClosedXML.Excel;
using ptdn_net.Data.Dto.BaoCao;

namespace ptdn_net.Services.BaoCao.Interfaces;

public interface IBaoCaoKhoService
{
    Task<List<BaoCaoKhoResp>> GetAll();
    
    XLWorkbook Export(List<BaoCaoKhoResp> baoCaoList);
}