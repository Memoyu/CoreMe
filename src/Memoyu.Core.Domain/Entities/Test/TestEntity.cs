/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Infrastructure.Entities.Test
*   文件名称 ：TestEntity.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 8:20:20
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql.DataAnnotations;
using Memoyu.Core.Domain.Base;
using Memoyu.Core.Domain.Shared.Const;
using System;

namespace Memoyu.Core.Domain.Entities.Test
{
    /// <summary>
    /// 实体案例
    /// </summary>
    [Table(Name = SystemConst.DbTablePrefix + "_test")]
    [Index("index_test_on_age", "Age", true)]//索引
    public class TestEntity : FullAduitEntity<Guid>
    {
        [Column(StringLength = 10)]
        public string Name { get; set; }
        public int Sex { get; set; }
        public int Age { get; set; }
        [Column(StringLength = 100)]
        public string Address { get; set; }
    }
}
