using ApplicationCore.Views;
using ApplicationCore.Models;
using Infrastructure.Helpers;
using ApplicationCore.Models.Identity;
using ApplicationCore.Views.Identity;

namespace ApplicationCore.Helpers;

public static class UsersHelpers
{
   public static string GetUserName(this User user)
      => String.IsNullOrEmpty(user.UserName) ? String.Empty : user.UserName;


   public static IEnumerable<User> GetOrdered(this IEnumerable<User> users)
      => users.OrderByDescending(u => u.CreatedAt);

   public static IEnumerable<User> FilterByKeyword(this IEnumerable<User> users, ICollection<string> keywords)
   {
      var byUsername = users.FilterByUsername(keywords);
      var byName = users.FilterByName(keywords);
      return byUsername.Union(byName, new UserEqualityComparer()).ToList();
   }

   public static IEnumerable<User> FilterByUsername(this IEnumerable<User> users, ICollection<string> keywords)
      => users.Where(user => keywords.Any(user.GetUserName().CaseInsensitiveContains)).ToList();

   public static IEnumerable<User> FilterByName(this IEnumerable<User> users, ICollection<string> keywords)
      => users.Where(user => user.Profiles != null && keywords.Any(user.Profiles.Name.CaseInsensitiveContains)).ToList();


   #region Views
   public static UserViewModel MapViewModel(this User user)
   {
      var model = new UserViewModel();
      user.SetValuesTo(model);
      //if (user.Profiles != null) model.Profiles = user.Profiles.MapViewModel(mapper, department);
      //if (user.Roles.HasItems()) model.Roles = user.Roles.Select(x => x.MapViewModel(mapper)).ToList();
      return model;
   }
   public static User MapEntity(this UserViewModel model, string currentUserId, User? entity = null)
   {
      
      if (entity == null) entity = new User();
      model.SetValuesTo(entity);
      entity.LastUpdated = DateTime.Now;

      return entity;
   }
   

   public static List<UserViewModel> MapViewModelList(this IEnumerable<User> users)
      => users.Select(item => MapViewModel(item)).ToList();

   
   #endregion

}

public class UserEqualityComparer : IEqualityComparer<User>
{
   public bool Equals(User? a, User? b) => a!.Id == b!.Id;

   public int GetHashCode(User obj) => obj.Id.GetHashCode() ^ obj.UserName!.GetHashCode();
}
