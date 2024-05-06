using ClosedXML.Excel;
using ptdn_net.Data.Dto.BaoCao;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.BaoCao.Interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services.BaoCao;

public class BaoCaoKhoService : BaseService, IBaoCaoKhoService
{
    private readonly IInventoryService _inventoryService;

    public BaoCaoKhoService(IUnitOfWork unitOfWork
        , IInventoryService inventoryService) 
        : base(unitOfWork)
    {
        _inventoryService = inventoryService;
    }

    public async Task<List<BaoCaoKhoResp>> GetAll()
    {
        return await _inventoryService.ThongKeKho();
    }

    public XLWorkbook Export(List<BaoCaoKhoResp> baoCaoList)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Báo cáo kho");
        worksheet.Columns().AdjustToContents();

        var titleRow = worksheet.Range("A1:I1");
        titleRow.Merge();
        worksheet.Cell(1, 1).Value = "Báo cáo kho";
        titleRow.Style.Font.Bold = true;
        titleRow.Style.Font.FontColor = XLColor.Black;
        titleRow.Style.Font.FontSize = 30;

        var headerRow = worksheet.Range("A3:I3");
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRow.Style.Font.FontColor = XLColor.Black;

        worksheet.Cell(3, 1).Value = "STT";
        worksheet.Cell(3, 2).Value = "Tên sản phẩm";
        worksheet.Cell(3, 3).Value = "Đơn vị tính";
        worksheet.Cell(3, 4).Value = "Mã sản phẩm";
        worksheet.Cell(3, 5).Value = "Giá nhập";
        worksheet.Cell(3, 6).Value = "Tồn kho";
        worksheet.Cell(3, 7).Value = "Giá bán lẻ";
        worksheet.Cell(3, 8).Value = "Giá vốn";
        worksheet.Cell(3, 9).Value = "Tổng giá hàng";
        
        for (var col = 1; col <= 9; col++)
        {
            worksheet.Cell(3, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(3, col).Style.Border.OutsideBorderColor = XLColor.Black;
        }

        var stt = 1;
        var row = 5;
        foreach (var item in baoCaoList)
        {
            worksheet.Cell(row, 1).Value = stt++;
            worksheet.Cell(row, 2).Value = item.TenSanPham;
            worksheet.Cell(row, 3).Value = item.DonViTinh;
            worksheet.Cell(row, 4).Value = item.MaSanPham;
            worksheet.Cell(row, 5).Value = item.GiaNhap;
            worksheet.Cell(row, 6).Value = item.TonKho;
            worksheet.Cell(row, 7).Value = item.GiaBanLe;
            worksheet.Cell(row, 8).Value = item.GiaVon;
            worksheet.Cell(row, 9).Value = item.TongGiaHang;

            for (var col = 1; col <= 9; col++)
            {
                worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(row, col).Style.Border.OutsideBorderColor = XLColor.Black;
            }

            row++;
        }

        var giaNhaSum = baoCaoList.Sum(x => x.GiaNhap);
        var tonKhoSum = baoCaoList.Sum(x => x.TonKho);
        var giaBanLeSum = baoCaoList.Sum(x => x.GiaBanLe);
        var giaVonSum = baoCaoList.Sum(x => x.GiaVon);
        var tongGiaHang = baoCaoList.Sum(x => x.TongGiaHang);
        var mergedRange = worksheet.Range("A4:B4").Merge();
        mergedRange.Value = $"Tổng có {stt - 1} sản phẩm";
        worksheet.Cell(4, 3).Value = "";
        worksheet.Cell(4, 4).Value = "";
        worksheet.Cell(4, 5).Value = giaNhaSum;
        worksheet.Cell(4, 6).Value = tonKhoSum;
        worksheet.Cell(4, 7).Value = giaBanLeSum;
        worksheet.Cell(4, 8).Value = giaVonSum;
        worksheet.Cell(4, 9).Value = tongGiaHang;
        for (var col = 1; col <= 9; col++)
        {
            worksheet.Cell(4, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(4, col).Style.Border.OutsideBorderColor = XLColor.Black;
        }
        
        worksheet.Columns().AdjustToContents();
        return workbook;
    }
}