using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Data.Dto.Customer;
using ptdn_net.Services.interfaces;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.Customer;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class CustomerGroupController : ControllerBase
{
    private readonly ICustomerGroupService _customerGroupService;

    public CustomerGroupController(ICustomerGroupService customerGroupService)
    {
        _customerGroupService = customerGroupService;
    }

    [HttpGet]
    [HasPermission(CustomerGroupPermission.View)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _customerGroupService.GetAll());
    }

    [HttpGet("{id:long}")]
    [HasPermission(CustomerGroupPermission.View)]
    public async Task<IActionResult> GetById(long id)
    {
        return Ok(await _customerGroupService.GetById(id));
    }

    [HttpPost]
    [HasPermission(CustomerGroupPermission.Create)]
    public async Task<ActionResult<long>> CreateBy([FromBody] CustomerGroupReq req)
    {
        var result = await _customerGroupService.CreateBy(req);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [HasPermission(CustomerGroupPermission.Update)]
    public async Task<ActionResult<long>> UpdateBy(long id, [FromBody] CustomerGroupReq req)
    {
        var result = await _customerGroupService.UpdateBy(req, id);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [HasPermission(CustomerGroupPermission.Delete)]
    public async Task<ActionResult<string>> DeleteById(long id)
    {
        await _customerGroupService.DeleteBy(id);
        return Ok();
    }
}