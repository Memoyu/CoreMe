﻿/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.Application.Contracts.Dtos.Test
*   文件名称 ：TestDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 8:36:48
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Core.Domain.Base;

namespace Memoyu.Core.Application.Contracts.Dtos.Test
{
    public class TestDto : EntityDto
    {
        public string Name { get; set; }
        public int Sex { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
    }
}
