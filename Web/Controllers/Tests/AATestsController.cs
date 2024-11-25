using Microsoft.AspNetCore.Mvc;
using ApplicationCore.DataAccess;
using Microsoft.SqlServer.Dac;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;

namespace Web.Controllers.Tests;

public class AATestsController : BaseTestController
{
   private readonly UserManager<User> _userManager;
   private readonly DefaultContext _defaultContext;
   public AATestsController(DefaultContext defaultContext, UserManager<User> userManager)
   {
      
      _defaultContext = defaultContext;
      _userManager = userManager;
   }

   [HttpGet]
   public async Task<ActionResult> Index()
   {
      
      return Ok();
   }

   void ExportDatabaseToBacpac(string connectionString, string bacpacFilePath)
   {
      try
      {
         // Create an instance of DacServices with the connection string
         DacServices dacServices = new DacServices(connectionString);

         // Subscribe to the Message event to receive status messages
         dacServices.Message += (sender, e) => Console.WriteLine(e.Message);

         Console.WriteLine("Starting export...");

         // Perform the export
         dacServices.ExportBacpac(bacpacFilePath, "hlh_api");

         Console.WriteLine($"Export completed. Bacpac file saved to: {bacpacFilePath}");
      }
      catch (Exception ex)
      {
         Console.WriteLine($"An error occurred: {ex.Message}");
      }
   }
}