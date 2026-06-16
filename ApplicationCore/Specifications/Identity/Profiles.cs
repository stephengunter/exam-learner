using Ardalis.Specification;
using ApplicationCore.Models.Identity;

namespace ApplicationCore.Specifications.Identity;
public class ProfilesSpecification : Specification<Profiles>
{
   public ProfilesSpecification(User user)
   {
      Query.Where(p => p.UserId == user.Id);
   }
}

