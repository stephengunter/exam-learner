using ApplicationCore.Models;
using ApplicationCore.Views;
using Infrastructure.Paging;
using Infrastructure.Helpers;

namespace ApplicationCore.Helpers;
public static class BooksHelpers
{
   public static IEnumerable<Book> GetOrdered(this IEnumerable<Book> books)
     => books.OrderBy(item => item.Order).OrderByDescending(item => item.CreatedAt);
}
