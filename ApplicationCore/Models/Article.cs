using ApplicationCore.Consts;
using Infrastructure.Entities;
using Infrastructure.Helpers;

namespace ApplicationCore.Models;
public class Article : EntityBase, IBaseRecord, IRemovable, ISortable
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
   public bool Active => ISortableHelpers.IsActive(this);

   public DateTime CreatedAt { get; set; } = DateTime.Now;
   public string CreatedBy { get; set; } = string.Empty;
   public DateTime? LastUpdated { get; set; }
   public string? UpdatedBy { get; set; }


   public virtual ICollection<ArticleItem> Items { get; set; } = new List<ArticleItem>();
}

