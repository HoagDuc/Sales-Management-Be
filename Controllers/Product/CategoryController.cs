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
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [HasPermission(CategoryPermission.View)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _categoryService.GetAll());
    }
    
    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel()
    {
        var categories = await _categoryService.GetAll();
        var workbook = _categoryService.Export(categories);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var memoryStream = new MemoryStream();
        
        workbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        response.Content = new StreamContent(memoryStream);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") {
            FileName = "DanhSachLoaiSanPham.xlsx"
        };
        var content = memoryStream.ToArray();

        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachLoaiSanPham.xlsx");
    }
    
    [HttpGet("{id:long}")]
    [HasPermission(CategoryPermission.View)]
    public async Task<IActionResult> GetById(long id)
    {
        return Ok(await _categoryService.GetById(id));
    }

    [HttpPost]
    [HasPermission(CategoryPermission.Create)]
    public async Task<ActionResult<long>> CreateBy([FromBody] CategoryReq req)
    {
        var result = await _categoryService.CreateBy(req);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [HasPermission(CategoryPermission.Update)]
    public async Task<ActionResult<long>> UpdateBy(long id, [FromBody] CategoryReq req)
    {
        var result = await _categoryService.UpdateBy(req, id);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [HasPermission(CategoryPermission.Delete)]
    public async Task<ActionResult<string>> DeleteById(long id)
    {
        await _categoryService.DeleteBy(id);
        return Ok();
    }
}