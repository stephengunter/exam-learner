using ApplicationCore.Models;
using ApplicationCore.Models.Identity;
using ApplicationCore.Views;
using ApplicationCore.Views.Identity;
using Infrastructure.Helpers;

namespace ApplicationCore.Helpers;
public static class ProfilesHelpers
{
   public static ProfilesViewModel MapViewModel(this Profiles profiles)
   {
      var model = new ProfilesViewModel();
      profiles.SetValuesTo(model);
      return model;
   }

   
   public static Profiles MapEntity(this ProfilesViewModel model, string currentUserId, Profiles? entity = null)
   {
      if (entity == null) entity = new Profiles();
      model.SetValuesTo(entity);

      if (String.IsNullOrEmpty(model.UserId)) entity.SetCreated(currentUserId);
      else entity.SetUpdated(currentUserId);
      return entity;
   }

   public static List<ProfilesViewModel> MapViewModelList(this IEnumerable<Profiles> profiles)
      => profiles.Select(item => MapViewModel(item)).ToList();
}
