using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;

namespace TableApi
{
    public class LocationAzureModel : TableEntity
    {
        virtual public string Date { get; set; }
        virtual public string Latitude { get; set; }
        virtual public string Longitude { get; set; }
        virtual public string Text { get; set; }
    }
}
