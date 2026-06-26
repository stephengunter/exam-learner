using ApplicationCore.DataAccess;
using ApplicationCore.Models;
using ApplicationCore.Specifications;
using Infrastructure.Helpers;

namespace ApplicationCore.Services;

public interface IArticleItemService
{
   Task<IEnumerable<ArticleItem>> FetchAsync(Article article);
   Task<ArticleItem?> GetByIdAsync(int id);
   Task<ArticleItem> CreateAsync(ArticleItem entity, string userId);
   Task UpdateAsync(ArticleItem entity, string userId);
   Task RemoveAsync(ArticleItem entity, string userId);
}

public class BookArticleItemService : IArticleItemService
{
	private readonly IDefaultRepository<ArticleItem> _repository;

	public BookArticleItemService(IDefaultRepository<ArticleItem> repository)
	{
      _repository = repository;
	}
   public async Task<IEnumerable<ArticleItem>> FetchAsync(Article article)
       => await _repository.ListAsync(new ArticleItemSpecification(article));

   public async Task<ArticleItem?> GetByIdAsync(int id)
      => await _repository.FirstOrDefaultAsync(new ArticleItemSpecification(id));


   public async Task<ArticleItem> CreateAsync(ArticleItem entity, string userId)
   {
      entity.SetCreated(userId);
      return await _repository.AddAsync(entity);
   }

   public async Task UpdateAsync(ArticleItem entity, string userId)
   {
      entity.SetUpdated(userId);
      await _repository.UpdateAsync(entity);
   }

   public async Task RemoveAsync(ArticleItem entity, string userId)
   {
      entity.Removed = true;
      entity.SetUpdated(userId);
      await _repository.UpdateAsync(entity);
   }

}
