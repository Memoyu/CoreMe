﻿/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Sr.Manager.Application.Contracts.Mapper
*   文件名称 ：AutoMapperConfig.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 11:24:55
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AutoMapper;
using Sr.Manager.Application.Contracts.Mapper.Test;

namespace Sr.Manager.Application.Contracts.Mapper
{
    /// <summary>
    /// 静态全局 AutoMapper 配置文件
    /// </summary>
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                new TestMapper();
            });
        }
    }

}
