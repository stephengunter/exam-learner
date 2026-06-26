using ApplicationCore.Models;
using ApplicationCore.Models.Identity;
using ApplicationCore.Views;
using ApplicationCore.Views.Identity;
using Infrastructure.Helpers;

namespace ApplicationCore.Helpers;
public static class ArticleItemsHelpers
{
   public static ArticleItemViewModel MapViewModel(this ArticleItem entity)
   {
      var model = new ArticleItemViewModel();
      entity.SetValuesTo(model);
      return model;
   }
   public static List<ArticleItemViewModel> MapViewModelList(this IEnumerable<ArticleItem> entities)
      => entities.Select(item => MapViewModel(item)).ToList();
   public static IEnumerable<ArticleItem> GetOrdered(this IEnumerable<ArticleItem> items)
     => items.OrderBy(item => item.Order);
}
