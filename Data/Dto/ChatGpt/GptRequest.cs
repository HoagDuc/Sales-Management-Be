namespace ptdn_net.Data.Dto.ChatGpt;

public class GptRequest
{
    public string model { get; set; } = "gpt-3.5-turbo";
    public GptMessage[] messages { get; set; }
    public double temperature { get; set; } = 0.7;
}