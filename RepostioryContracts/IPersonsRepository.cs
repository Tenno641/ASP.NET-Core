using Entities;
using System.Linq.Expressions;

namespace RepositoryContracts;

public interface IPersonsRepository
{
    Task<Person> AddPersonAsync(Person person);
    Task<Person?> GetAsync(Guid id);
    IQueryable<Person> All();
    Task<Person> UpdateAsync(Person person);
    IQueryable<Person> FilterAsync(Expression<Func<Person, bool>> predicate);
    Task<bool> RemoveAsync(Guid id);
}
