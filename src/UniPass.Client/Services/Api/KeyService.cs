﻿using UniPass.Infrastructure.Models;

namespace UniPass.Client.Services.Api;

public class KeyService : BaseCrudRequests<Key, Guid>
{
    public KeyService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }
}