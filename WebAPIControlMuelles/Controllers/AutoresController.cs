using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIControlMuelles.Entidades;


namespace WebAPIControlMuelles.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }
        /*
        [HttpGet]
        [HttpGet("listado")] //Un endpoint puede tener varias rutas para un mismo endpoint.
        [HttpGet("/listado")] //También podemos sacarlo de su ruta original, en este caso de api/autores, poniendo el /. Ya no estaría en autores sino en listado.
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }
        */
        //Se pueden configurar varios endpoints (acciones) para un mismo método, por ejemplo, un GET que solo devuelva el primer autor
        [HttpGet("primero")] //Para poder tener dos GET debemos ponerles diferentes rutas, por ejemplo que este esté en la ruta api/autores/primero
        public async Task<ActionResult<Autor>> PrimerAutor()
        {
            return await context.Autores.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}")] //Tambien podemos usar variables para realizar enpoints según los datos que queramos. 
        public async Task<ActionResult<Autor>> Get(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
            {
                return NotFound();
            }
            return autor;
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> Get(string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));
            if (autor == null)
            {
                return NotFound();
            }
            return autor;
        }

        [HttpGet("{id:int}/{param2}")] //Tambien podemos añadir más parámetros. Si queremos que los parametros pueda o no estar, es decir, sean opcionales le ponemos ?. {param2?}
        //Si no se introduce param2, abajo en el Get, param2 tendría valor null. Si no queremos que sea null, sino que tenga un valor por defecto se lo damos en lugar de la ? 
        // {param2=persona}
        public async Task<ActionResult<Autor>> Get(int id, string param2)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
            {
                return NotFound();
            }
            return autor;
        }


        //TIPOS DE DATOS DE RETORNO

        [HttpGet("retornos")]
        public async Task<List<Autor>> Get()
        {
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }



        [HttpPost]
        public async Task<ActionResult> Post(Autor autor) 
        {
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        //[HttpPut("algo")] Esto se combinará con la ruta de arriba, siendo la ruta total: api/autores/algo. Ese algo puede ser el ID de un autor.
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id)
            { 
                return BadRequest("El id del autor no coincide con el id de la URL"); //Un Badrequest es un error 400. Por lo tanto, luego especificamos qué está mal para que el cliente pueda corregirlo. 
            }

            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")] //api/autores/2
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
