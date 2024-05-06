using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Const.Emun;
using ptdn_net.Data.Dto.Transaction;
using ptdn_net.Services.interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.Transaction;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    [HasPermission(TransactionPermission.View)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _transactionService.GetAll());
    }
    
    [HttpGet("{id:guid}")]
    [HasPermission(TransactionPermission.View)]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _transactionService.GetById(id));
    }

    [HttpPost]
    [HasPermission(TransactionPermission.Create)]
    public async Task<ActionResult<Guid>> CreateBy([FromBody] TransactionReq req)
    {
        var result = await _transactionService.CreateBy(req);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [HasPermission(TransactionPermission.Update)]
    public async Task<ActionResult<Guid>> UpdateBy(Guid id, [FromBody] TransactionReq req)
    {
        var result = await _transactionService.UpdateBy(req, id);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(TransactionPermission.Delete)]
    public async Task<ActionResult<string>> DeleteById(Guid id)
    {
        await _transactionService.DeleteBy(id);
        return Ok();
    }
    
    [HttpGet("ExportExcel-phieu-thu")]
    public async Task<IActionResult> ExportExcelPhieuThu([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
    {
        var transactionList = await _transactionService.GetAllFromDate(fromDate, toDate);
        transactionList = transactionList.Where(x => x.TransactionTypeId == (long)TransactionType.PhieuThu).ToList();
        var workbook = _transactionService.Export(transactionList, fromDate, toDate);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var memoryStream = new MemoryStream();
        
        workbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        response.Content = new StreamContent(memoryStream);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")  {
            FileName = "DanhSachPhieuChi.xlsx"
        };
        var content = memoryStream.ToArray();

        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachPhieuChi.xlsx");
    }
    
    [HttpGet("ExportExcel-phieu-chi")]
    public async Task<IActionResult> ExportExcelPhieuChi([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
    {
        var transactionList = await _transactionService.GetAllFromDate(fromDate, toDate);
        transactionList = transactionList.Where(x => x.TransactionTypeId == (long)TransactionType.PhieuChi).ToList();
        var workbook = _transactionService.Export(transactionList, fromDate, toDate);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var memoryStream = new MemoryStream();
        
        workbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        response.Content = new StreamContent(memoryStream);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")  {
            FileName = "DanhSachPhieuThu.xlsx"
        };
        var content = memoryStream.ToArray();

        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachPhieuThu.xlsx");
    }
    
    [HttpGet("ExportExcel-all")]
    public async Task<IActionResult> ExportExcelAll([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
    {
        var transactionList = await _transactionService.GetAllFromDate(fromDate, toDate);
        var workbook = _transactionService.Export(transactionList, fromDate, toDate);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var memoryStream = new MemoryStream();
        
        workbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        response.Content = new StreamContent(memoryStream);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")  {
            FileName = "DanhSachPhieuThu.xlsx"
        };
        var content = memoryStream.ToArray();

        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachPhieuThu.xlsx");
    }
}