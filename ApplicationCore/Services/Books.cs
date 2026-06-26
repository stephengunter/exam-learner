using ApplicationCore.DataAccess;
using ApplicationCore.Models;
using ApplicationCore.Specifications;
using Infrastructure.Helpers;

namespace ApplicationCore.Services;

public interface IBookService
{
   Task<IEnumerable<Book>> FetchAsync(ICollection<string>? includes = null);
   Task<Book?> GetByIdAsync(int id, ICollection<string>? includes = null);
   Task<Book> CreateAsync(Book entity, string userId);
   Task UpdateAsync(Book entity, string userId);
   Task RemoveAsync(Book entity, string userId);
}

public class BookService : IBookService
{
	private readonly IDefaultRepository<Book> _repository;

	public BookService(IDefaultRepository<Book> repository)
	{
      _repository = repository;
	}
   public async Task<IEnumerable<Book>> FetchAsync(ICollection<string>? includes = null)
       => await _repository.ListAsync(new BookSpecification(includes));


   public async Task<Book?> GetByIdAsync(int id, ICollection<string>? includes = null)
      => await _repository.FirstOrDefaultAsync(new BookSpecification(id, includes));


   public async Task<Book> CreateAsync(Book entity, string userId)
   {
      entity.SetCreated(userId);
      return await _repository.AddAsync(entity);
   }

   public async Task UpdateAsync(Book entity, string userId)
   {
      entity.SetUpdated(userId);
      await _repository.UpdateAsync(entity);
   }

   public async Task RemoveAsync(Book entity, string userId)
   {
      entity.Removed = true;
      entity.SetUpdated(userId);
      await _repository.UpdateAsync(entity);
   }

}
