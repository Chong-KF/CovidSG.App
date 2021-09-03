using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidSG.App.Models
{
    public class CovidDaily
    {
        public string PartitionKey { get; set; }
        public int Count { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
