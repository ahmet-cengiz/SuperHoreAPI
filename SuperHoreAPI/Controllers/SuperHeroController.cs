using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SuperHoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        // db deki veri olduğunu düşünelim
        // ramda oluşturulan heroes adlı değişken dbnin örneği olmuş oluyor..
        private static List<SuperHero> heroes = new List<SuperHero>
            {
                new SuperHero {
                    Id=1,
                    Name="Spider Man",
                    FirstName="Piater",
                    LastName="Parker",
                    Place="New York City"
                },
                new SuperHero {
                    Id=2,
                    Name="Iron Man",
                    FirstName="Tony",
                    LastName="Stark",
                    Place="Long Island"
                }
            };

        // db örneğini constractorda atadım
        private readonly DataContext _context;
        public SuperHeroController(DataContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            return Ok(await _context.SuperHeroes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> Get(int id)
        {
            //var hero = heroes[id];
            var hero = await _context.SuperHeroes.FindAsync(id);
            if (hero == null)
            {
                return BadRequest("Hero not found.");
            }
            return Ok(hero);
        }


        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            _context.SuperHeroes.Add(hero);
            await _context.SaveChangesAsync();

            //heroes.Add(hero);
            return Ok(await _context.SuperHeroes.ToListAsync());
        }


        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero request)
        {
            var dbHero = await _context.SuperHeroes.FindAsync(request.Id);
            if (dbHero == null)
            {
                return BadRequest("Hero not found.");
            }
            else
            {
                dbHero.Name = request.Name;
                dbHero.FirstName = request.FirstName;
                dbHero.LastName = request.LastName;
                dbHero.Place = request.Place;
                await _context.SaveChangesAsync();
            }

            return Ok(await _context.SuperHeroes.ToListAsync());
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> Delete(int id)
        {
            //var hero = heroes[id];
            var dbhero = await _context.SuperHeroes.FindAsync(id);
            if (dbhero == null)
            {
                return BadRequest("Hero not found.");
            }
            else
            {
                _context.Remove(dbhero);
                await _context.SaveChangesAsync();
            }

            return Ok(await _context.SuperHeroes.ToListAsync());
            //return Ok($"id değeri :{id} olan hero silindi.");
        }





    }
}
