using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ptdn_net.Data.Dto.ChatGpt;
using ptdn_net.Services.BaoCao.Interfaces;

namespace ptdn_net.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class ChatGptController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IDashboardService _dashboardService;

    public ChatGptController(IHttpClientFactory httpClientFactory, IDashboardService dashboardService)
    {
        _httpClientFactory = httpClientFactory;
        _dashboardService = dashboardService;
    }

    public static string GetHuongDanString()
    {
        return
            "Quy trình Nhập Hàng của hàng ptdn Vật Liệu Xây Dựng :\n\nB1: Tạo đơn hàng .\n\nB2: tiếp nhận yêu cầu .\n\nB3: vận chuyển.\n \nB4: Thanh toán.\n\nB5 : Lưu trữ thông tin .\n\nB6: phản hồi yêu cầu .\n\n\n\n . Quy trình xuất hàng của  hàng ptdn : \n\nB1: Đặt hàng .\n\nB2: Tiếp nhận thông tin đặt hàng .\n\nB3: Nhân viên kiểm tra xác thực thông tin đặt hàng .\n\nB4: Lập đơn hàng . \n\n\nB5: Lập hóa đơn bán hàng .\n\nB6: Đóng gói gửi hàng .\n\nB7: Tiếp nhận đơn hàng .\n\nB8: Nhận hàng .\n\nB9: Thanh Toán .\n\nB10: Kiểm tra thanh toán .\n\nB11: cập nhập trạng thái đơn hàng \n";
    } 
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] GptMessage[] content)
    {
        const string apiKey = "AIzaSyAgrYXkNvifZ54oZPWxHzA4w2qRgaWCp2Y";
        const string apiUrl = "https://free-gpt-api.vercel.app/v1/chat/completions";

        var httpClient = _httpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var tkngay = await _dashboardService.GetKetQuaNgay();
        for (var index = 0; index < content.Length; index++)
        {
            if (index == content.Length - 1)
            {
                content[index].content = GetHuongDanString() + tkngay!.ToString() + "." + content[index].content;
            }
        }

        var body = new GptRequest
        {
            messages = content
        };

        var json = JsonSerializer.Serialize(body);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await httpClient.PostAsync(apiUrl, httpContent);

        if (!response.IsSuccessStatusCode)
        {
            // Xử lý khi yêu cầu không thành công
            return BadRequest("Failed to fetch response from GPT API.");
        }

        var result = await response.Content.ReadAsStringAsync();
        return Ok(result);
    }
}
