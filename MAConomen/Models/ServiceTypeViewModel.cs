using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MAConomen.Models
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
