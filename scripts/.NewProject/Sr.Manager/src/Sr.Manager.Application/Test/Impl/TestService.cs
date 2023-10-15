/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Application.Test.Impl
*   文件名称 ：TestService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 9:10:23
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Sr.Manager.Application.Base.Impl;
using Sr.Manager.Application.Contracts.Test;
using Sr.Manager.Domain.Entities;
using Sr.Manager.Domain.Shared.Base;
using Sr.Manager.Domain.Shared.Exceptions;
using Sr.Manager.ToolKits.Base.Page;
using System;
using System.Threading.Tasks;

namespace Memoyu.Application.Test.Impl
{
    public class TestService : ApplicationService, ITestService
    {
        private readonly IAuditBaseRepository<TestEntity> _testRepository;

        public TestService(IAuditBaseRepository<TestEntity> testRepository)
        {
            _testRepository = testRepository;
        }



        public async Task CreateAsync(ModifyTestDto inputDto)
        {
            bool exist = _testRepository.Select.Any(r => r.Name == inputDto.Name);
            if (exist)
            {
                throw new KnownException("信息已存在");
            }

            TestEntity test = Mapper.Map<TestEntity>(inputDto);
            await _testRepository.InsertAsync(test);
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TestDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedDto<TestDto>> GetListAsync(PagingDto pageDto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, ModifyTestDto inputDto)
        {
            throw new NotImplementedException();
        }
    }
}
