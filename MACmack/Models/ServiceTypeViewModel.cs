using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MACmack.Models
{
    public class ServiceTypeViewModel
    {
        public string Regnumber { get; set; }
        public ServiceTypes ServiceTypes { get; set; }

    }

    public enum ServiceTypes
        {
        wash,
        fuel,
        oil,
        lamp,
        tire,
        washingfluid,
        wiper,
        air
        }
}
