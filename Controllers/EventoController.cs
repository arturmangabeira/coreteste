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
        private readonly ILogger<EventoController> _logger;
        public DataContext Context { get; }

        public EventoController(DataContext context)
        {
            this.Context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            /*return new Evento[]{
                new Evento(){
                    DataEvento = "12/02/2020",
                    EventoId = 1,
                    Local = "testse",
                    Lote = "1",
                    QtdPessoas = 3,
                    Tema = "Show" 
                },
                new Evento(){
                    DataEvento = "12/02/2020",
                    EventoId = 1,
                    Local = "testse",
                    Lote = "1",
                    QtdPessoas = 3,
                    Tema = "Show" 
                }
            };*/
            try            
            {
                var results = await this.Context.Eventos.ToListAsync();     
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha no banco de dados!");                
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await this.Context.Eventos.FirstOrDefaultAsync(x => x.EventoId == id);            
                return Ok(result);

            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no banco de dados!");                
            }
            
        }
    }
}