// using Microsoft.AspNetCore.Mvc;
// using ptdn_net.Data.Dto.Product;
// using ptdn_net.Services.interfaces;
//
// namespace ptdn_net.Controllers;
//
// [Route("api/v{version:apiVersion}/[controller]")]
// [ApiVersion("1.0")]
// [ApiController]
// public class ImageController : ControllerBase
// {
//     private readonly IImageService _imageService;
//
//     public ImageController(IImageService imageService)
//     {
//         _imageService = imageService;
//     }
//     
//     [HttpGet]
//     [Route("GetByProductId")]
//     public async Task<IActionResult> GetByProductId(long id)
//     {
//         return Ok(await _imageService.GetByProductId(id));
//     }
//     
//     [HttpPost]
//     public async Task<ActionResult<uint>> CreateBy([FromBody] ImageReq req)
//     {
//         var result = await _imageService.CreateBy(req);
//         return Ok(result);
//     }
//     
//     [HttpPut("{id:long}")]
//     public async Task<ActionResult<long>> UpdateBy(long id, [FromBody] ImageReq req)
//     {
//         var result = await _imageService.UpdateBy(req, id);
//         return Ok(result);
//     }
//     
//     [HttpDelete("{id:long}")]
//     public async Task<ActionResult<string>> DeleteById(long id)
//     {
//         await _imageService.DeleteBy(id);
//         return Ok();
//     }
// }