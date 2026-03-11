using CoreMe.Application.Common.Security;

namespace CoreMe.Application.Common.Services.App;

/// <summary>
/// Demo Service
/// 在Handler响应中，业务逻辑中出现重合，可以使用Service类进行整合
/// 在Service实现共用逻辑，由批量注册到容器中（确保实现类贴上AppService特性），最后在Handler注入Service，调用公共方法
/// </summary>
/// <param name="currentUserProvider"></param>
[AppService]
internal class DemoService(ICurrentUserProvider currentUserProvider) : IDemoService
{
    public long GetUserId() => currentUserProvider.UserId;

}
