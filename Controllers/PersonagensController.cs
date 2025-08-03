using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoDBZ.Data;
using ProjetoDBZ.Models;

namespace ProjetoDBZ.Controllers
{
    [ApiController]
    // quando ta entre os [] ele pega o nome direto que será personagens
    [Route("api/[controller]")]
    public class PersonagensController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public PersonagensController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // se tem conexão com o banco precisa ser async
        [HttpPost]
        public async Task<IActionResult> AddPersonagem([FromBody] Personagem personagem)
        {
            // validar o que veio do body
            if (!ModelState.IsValid)
            {
                return BadRequest(personagem);
            }

            _appDbContext.DBZ.Add(personagem);
            await _appDbContext.SaveChangesAsync();

            return Created("Personagem adicionado com sucesso", personagem);
        }

        // se usa task por que é uma funçãom async
        // actionsResult pq é uma função que retorna um código http, como o OK
        // IEnumerable é a interface básica e n pode editar o conteúdo de dentro (readOnly)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Personagem>>> GetPersonagens()
        {
            // nao precisa do Task<> aqui pois o await ja faz o unwrap
            List<Personagem> personagens = await _appDbContext.DBZ.ToListAsync();
            return Ok(personagens);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Personagem>> GetPersonagemById(int id)
        {
            Personagem personagem = await _appDbContext.DBZ.FindAsync(id);

            if (personagem == null)
            {
                return NotFound("Personagem nao encontrado");
            }
            return Ok(personagem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Personagem>> UpdatePersonagem(int id, [FromBody] Personagem personagem)
        {
            Personagem dataToUpdate = await _appDbContext.DBZ.FindAsync(id);

            if (dataToUpdate == null)
            {
                return NotFound($"Personagem com o Id-{id} não existe!!");
            }

            // evita que tenta fazer overwrite do id
            personagem.Id = dataToUpdate.Id;

            // o setValues precisa receber a mesma estrutura do tipo de dado que ta no banco
            _appDbContext.Entry(dataToUpdate).CurrentValues.SetValues(personagem);

            await _appDbContext.SaveChangesAsync();

            return StatusCode(201, personagem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonagem(int id)
        {
            Personagem personagem = await _appDbContext.DBZ.FindAsync(id);

            if (personagem == null)
            {
                return NotFound($"Personagem com o Id-{id} não existe!!");
            }

            _appDbContext.DBZ.Remove(personagem);

            await _appDbContext.SaveChangesAsync();

            return Ok("Deletado com sucesso");
        }
    }
}