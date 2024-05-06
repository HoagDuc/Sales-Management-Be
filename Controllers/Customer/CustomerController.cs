using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Data.Dto.Customer;
using ptdn_net.Services.interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.Customer;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    [HasPermission(CustomerPermission.View)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _customerService.GetAll());
    }
    
    [HttpPut("update-no/{id:Guid}")]
    [HasPermission(CustomerPermission.Update)]
    public async Task<ActionResult<Guid>> UpdateNoBy(Guid id)
    {
        var result = await _customerService.UpdateNoBy(id);
        return Ok(result);
    }
    
    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel()
    {
        var customers = await _customerService.GetAll();
        var workbook = _customerService.Export(customers);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var memoryStream = new MemoryStream();
        
        workbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        response.Content = new StreamContent(memoryStream);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") {
            FileName = "DanhSachKhachHang.xlsx"
        };
        var content = memoryStream.ToArray();

        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachKhachHang.xlsx");
    }

    [HttpGet("{id:Guid}")]
    [HasPermission(CustomerPermission.View)]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _customerService.GetById(id));
    }

    [HttpPost]
    [HasPermission(CustomerPermission.Create)]
    public async Task<ActionResult<Guid>> CreateBy([FromBody] CustomerReq req)
    {
        var result = await _customerService.CreateBy(req);
        return Ok(result);
    }

    [HttpPut("{id:Guid}")]
    [HasPermission(CustomerPermission.Update)]
    public async Task<ActionResult<Guid>> UpdateBy(Guid id, [FromBody] CustomerReq req)
    {
        var result = await _customerService.UpdateBy(req, id);
        return Ok(result);
    }

    [HttpDelete("{id:Guid}")]
    [HasPermission(CustomerPermission.Delete)]
    public async Task<ActionResult<string>> DeleteById(Guid id)
    {
        await _customerService.DeleteBy(id);
        return Ok();
    }
}