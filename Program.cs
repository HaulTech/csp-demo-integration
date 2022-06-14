using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaulTech.Public.CSP;
using HaulTech.Public.CSP.Models;

namespace CSP_Integration_Demo
{

  class Program
  {

    #region Demo variables
    // The base URLs for the CSP API and CSP Authorisation endpoints
    static readonly string _apiUrl = "";
    static readonly string _authorisationUrl = "";

    // There credentials are here simply as an example, DO NOT ever store credentials in
    // plain text in a production environment.
    static readonly string _username = "";
    static readonly string _password = "";
    #endregion

    static async Task Main(string[] args)
    {
      // Initialise a new CustomerServicePortalClient object used to communicate with the CSP
      CustomerServicePortalClient cspClient;
      try
      {
        cspClient = new CustomerServicePortalClient(_username, _password, _apiUrl, _authorisationUrl);
      }
      catch (Exception exception)
      {
        LogException(exception);
        return;
      }
      // Retreive a list of all the accounts we have permission to access
      var accounts = await cspClient.GetAccountsAsync();
      if (accounts == null)
      {
        Console.WriteLine("Unable to resolve any Accounts.");
        return;
      }
      // Find all available Service Matricies
      var matrices = await cspClient.GetServiceMatricesAsync();
      if (matrices == null)
      {
        Console.WriteLine("Unable to resolve the Service Matrices.");
        return;
      }
      // Prepare a new job object
      var collectionServiceMatrix = matrices.First(x => x.Type == CSPGoodsMovementType.Collection);
      var deliveryServiceMatrix = matrices.First(x => x.Type == CSPGoodsMovementType.Delivery);
      var collectionDate = DateTime.UtcNow;
      var job = new CspPortalJob()
      {
        AccountId = accounts.First().AccountId, // For the purpose of the demo, we'll resolve the first Account our user has permission to access
        ServiceLevelId = collectionServiceMatrix.ServiceLevelId,
        CollectionDateTime = collectionDate,
        DeliveryDateTime = await cspClient.GetNextValidDeliveryDateForServiceMatrixAsync(collectionDate, deliveryServiceMatrix.ServiceLevelId),
        CollectionStartTime = collectionServiceMatrix.StartTime,
        CollectionEndTime = collectionServiceMatrix.EndTime,
        DeliveryStartTime = deliveryServiceMatrix.StartTime,
        DeliveryEndTime = deliveryServiceMatrix.EndTime,
        CollectionAddress1 = "CSP NuGet Demo Collection",
        DeliveryAddress1 = "CSP NuGet Demo Delivery",
        Weight = 1,
        DeliveryServiceLevelTimeId = deliveryServiceMatrix.ServiceTimeId,
        CollectionServiceLevelTimeId = collectionServiceMatrix.ServiceTimeId
      };
      // Add a pending job
      string pendingJobId;
      try
      {
        pendingJobId = await cspClient.AddJobAsync(job);
        Console.WriteLine($"Pending job submitted with id '{pendingJobId}'");
      }
      catch (Exception exception)
      {
        LogException(exception);
        return;
      }
      // Commit the pending job to the haulier
      string confirmedJobId;
      try
      {
        confirmedJobId = await cspClient.ConfirmJobAsync(pendingJobId);
        Console.WriteLine($"Job confirmed with id '{confirmedJobId}'");
      }
      catch (Exception exception)
      {
        LogException(exception);
        return;
      }
      // Track the job  
      List<CspPortalJobExtended> search;
      try
      {
        search = await cspClient.SearchForJobsAsync(new CspSearch()
        {
          JobNo = confirmedJobId
        });
      }
      catch (Exception exception)
      {
        LogException(exception);
        return;
      }
      Console.WriteLine($"Search returned {search.Count} jobs.");
    }

    /// <summary>
    /// A method to normalise how to handle exceptions.
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    private static void LogException(Exception exception)
    {
      // Grab the current console colour
      var previousForeground = Console.ForegroundColor;
      // Update the console colour
      Console.ForegroundColor = ConsoleColor.Red;
      // Log the exception
      Console.WriteLine($"An exception occurred: {exception.Message}");
      if (exception.InnerException != null)
      {
        Console.WriteLine($"Inner Exception: {exception.InnerException.Message}");
      }
      // Reset the console colours to previous state
      Console.ForegroundColor = previousForeground;
    }

  }

}
