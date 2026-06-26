using ApplicationCore.Models;
using ApplicationCore.Models.Identity;
using ApplicationCore.Views;
using ApplicationCore.Views.Identity;
using Infrastructure.Helpers;
using System.Text.Json;

namespace ApplicationCore.Helpers;
public static class HighlightsHelpers
{
   public static List<HighlightViewModel> ParseHighlights(this string? json)
   {
      if (string.IsNullOrWhiteSpace(json)) return new List<HighlightViewModel>();
      try
      {
         // 支援兩種格式：單純字串陣列 或 物件陣列
         var highlights = new List<HighlightViewModel>();

         // 先試試是不是字串陣列
         var stringArray = JsonSerializer.Deserialize<List<string>>(json);
         if (stringArray != null)
         {
            highlights.AddRange(stringArray.Select(s => new HighlightViewModel { Text = s }));
            return highlights;
         }
      }
      catch
      {
         // 不是字串陣列，試物件陣列
      }

      return JsonSerializer.Deserialize<List<HighlightViewModel>>(json)
             ?? new List<HighlightViewModel>();
   }
   
}
