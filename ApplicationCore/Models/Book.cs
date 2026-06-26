using Infrastructure.Entities;
using Infrastructure.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models;

public class Book : EntityBase, IBaseCategory<Book>, IBaseRecord, IRemovable, ISortable
{
   public int Number { get; set; }
   public string Title { get; set; } = String.Empty;
   public string Content { get; set; } = String.Empty;
   public Book? Parent { get; set; }

   public int? ParentId { get; set; }
   public int? RootId { get; set; }

   public bool IsRootItem => ParentId is null;

   public ICollection<Book>? SubItems { get; set; }
   [NotMapped]
   public ICollection<int>? SubIds { get; set; }

   public bool Removed { get; set; }
   public int Order { get; set; }

   public bool Active => ISortableHelpers.IsActive(this);
   public DateTime CreatedAt { get; set; } = DateTime.Now;
   public string CreatedBy { get; set; } = string.Empty;
   public DateTime? LastUpdated { get; set; }
   public string? UpdatedBy { get; set; }

   public void LoadSubItems(IEnumerable<IBaseCategory<Book>> items) => BaseCategoriesHelpers.LoadSubItems(this, items);

}
