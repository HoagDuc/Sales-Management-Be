using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using ptdn_net.Common.Attribute;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Services.interfaces;
using systemIO = System.IO;
using static ptdn_net.Const.Permission;

namespace ptdn_net.Controllers.Product;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [HasPermission(ProductPermission.View)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _productService.GetAll());
    }

    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel()
    {
        var products = await _productService.GetAll();
        var workbook = _productService.Export(products);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var memoryStream = new MemoryStream();
        
        workbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        response.Content = new StreamContent(memoryStream);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") {
            FileName = "DanhSachSanPham.xlsx"
        };
        var content = memoryStream.ToArray();

        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachSanPham.xlsx");
    }

    [HttpGet("{id:long}")]
    [HasPermission(ProductPermission.View)]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _productService.GetById(id);
        return Ok(result);
    }

    [HttpPost]
    [HasPermission(ProductPermission.Create)]
    public async Task<ActionResult<long>> CreateBy([FromForm] ProductReq req)
    {
        var result = await _productService.CreateBy(req, req.Images);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [HasPermission(ProductPermission.Update)]
    public async Task<ActionResult<long>> UpdateBy(long id, [FromForm] ProductReq req)
    {
        var result = await _productService.UpdateBy(req, id);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [HasPermission(ProductPermission.Delete)]
    public async Task<ActionResult<string>> DeleteById(long id)
    {
        await _productService.DeleteBy(id);
        return Ok();
    }
    
    [HttpGet("DownloadExcelTemplate")]
    public IActionResult DownloadExcelTemplate()
    {
        const string templateFilePath = "wwwroot/template/TemplateThemSanPham.xlsx";
        if (!systemIO.File.Exists(templateFilePath))
        {
            return NotFound("File template không tồn tại.");
        }

        var fileBytes = systemIO.File.ReadAllBytes(templateFilePath);
        var fileStream = new MemoryStream(fileBytes);

        return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "TemplateThemSanPham.xlsx");
    }
    
    [HttpPost("ImportExcel")]
    public async Task<IActionResult> ImportExcel( IFormFile file)
    {
        await _productService.ImportExcel(file);
        var a = file;
        return Ok();
    }
}