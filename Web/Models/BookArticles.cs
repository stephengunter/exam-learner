using ApplicationCore.Consts;
using ApplicationCore.Models;
using ApplicationCore.Views;
using Infrastructure.Views;

namespace Web.Models;

public class BookArticleLabels
{
   public string Book => "書名";
   public string Chapter => "章節";
   public string Type => "類型";
   public string Number => "編號";
   public string Importance => "重要性";
   public string Status => "狀態";
   public string Title => "標題";
   public string Content => "內容";
   public string Summary => "摘要";
   public string Order => "順序";
   public string Ps => "備註";
}
public class BookArticlesIndexModel
{
   public BookArticlesIndexModel(BookArticlesFetchRequest request)
   {
      Request = request;
      Types = new List<string>() { ArticleTypes.Law };
   }
   public BookArticleLabels Labels => new BookArticleLabels(); 
   public BookArticlesFetchRequest Request { get; set; }

   public Book? Book { get; set; }
   public Book? Chapter { get; set; }
   public ICollection<Book> Books { get; set; } = new List<Book>();
   public ICollection<Book> Chapters { get; set; } = new List<Book>();
   public ICollection<string> Types { get; set; } = new List<string>();
}

public class BookArticlesFetchRequest
{
   public BookArticlesFetchRequest(int? book, int? chapter)
   {
      Book = book;
      Chapter = chapter;
   }
   public int? Book { get; set; }
   public int? Chapter { get; set; }
}
public abstract class BookArticleBaseForm
{
   public int Id { get; set; }
   public int BookId { get; set; }
   public int ChapterId { get; set; }
   public int Number { get; set; }
   public int Importance { get; set; }
   public string Type { get; set; } = String.Empty;
   public string? Summary { get; set; }
   public string Title { get; set; } = String.Empty;
   public string Content { get; set; } = String.Empty;
   public string Ps { get; set; } = string.Empty;
   public bool Active { get; set; } = true;
   public int Order { get; set; }

}
public class BookArticleAddForm : BookArticleBaseForm
{
   
}
public class BookArticleEditForm : BookArticleBaseForm
{
   public bool CanRemove { get; set; }
}
public class BookArticleAddRequest
{
   public BookArticleAddRequest(BookArticleAddForm form, ICollection<ArticleItemAddForm> items)
   {
      Form = form;
      Items = items;
   }
   public BookArticleAddForm Form { get; set; }
   public ICollection<ArticleItemAddForm> Items { get; set; }
}

public class BookArticleEditRequest
{
   public BookArticleEditRequest(BookArticleEditForm form, ICollection<ArticleItemEditForm> items)
   {
      Form = form;
      Items = items;
   }
   public BookArticleEditForm Form { get; set; }
   public ICollection<ArticleItemEditForm> Items { get; set; }
}


public abstract class ArticleItemBaseForm
{
   public int Id { get; set; }
   public int ArticleId { get; set; }
   public int Importance { get; set; }
   public string Title { get; set; } = String.Empty;
   public string? Content { get; set; }
   public string? Summary { get; set; }
   public bool Active { get; set; } = true;
   public bool Removed { get; set; }
   public int Order { get; set; }

}
public class ArticleItemAddForm : ArticleItemBaseForm
{

}
public class ArticleItemEditForm : ArticleItemBaseForm
{
   public bool CanRemove { get; set; }
}


public abstract class ArticleItemHighlightBaseForm
{
   public int ItemId { get; set; }
   public string Text { get; set; } = string.Empty;
}
public class ArticleItemHighlightAddForm : ArticleItemHighlightBaseForm
{

}
public class ArticleItemHighlightEditForm : ArticleItemHighlightBaseForm
{
   public bool CanRemove { get; set; }
}
