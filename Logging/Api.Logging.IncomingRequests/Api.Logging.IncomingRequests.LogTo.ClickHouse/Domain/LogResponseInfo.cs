﻿namespace Api.Logging.IncomingRequests.LogTo.ClickHouse.Domain;

public sealed record LogResponseInfo
{
    public int StatusCode { get; set; }
    public string ContentType { get; set; }
    public string Headers { get; set; }
    public string Body { get; set; }
}