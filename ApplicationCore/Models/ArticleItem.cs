using ApplicationCore.Consts;
using Infrastructure.Entities;
using Infrastructure.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models;
public class ArticleItem : EntityBase, IBaseRecord, IRemovable, ISortable
{
   public int ArticleId { get; set; }

   [Required]
   public virtual Article? Article { get; set; }
   public int Importance { get; set; }
   public string Title { get; set; } = String.Empty;
   public string? Content { get; set; }
   public string? Summary { get; set; }
   public string? Highlights { get; set; }

   public bool Removed { get; set; }
   public int Order { get; set; }
   public bool Active => ISortableHelpers.IsActive(this);

   public DateTime CreatedAt { get; set; } = DateTime.Now;
   public string CreatedBy { get; set; } = string.Empty;
   public DateTime? LastUpdated { get; set; }
   public string? UpdatedBy { get; set; }
}

