using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Api.Data;
using Core.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {
        //private readonly ILogger<EventoController> _logger;
        public DataContext Context { get; }

        public EventoController(DataContext context)
        {
            this.Context = context;
        }
    }
}