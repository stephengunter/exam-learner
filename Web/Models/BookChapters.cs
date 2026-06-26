using ApplicationCore.Models;
using ApplicationCore.Views;
using Infrastructure.Views;

namespace Web.Models;

public class BookChapterLabels
{
   public string Status => "狀態";
   public string Title => "標題";
   public string Content => "內容";
   public string Number => "編號";
   public string Ps => "備註";
}
public class BookChaptersIndexModel
{
   public BookChaptersIndexModel(BookChaptersFetchRequest request, Book book)
   {
      Request = request;
      Book = book;
   }
   public BookChapterLabels Labels => new BookChapterLabels(); 
   public BookChaptersFetchRequest Request { get; set; }

   public Book Book { get; set; }
}

public class BookChaptersFetchRequest
{
   public BookChaptersFetchRequest(int book)
   {
      Book = book;
   }
   public int Book { get; set; }
}
public abstract class BookChapterBaseForm
{
   public int Id { get; set; }
   public int? ParentId { get; set; }
   public int? RootId { get; set; }

   public int Number { get; set; }
   public string Title { get; set; } = String.Empty;
   public string Content { get; set; } = String.Empty;
   public string Ps { get; set; } = string.Empty;
   public bool Active { get; set; }
   public int Order { get; set; }

}
public class BookChapterAddForm : BookChapterBaseForm
{
   
}
public class BookChapterEditForm : BookChapterBaseForm
{
   public bool CanRemove { get; set; }
}
public class BookChapterAddRequest
{
   public BookChapterAddRequest(BookChapterAddForm form)
   {
      Form = form;
   }
   public BookChapterAddForm Form { get; set; }
}
public class BookChapterEditRequest
{
   public BookChapterEditRequest(BookChapterEditForm form)
   {
      Form = form;
   }
   public BookChapterEditForm Form { get; set; }
}