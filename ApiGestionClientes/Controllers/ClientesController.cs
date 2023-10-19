using ApiGestionClientes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace ApiGestionClientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly DataContext _context;
        public ClientesController(DataContext context) 
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<List<Clientes>>> AgregarCliente(Clientes cliente)
        { 
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();

            return Ok(await _context.Cliente.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult<List<Clientes>>> ObtenerTodosClientes()
        {
            return Ok(await _context.Cliente.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Clientes>>> ObtenerClientes(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return BadRequest("Cliente no encontrado");
            }

            return Ok(cliente);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> BorrarCliente(int id)
        {
            Clientes cliente = await _context.Cliente.FindAsync(id);

            if (cliente == null)
            {
                return NotFound("Cliente no existe");
            }

            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, Clientes cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("Cliente no encontrado");
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClienteExiste(id))
                {
                    return NotFound("Cliente no existe");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private async Task<bool> ClienteExiste(int id)
        {
            return await _context.Cliente.AnyAsync(e => e.Id == id);
        }
    }
}
