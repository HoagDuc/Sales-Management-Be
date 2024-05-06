using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Data.Dto.Transaction;
using ptdn_net.Services.interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.Transaction;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class RefundOrderController : ControllerBase
{
    private readonly IRefundOrderService _refundOrderService;

    public RefundOrderController(IRefundOrderService refundOrderService)
    {
        _refundOrderService = refundOrderService;
    }

    [HttpGet]
    [HasPermission(RefundOrderPermission.View)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _refundOrderService.GetAll());
    }
    
    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel([FromQuery] DateTime fromDate, DateTime toDate)
    {
        var refundOrderService = await _refundOrderService.GetAllFromDate(fromDate, toDate);
        var workbook = _refundOrderService.Export(refundOrderService, fromDate, toDate);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var memoryStream = new MemoryStream();
        
        workbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        response.Content = new StreamContent(memoryStream);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") {
            FileName = "DanhSachDonHoan.xlsx"
        };
        var content = memoryStream.ToArray();

        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachDonHoan.xlsx");
    }
    
    [HttpGet("dang-van-chuyen")]
    [HasPermission(RefundOrderPermission.View)]
    public async Task<IActionResult> GetAllDangVanChuyen()
    {
        return Ok(await _refundOrderService.GetAllDangVanChuyen());
    }

    [HttpGet("{id:Guid}")]
    [HasPermission(RefundOrderPermission.View)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _refundOrderService.GetById(id);
        return Ok(result);
    }

    [HttpPost]
    [HasPermission(RefundOrderPermission.Create)]
    public async Task<ActionResult<Guid>> CreateBy([FromBody] RefundOrderReq req)
    {
        var result = await _refundOrderService.CreateBy(req);
        return Ok(result);
    }

    [HttpPut("{id:Guid}")]
    [HasPermission(RefundOrderPermission.Update)]
    public async Task<ActionResult<Guid>> UpdateBy(Guid id, [FromBody] RefundOrderReq req)
    {
        var result = await _refundOrderService.UpdateBy(req, id);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("go-inventory/{id:Guid}")]
    [HasPermission(RefundOrderPermission.Update)]
    public async Task<ActionResult<Guid>> GoInventory(Guid id, [FromBody] RefundOrderReq req)
    {
        var result = await _refundOrderService.GoInventory(req, id);
        return Ok(result);
    }

    [HttpDelete("{id:Guid}")]
    [HasPermission(RefundOrderPermission.Delete)]
    public async Task<ActionResult<string>> DeleteById(Guid id)
    {
        await _refundOrderService.DeleteBy(id);
        return Ok();
    }
}