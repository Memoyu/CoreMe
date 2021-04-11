using CoreMe.Core.Domains.Common.Base;
using CoreMe.Core.Domains.Entities.Core;
using System.Collections.Generic;

namespace CoreMe.Service.Core.Permission.Input
{
    public class RoleDto: EntityDto
    {
        public List<PermissionEntity> Permissions { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public bool IsStatic { get; set; }
        public int Sort { get; set; }
    }
}
