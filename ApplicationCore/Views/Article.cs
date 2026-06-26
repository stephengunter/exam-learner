using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Views;

namespace ApplicationCore.Views;

public class ArticleViewModel : EntityBaseView
{
   public int BookId { get; set; }
   public int ChapterId { get; set; }
   public string Type { get; set; } = String.Empty;
   public int Number { get; set; }
   public int Importance { get; set; }
   public string Title { get; set; } = String.Empty;
   public string? Content { get; set; }
   public string? Summary { get; set; }

   public bool Removed { get; set; }
   public int Order { get; set; }
   public bool Active { get; set; }

   public string LawNumber => $"第{Number}條";
   
   public List<ArticleItemViewModel> Items { get; set; } = new List<ArticleItemViewModel>();

}
public class HighlightViewModel
{
   public string Text { get; set; } = null!;
   public string? Section { get; set; }   // 選填：章節/段落 ID
   public int Start { get; set; }         // 選填：起始偏移
   public int End { get; set; }           // 選填：結束偏移
   public string Color { get; set; } = "yellow";
}