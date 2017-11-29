using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAC_onomen.Models
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
