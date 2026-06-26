using ApplicationCore.Models;
using Infrastructure.Entities;

namespace ApplicationCore.Views;

public class ArticleItemViewModel : EntityBaseView
{
   public int ArticleId { get; set; }
   public int Importance { get; set; }
   public string Title { get; set; } = String.Empty;
   public string? Content { get; set; }
   public string? Summary { get; set; }

   public bool Removed { get; set; }
   public int Order { get; set; }
   public bool Active { get; set; }

}
