﻿using CoreMe.Core.AOP.Filters;
using CoreMe.Core.Domains.Common.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoreMe.Core.Extensions.ServiceCollection
{
    /// <summary>
    /// 配置注册Swagger
    /// </summary>
    public static class SwaggerSetup
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                //遍历应用Swagger分组信息
                ApiInfo.ApiInfos.ForEach(a => opt.SwaggerDoc(a.UrlPrefix, a.OpenApiInfo));

                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "CoreMe.xml"));
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "CoreMe.Service.xml"));


                #region 小绿锁

                #region Bearer
                //添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference()
                            {
                                Id =  "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization", //jwt默认的参数名称
                    In = ParameterLocation.Header, //jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                #endregion

                #region Oauth2
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference()
                            {
                                Id =  "oauth2",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                // Define the OAuth2.0 scheme that's in use (i.e. Implicit Flow)
                opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("/connect/authorize", UriKind.Relative),
                            TokenUrl = new Uri("/connect/token", UriKind.Relative),
                            Scopes = new Dictionary<string, string>
                            {
                                { "CoreMe", "Access read/write CoreMe" }
                            }
                        },
                        Password = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri("/connect/authorize", UriKind.Relative),
                            TokenUrl = new Uri("/connect/token", UriKind.Relative),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "Access read openid" },
                                { "offline_access", "Access read offline_access" },
                                { "CoreMe", "Access read/write CoreMe" }
                            }
                        }
                    }
                });
                #endregion

                #endregion

                //opt.OperationFilter<AddResponseHeadersFilter>();
                //opt.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //opt.OperationFilter<SecurityRequirementsOperationFilter>();

                // 应用Controller的API文档描述信息
                opt.DocumentFilter<SwaggerDocumentFilter>();
            });
            return services;
        }
    }
}
