using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MAC_onomen.Models;

namespace MAC_onomen.Controllers
{
    public class SendController : Controller
    {
        [HttpPost]
        public void Send(ServiceTypeViewModel model)
        {
            var x = model;
        }
    }
}