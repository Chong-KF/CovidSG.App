using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;

namespace CovidSG.App.Models
{
    public class LocationAzureModel : TableEntity
    {
        virtual public string Date { get; set; }     
        virtual public string Latitude { get; set; }
        virtual public string Longitude { get; set; }
        virtual public string Text { get; set; }
        public LocationAzureModel()
        { }

        public LocationAzureModel(string date, string lat, string lng, string text)
        {
            base.PartitionKey = this.Date = date;
            base.RowKey = Guid.NewGuid().ToString();
            this.Latitude = lat;
            this.Longitude = lng;
            this.Text = text;
        }
    }
}
