using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using System.Linq;

namespace TableApi
{
    public static class GetLocations
    {
        [FunctionName("GetLocations")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "locations")] HttpRequest req,
            [Table("Location", Connection = "AzureWebJobsStorage")] CloudTable tblLocation,
            [Table("Status", Connection = "AzureWebJobsStorage")] CloudTable tblStatus)
        {
            var operation = TableOperation.Retrieve<StatusAzureModel>("Status", "Update");
            var result = await tblStatus.ExecuteAsync(operation);
            var ret = result.Result as StatusAzureModel;
            if (ret != null)
            {
                string filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, ret.Data);
                TableQuery<LocationAzureModel> query = new TableQuery<LocationAzureModel>().Where(filter);
                var segment = await tblLocation.ExecuteQuerySegmentedAsync(query, null); //will return first 1K, so take 50 only
                var data = segment.Select(e => new LocationAzureModel
                {
                    PartitionKey = e.PartitionKey,
                    RowKey = e.RowKey,
                    Date = e.Date,
                    Latitude = e.Latitude,
                    Longitude = e.Longitude,
                    Text = e.Text
                });

                return new OkObjectResult(data);
            }
            return new NotFoundResult();
        }
    }
}
