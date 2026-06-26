using ApplicationCore.DataAccess;
using ApplicationCore.Models;
using ApplicationCore.Specifications;
using Infrastructure.Helpers;

namespace ApplicationCore.Services;

public interface IBookArticleService
{
   Task<IEnumerable<Article>> FetchAsync(Book chapter, ICollection<string>? includes = null);
   Task<IEnumerable<Article>> FetchByBookAsync(int bookId, ICollection<string>? includes = null);
   Task<Article?> GetByIdAsync(int id, ICollection<string>? includes = null);
   Task<Article> CreateAsync(Article entity, string userId);
   Task UpdateAsync(Article entity, string userId);
   Task RemoveAsync(Article entity, string userId);
}

public class BookArticleService : IBookArticleService
{
	private readonly IDefaultRepository<Article> _repository;

	public BookArticleService(IDefaultRepository<Article> repository)
	{
      _repository = repository;
	}
   public async Task<IEnumerable<Article>> FetchAsync(Book chapter, ICollection<string>? includes = null)
       => await _repository.ListAsync(new BookArticleSpecification(new Book { Id = chapter.RootId.ToInt() },  chapter, includes));
   public async Task<IEnumerable<Article>> FetchByBookAsync(int bookId, ICollection<string>? includes = null)
       => await _repository.ListAsync(new BookArticleSpecification(new Book { Id = bookId }, includes));  


   public async Task<Article?> GetByIdAsync(int id, ICollection<string>? includes = null)
      => await _repository.FirstOrDefaultAsync(new BookArticleSpecification(id, includes));


   public async Task<Article> CreateAsync(Article entity, string userId)
   {
      entity.SetCreated(userId);
      if (entity.Items.HasItems())
      {
         foreach (var item in entity.Items)
         {
            item.SetCreated(userId);
         }
      }
      return await _repository.AddAsync(entity);
   }

   public async Task UpdateAsync(Article entity, string userId)
   {
      entity.SetUpdated(userId);
      await _repository.UpdateAsync(entity);
   }

   public async Task RemoveAsync(Article entity, string userId)
   {
      entity.Removed = true;
      entity.SetUpdated(userId);
      await _repository.UpdateAsync(entity);
   }

}
