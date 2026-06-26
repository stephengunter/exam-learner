using ApplicationCore.Models;
using Ardalis.Specification;
using Infrastructure.Helpers;

namespace ApplicationCore.Specifications;
public class ArticleItemSpecification : Specification<ArticleItem>
{
   public ArticleItemSpecification(Article article)
   {
      Query.Where(item => !item.Removed && item.ArticleId == article.Id);
   }
   public ArticleItemSpecification(int id)
   {
      Query.Where(item => !item.Removed && item.Id == id);
   }

}