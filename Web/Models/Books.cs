using ApplicationCore.Models;
using ApplicationCore.Views;
using Infrastructure.Views;

namespace Web.Models;

public class BookLabels
{
   public string Status => "ª¬ºA";
   public string Title => "¼ÐÃD";
   public string Ps => "³Æµù";
}
public class BooksIndexModel
{
   public BooksIndexModel(BooksFetchRequest request)
   {
      Request = request;
   }
   public BookLabels Labels => new BookLabels(); 
   public BooksFetchRequest Request { get; set; }
}

public class BooksFetchRequest
{
   public BooksFetchRequest(bool active)
   {
      Active = active;
   }
   public bool Active { get; set; }
}
public abstract class BookBaseForm
{
   public int Id { get; set; }
   public string Title { get; set; } = String.Empty;
   public string Ps { get; set; } = string.Empty;
   public bool Active { get; set; }
   public int Order { get; set; }

   

}
public class BookAddForm : BookBaseForm
{
   
}
public class BookEditForm : BookBaseForm
{
   public bool CanRemove { get; set; }
}
public class BookAddRequest
{
   public BookAddRequest(BookAddForm form)
   {
      Form = form;
   }
   public BookAddForm Form { get; set; }
}
public class BookEditRequest
{
   public BookEditRequest(BookEditForm form)
   {
      Form = form;
   }
   public BookEditForm Form { get; set; }
}