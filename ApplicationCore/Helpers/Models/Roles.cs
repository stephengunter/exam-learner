using ApplicationCore.Views;
using ApplicationCore.Models;
using Infrastructure.Helpers;
using Infrastructure.Paging;
using ApplicationCore.Views.Identity;
using ApplicationCore.Models.Identity;

namespace ApplicationCore.Helpers;

public static class RolesHelpers
{
   public static RoleViewModel MapViewModel(this Role role)
   {
      var model = new RoleViewModel();
      role.SetValuesTo(model);
      return model;
   }

   public static List<RoleViewModel> MapViewModelList(this IEnumerable<Role> roles)
      => roles.Select(item => MapViewModel(item)).ToList();
}