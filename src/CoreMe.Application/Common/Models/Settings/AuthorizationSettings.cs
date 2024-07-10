﻿namespace CoreMe.Application.Common.Models.Settings;

public class AuthorizationSettings
{
    public JwtOptions Jwt { get; set; } = new();

    public QiniuOptions Qiniu { get; set; } = new();

    public GitHubOptions GitHub { get; set; } = new();
}
