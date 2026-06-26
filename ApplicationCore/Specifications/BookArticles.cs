using ApplicationCore.Models;
using Ardalis.Specification;
using Infrastructure.Helpers;

namespace ApplicationCore.Specifications;
public class BookArticleSpecification : Specification<Article>
{
   public BookArticleSpecification(Book book, ICollection<string>? includes = null)
   {
      if (includes!.HasItems())
      {
         foreach (var item in includes!) Query.Include(item);
      }
      Query.Where(item => !item.Removed && item.BookId == book.Id);
   }
   public BookArticleSpecification(Book book, Book chapter, ICollection<string>? includes = null)
   {
      if (includes!.HasItems())
      {
         foreach (var item in includes!) Query.Include(item);
      }
      Query.Where(item => !item.Removed && item.BookId == book.Id && item.ChapterId == chapter.Id);
   }
   public BookArticleSpecification(int id, ICollection<string>? includes = null)
   {
      if (includes!.HasItems())
      {
         foreach (var item in includes!) Query.Include(item);
      }
      Query.Where(item => !item.Removed && item.Id == id);
   }

}