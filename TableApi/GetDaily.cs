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
using System.Collections.Generic;

namespace TableApi
{
    public static class GetDaily
    {
        [FunctionName("GetDaily")]
        public static async Task<IActionResult> GetAll(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "daily")] HttpRequest req,
            [Table("Daily", Connection = "AzureWebJobsStorage")] CloudTable tblDaily,
            [Table("Status", Connection = "AzureWebJobsStorage")] CloudTable tblStatus)
        {
            var operation = TableOperation.Retrieve<StatusAzureModel>("Status", "Update");
            var result = await tblStatus.ExecuteAsync(operation);
            var ret = result.Result as StatusAzureModel;
            if (ret != null)
            {
                string filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, ret.Data);
                TableQuery<DailyAzureModel> query = new TableQuery<DailyAzureModel>().Where(filter);
                var segment = await tblDaily.ExecuteQuerySegmentedAsync(query, null); //will return first 1K, so take 50 only
                var data = segment.Select(e => new DailyAzureModel
                {
                    PartitionKey = e.PartitionKey,
                    RowKey = e.RowKey,
                    Date = e.Date,
                    Key = e.Key,
                    Type = e.Type,
                    Name = e.Name,
                    Count = e.Count
                });

                return new OkObjectResult(data);
            }
            return new NotFoundResult();
        }
    }
}
