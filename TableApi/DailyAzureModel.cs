using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace TableApi
{
    public class DailyAzureModel : TableEntity
    {
        virtual public string Date { get; set; }
        virtual public string Key { get; set; }
        virtual public string Type { get; set; }
        virtual public string Name { get; set; }
        virtual public int Count { get; set; }
    }
}
