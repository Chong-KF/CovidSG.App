using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;

namespace CovidSG.App.Models
{
    public class DailyAzureModel : TableEntity
    {
        virtual public string Date { get; set; }
        virtual public string Key { get; set; }
        virtual public string Type { get; set; }
        virtual public string Name { get; set; }
        virtual public int Count { get; set; }
        public DailyAzureModel()
        { }

        public DailyAzureModel(string date, string key, string type, string name, int count)
        {
            base.PartitionKey = this.Date = date;
            base.RowKey = this.Key = key;
            this.Type = type;            
            this.Name = name;
            this.Count = count;
        }
    }
}
