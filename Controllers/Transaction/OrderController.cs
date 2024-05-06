using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Data.Dto.Transaction;
using ptdn_net.Services.interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.Transaction;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [HasPermission(OrderPermission.View)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _orderService.GetAll());
    }

    [HttpGet("get-list-debt-by-customer-id")]
    [HasPermission(OrderPermission.View)]
    public async Task<IActionResult> GetListDebtByCustomerId([FromQuery] Guid customerId)
    {
        return Ok(await _orderService.GetByCustomerId(customerId));
    }
    
    [HttpGet("get-order-done")]
    [HasPermission(OrderPermission.View)]
    public async Task<IActionResult> GetAllOrderDone()
    {
        return Ok(await _orderService.GetAllOrderDone());
    }
    
    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
    {
        var orders = await _orderService.GetAllFromDate(fromDate, toDate);
        var workbook = _orderService.Export(orders, fromDate, toDate);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var memoryStream = new MemoryStream();
        
        workbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        response.Content = new StreamContent(memoryStream);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")  {
            FileName = "DanhSachDonBan.xlsx"
        };
        var content = memoryStream.ToArray();

        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachDonBan.xlsx");
    }
    
    [HttpGet("dang-van-chuyen")]
    [HasPermission(OrderPermission.View)]
    public async Task<IActionResult> GetAllDangVanChuyen()
    {
        return Ok(await _orderService.GetAllDangVanChuyen());
    }

    [HttpGet("{id:Guid}")]
    [HasPermission(OrderPermission.View)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _orderService.GetById(id);
        return Ok(result);
    }

    [HttpPost]
    [HasPermission(OrderPermission.Create)]
    public async Task<ActionResult<Guid>> CreateBy([FromBody] OrderReq req)
    {
        var result = await _orderService.CreateBy(req);
        return Ok(result);
    } 
    
    [HttpPost("don-ban-tai-quay")]
    [HasPermission(OrderPermission.Create)]
    public async Task<ActionResult<Guid>> CreateDonBanTaiQuayBy([FromBody] OrderReq req)
    {
        var result = await _orderService.CreateDonBanTaiQuayBy(req);
        return Ok(result);
    }

    [HttpPut("{id:Guid}")]
    [HasPermission(OrderPermission.Update)]
    public async Task<ActionResult<Guid>> UpdateBy(Guid id, [FromBody] OrderReq req)
    {
        var result = await _orderService.UpdateBy(req, id);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("out-inventory/{id:Guid}")]
    [HasPermission(OrderPermission.Update)]
    public async Task<ActionResult<Guid>> OutInventory(Guid id, [FromBody] OrderReq req)
    {
        var result = await _orderService.OutInventory(req, id);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("update-status-giao-that-bai/{id:Guid}")]
    [HasPermission(OrderPermission.Update)]
    public async Task<ActionResult<Guid>> UpdateStatusGiaoThatBai(Guid id)
    {
        var result = await _orderService.UpdateStatusGiaoThatBai(id);
        return Ok(result);
    }

    [HttpDelete("{id:Guid}")]
    [HasPermission(OrderPermission.Delete)]
    public async Task<ActionResult<string>> DeleteById(Guid id)
    {
        await _orderService.DeleteBy(id);
        return Ok();
    }
}