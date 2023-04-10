using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Dto;
using MoviesAPI.Entities;
using MoviesAPI.Helpers;

namespace MoviesAPI.Controllers
{
    [Route("/api/movieTheathers")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]

    public class MovieTheatherController : ControllerBase
    {
        private readonly ILogger<MovieTheatherController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;


        public MovieTheatherController(ILogger<MovieTheatherController> logger, ApplicationDbContext context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<MovieTheatherDTO>>> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = context.MovieTheathers.AsQueryable();
            await HttpContext.InsertParametersPaginationInHeader(queryable);
            var movieTheather = await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync();
            return mapper.Map<List<MovieTheatherDTO>>(movieTheather);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieTheatherDTO>> Get(int id)
        {
            var movieTheather = await context.MovieTheathers.FirstOrDefaultAsync(x => x.Id == id);
            if (movieTheather == null)
            {
                return NotFound();
            }
            
            return mapper.Map<MovieTheatherDTO>(movieTheather);
        }
        [HttpPost]
        public async Task<ActionResult> Post(MovieTheatherCreationDTO movieCreationDTO)
        {
            var movieTheather = mapper.Map<MovieTheather>(movieCreationDTO);
            context.MovieTheathers.Add(movieTheather);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id,MovieTheatherCreationDTO movieCreationDTO)
        {
            var movieTheather = await context.MovieTheathers.FirstOrDefaultAsync(x => x.Id == id);
            if (movieTheather == null)
            {
                return NotFound();
            }
            movieTheather = mapper.Map(movieCreationDTO, movieTheather);

            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var movieTheather = await context.MovieTheathers.FirstOrDefaultAsync(x => x.Id == id);
            if (movieTheather == null)
            {
                return NotFound();
            }
            context.Remove(movieTheather);
            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
