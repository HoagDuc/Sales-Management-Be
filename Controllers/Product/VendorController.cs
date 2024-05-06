using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Services.interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.Product;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class VendorController : ControllerBase
{
    private readonly IVendorService _vendorService;

    public VendorController(IVendorService vendorService)
    {
        _vendorService = vendorService;
    }

    [HttpGet]
    [HasPermission(VendorPermission.View)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _vendorService.GetAll());
    }
    
    [HttpGet("{id:long}")]
    [HasPermission(VendorPermission.View)]
    public async Task<IActionResult> GetById(long id)
    {
        return Ok(await _vendorService.GetById(id));
    }

    [HttpPost]
    [HasPermission(VendorPermission.Create)]
    public async Task<ActionResult<long>> CreateBy([FromBody] VendorReq req)
    {
        var result = await _vendorService.CreateBy(req);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [HasPermission(VendorPermission.Update)]
    public async Task<ActionResult<long>> UpdateBy(long id, [FromBody] VendorReq req)
    {
        var result = await _vendorService.UpdateBy(req, id);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [HasPermission(VendorPermission.Delete)]
    public async Task<ActionResult<string>> DeleteById(long id)
    {
        await _vendorService.DeleteBy(id);
        return Ok();
    }

    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel()
    {
        var vendors = await _vendorService.GetAll();
        var workbook = _vendorService.Export(vendors);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var memoryStream = new MemoryStream();
        
        workbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        response.Content = new StreamContent(memoryStream);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") {
            FileName = "Employees.xlsx"
        };
        var content = memoryStream.ToArray();

        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NhaCungCap.xlsx");
    }
}