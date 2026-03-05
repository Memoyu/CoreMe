using CoreMe.Application.Common.Models.Settings;
using CoreMe.Application.Common.Utils.QiniuUtil;
using CoreMe.Application.Common.Utils.QiniuUtil.QiniuUtil;
using CoreMe.Application.FileStorages.Common;
using Microsoft.Extensions.Options;

namespace CoreMe.Application.FileStorages.Queries.Generate;

public class GenerateQiniuUploadTokenQueryHandler(IOptionsMonitor<AuthorizationSettings> authOptions) : IRequestHandler<GenerateQiniuUploadTokenQuery, Result>
{
    public async Task<Result> Handle(GenerateQiniuUploadTokenQuery request, CancellationToken cancellationToken)
    {
        var options = authOptions.CurrentValue?.Qiniu ?? throw new Exception("未配置七牛云授权信息");
        var sign = new QiniuSignature(options.AK, options.SK);
        var policy = new QiniuPutPolicy
        {
            Scope = string.IsNullOrWhiteSpace(request.Path) ? options.Bucket : $"{options.Bucket}:{request.Path}"
        };

        var token = sign.SignWithData(policy.ToJsonString());

        return await Task.FromResult(Result.Success(new QiniuUploadTokenResult { Token = token, Host = options.Host }));
    } 
}
