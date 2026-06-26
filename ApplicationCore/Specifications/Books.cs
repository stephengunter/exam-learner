using ApplicationCore.Models;
using Ardalis.Specification;
using Infrastructure.Helpers;

namespace ApplicationCore.Specifications;
public class BookSpecification : Specification<Book>
{
   public BookSpecification(ICollection<string>? includes = null)
   {
      if (includes!.HasItems())
      {
         foreach (var item in includes!) Query.Include(item);
      }
      Query.Where(item => !item.Removed && (item.ParentId == null || item.ParentId == 0));
   }
   public BookSpecification(int id, ICollection<string>? includes = null)
   {
      if (includes!.HasItems())
      {
         foreach (var item in includes!) Query.Include(item);
      }
      Query.Where(item => !item.Removed && item.Id == id);
   }
   
}