using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;

namespace CovidSG.App.Models
{
    public class StatusAzureModel : TableEntity
    {
        virtual public string Pk { get; set; }
        virtual public string Rk { get; set; }
        virtual public string Data { get; set; }
    }
}
