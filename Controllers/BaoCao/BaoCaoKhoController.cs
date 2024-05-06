using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Services.BaoCao.Interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.BaoCao;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class BaoCaoKhoController : ControllerBase
{
    private readonly IBaoCaoKhoService _baoCaoKhoService;
    
    public BaoCaoKhoController(IBaoCaoKhoService inventoryService)
    {
        _baoCaoKhoService = inventoryService;
    }
    
    [HttpGet]
    [HasPermission(BaoCaoPermission.View)]
    public async Task<IActionResult> GetAllBaoCao()
    {
        return Ok(await _baoCaoKhoService.GetAll());
    }
    
    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel()
    {
        var baoCaoList = await _baoCaoKhoService.GetAll();
        var workbook = _baoCaoKhoService.Export(baoCaoList);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var memoryStream = new MemoryStream();
        
        workbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        response.Content = new StreamContent(memoryStream);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") {
            FileName = "BaoCaoKho.xlsx"
        };
        var content = memoryStream.ToArray();

        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BaoCaoKho.xlsx");
    }
}