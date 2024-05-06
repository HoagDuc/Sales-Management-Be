using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Data.Dto.Transaction;
using ptdn_net.Services.interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.Transaction;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class TransactionTypeController : ControllerBase
{
    private readonly ITransactionTypeService _transactionTypeService;

    public TransactionTypeController(ITransactionTypeService transactionTypeService)
    {
        _transactionTypeService = transactionTypeService;
    }

    [HttpGet]
    [HasPermission(TransactiontionTypePermission.View)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _transactionTypeService.GetAll());
    }
    
    [HttpGet("{id:long}")]
    [HasPermission(TransactiontionTypePermission.View)]
    public async Task<IActionResult> GetById(long id)
    {
        return Ok(await _transactionTypeService.GetById(id));
    }

    [HttpPost]
    [HasPermission(TransactiontionTypePermission.Create)]
    public async Task<ActionResult<long>> CreateBy([FromBody] TransactionTypeReq req)
    {
        var result = await _transactionTypeService.CreateBy(req);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [HasPermission(TransactiontionTypePermission.Update)]
    public async Task<ActionResult<long>> UpdateBy(long id, [FromBody] TransactionTypeReq req)
    {
        var result = await _transactionTypeService.UpdateBy(req, id);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [HasPermission(TransactiontionTypePermission.Delete)]
    public async Task<ActionResult<string>> DeleteById(long id)
    {
        await _transactionTypeService.DeleteBy(id);
        return Ok();
    }
}