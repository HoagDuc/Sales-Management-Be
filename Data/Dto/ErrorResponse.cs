﻿using System.Text.Json;

namespace ptdn_net.Data.Dto;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}