using Domain.Common;
using Domain.Common.People;
using Domain.Entities;
using Domain.Entities.Dtos;

namespace Application.Interfaces;

public interface IPeopleService : IBaseCrudService<Person>
{
    Task<BaseResponse<object>> CreateAsync(CreatePersonRequest request);
    Task<BaseResponse<IEnumerable<PersonDto>>> GetDtoListAsync(int pageIndex, int pageSize);
    Task<BaseResponse<PersonDto>> GetDtoByIdAsync(Guid id);
}