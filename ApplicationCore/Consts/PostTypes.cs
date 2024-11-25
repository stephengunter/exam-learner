using ApplicationCore.Models;
using Ardalis.Specification;

namespace ApplicationCore.Consts;

public class PostTypes
{
   public static string Article = new Article().GetType().Name;
}