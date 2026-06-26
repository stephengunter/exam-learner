using ApplicationCore.Models;
using ApplicationCore.Models.Identity;
using ApplicationCore.Views;
using ApplicationCore.Views.Identity;
using Infrastructure.Helpers;

namespace ApplicationCore.Helpers;
public static class BookArticlesHelpers
{
   public static ArticleViewModel MapViewModel(this Article article)
   {
      var model = new ArticleViewModel();
      article.SetValuesTo(model);

      if (article.Items.HasItems()) model.Items = article.Items.MapViewModelList();
      return model;
   }
   public static List<ArticleViewModel> MapViewModelList(this IEnumerable<Article> articles)
      => articles.Select(item => MapViewModel(item)).ToList();
   public static IEnumerable<Article> GetOrdered(this IEnumerable<Article> articles)
     => articles.OrderBy(item => item.Order);

}
