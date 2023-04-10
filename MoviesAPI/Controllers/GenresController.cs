using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Dto;
using MoviesAPI.Entities;
using MoviesAPI.Filters;
using MoviesAPI.Helpers;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]

    public class GenresController : ControllerBase
    {
        private readonly ILogger<GenresController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenresController(ILogger<GenresController> logger, ApplicationDbContext context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<GenreDTO>>> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = context.Genre.AsQueryable();
            await HttpContext.InsertParametersPaginationInHeader(queryable);
            var genres = await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync();
            logger.LogInformation("Get All Genre");
            return mapper.Map<List<GenreDTO>>(genres);
        }

        [HttpGet("All")]
        [AllowAnonymous]
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {
            var genre = await context.Genre.OrderBy(x=>x.Name).ToListAsync();
            return mapper.Map<List<GenreDTO>>(genre);
        }
        [HttpGet("{id:int}")]
        //[ResponseCache(Duration = 60)]
        //[ServiceFilter(typeof(CustomActionFilter))]
        public async Task<ActionResult<GenreDTO>> Get(int id)
        {
            var genre = await context.Genre.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (genre == null)
            {
                return NotFound();
            }
            return mapper.Map<GenreDTO>(genre);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreCreationDTO genreCreation)
        {
            var genre = mapper.Map<Genre>(genreCreation);
            context.Genre.Add(genre);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenreCreationDTO genreCreation)
        {
            var genre = await context.Genre.FirstOrDefaultAsync(z => z.Id == id);
            if (genre == null)
            {
                return NotFound();
            }
            genre = mapper.Map(genreCreation, genre);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Genre.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }
            context.Remove(new Genre()
            {
                Id = id,
            });
            context.SaveChanges();
            return NoContent();
        }
    }
}
