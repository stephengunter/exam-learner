using Ardalis.Specification;
using ApplicationCore.Models;
using ApplicationCore.Models.Identity;

namespace ApplicationCore.Specifications;
public class RolesIdSpecification : Specification<Role>
{
   public RolesIdSpecification(ICollection<string> ids)
   {
      Query.Where(x => ids.Contains(x.Id));
   }
}