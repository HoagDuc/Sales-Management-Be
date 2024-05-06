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
public class PurchaseOrderController : ControllerBase
{
    private readonly IPurchaseOrderService _purchaseOrderService;

    public PurchaseOrderController(IPurchaseOrderService purchaseOrderService)
    {
        _purchaseOrderService = purchaseOrderService;
    }

    [HttpGet]
    [HasPermission(PurchaseOrderPermission.View)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _purchaseOrderService.GetAll());
    }
    
    [HttpGet("dang-van-chuyen")]
    [HasPermission(PurchaseOrderPermission.View)]
    public async Task<IActionResult> GetAllDangVanChuyen()
    {
        return Ok(await _purchaseOrderService.GetAllDangVanChuyen());
    }
    
    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel([FromQuery] DateTime fromDate, DateTime toDate)
    {
        var purchaseOrders = await _purchaseOrderService.GetAllFromDate(fromDate, toDate);
        var workbook = _purchaseOrderService.Export(purchaseOrders, fromDate, toDate);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var memoryStream = new MemoryStream();
        
        workbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        response.Content = new StreamContent(memoryStream);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") {
            FileName = "DanhSachDonNhap.xlsx"
        };
        var content = memoryStream.ToArray();

        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachDonNhap.xlsx");
    }

    [HttpGet("{id:Guid}")]
    [HasPermission(PurchaseOrderPermission.View)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _purchaseOrderService.GetById(id);
        return Ok(result);
    }

    [HttpPost]
    [HasPermission(PurchaseOrderPermission.Create)]
    public async Task<ActionResult<Guid>> CreateBy([FromBody] PurchaseOrderReq req)
    {
        var result = await _purchaseOrderService.CreateBy(req);
        return Ok(result);
    }

    [HttpPut("{id:Guid}")]
    [HasPermission(PurchaseOrderPermission.Update)]
    public async Task<ActionResult<Guid>> UpdateBy(Guid id, [FromBody] PurchaseOrderReq req)
    {
        var result = await _purchaseOrderService.UpdateBy(req, id);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("go-inventory/{id:Guid}")]
    [HasPermission(PurchaseOrderPermission.Update)]
    public async Task<ActionResult<Guid>> GoInventory(Guid id, [FromBody] PurchaseOrderReq req)
    {
        var result = await _purchaseOrderService.GoInventory(req, id);
        return Ok(result);
    }

    [HttpDelete("{id:Guid}")]
    [HasPermission(PurchaseOrderPermission.Delete)]
    public async Task<ActionResult<string>> DeleteById(Guid id)
    {
        await _purchaseOrderService.DeleteBy(id);
        return Ok();
    }
}