using ApplicationCore.Models;
using Ardalis.Specification;
using Infrastructure.Helpers;

namespace ApplicationCore.Specifications;
public class BookChapterSpecification : Specification<Book>
{
   public BookChapterSpecification(Book book, ICollection<string>? includes = null)
   {
      if (includes!.HasItems())
      {
         foreach (var item in includes!) Query.Include(item);
      }
      Query.Where(item => !item.Removed && item.RootId == book.Id);
   }
   public BookChapterSpecification(int id, ICollection<string>? includes = null)
   {
      if (includes!.HasItems())
      {
         foreach (var item in includes!) Query.Include(item);
      }
      Query.Where(item => !item.Removed && item.Id == id);
   }
   
}