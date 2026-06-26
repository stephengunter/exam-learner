using ApplicationCore.Consts;
using ApplicationCore.Models;
using ApplicationCore.Models.Auth;
using ApplicationCore.Models.Identity;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Auth;
public class OAuthSpecification : Specification<OAuth>
{
   public OAuthSpecification(User user, OAuthProvider provider)
   {
      Query.Where(item => item.UserId == user.Id && item.Provider == provider);
   }
}
