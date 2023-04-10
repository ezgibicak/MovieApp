using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Dto;
using MoviesAPI.Entities;
using MoviesAPI.Helpers;
using System.Linq;

namespace MoviesAPI.Controllers
{
    [Route("/api/movies")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]

    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorageService fileStorageService;
        private readonly string containerName = "movie";
        private readonly UserManager<IdentityUser> userManager;

        public MovieController(ILogger<MovieController> logger, ApplicationDbContext context, IMapper mapper, IFileStorageService fileStorageService, UserManager<IdentityUser> userManager)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
            this.userManager = userManager;

        }

        [HttpGet("PostGet")]
        public async Task<ActionResult<MoviePostGetDTO>> PostGet()
        {
            var movietheater = await context.MovieTheathers.ToListAsync();
            var genre = await context.Genre.ToListAsync();
            var movieTheaterDTO = mapper.Map<List<MovieTheatherDTO>>(movietheater);
            var genresDTO = mapper.Map<List<GenreDTO>>(genre);
            return new MoviePostGetDTO() { Genres = genresDTO, MovieTheathers = movieTheaterDTO };

        }
        [HttpGet("PutGet/{id:int}")]
        public async Task<ActionResult<MoviePutGetDTO>> PutGet(int id)
        {
            var movieResult = await Get(id);
            if (movieResult.Result is NotFoundResult)
            {
                return NotFound();
            }
            var movie = movieResult.Value;
            var selectedGenreIdList = movie.Genres.Select(x => x.Id).ToList();
            var noneSelectedGenreList = await context.Genre.Where(x => !selectedGenreIdList.Contains(x.Id)).ToListAsync();
            var selectedMovieTheaterIdList = movie.MovieTheathers.Select(x => x.Id).ToList();
            var noneSelectedMovieTheaterList = await context.MovieTheathers.Where(x => !selectedMovieTheaterIdList.Contains(x.Id)).ToListAsync();

            var noneSelectedGenreDTO = mapper.Map<List<GenreDTO>>(noneSelectedGenreList);
            var noneSelectedMovieTheterDTO = mapper.Map<List<MovieTheatherDTO>>(noneSelectedMovieTheaterList);
            var response = new MoviePutGetDTO();
            response.Movie = movie;
            response.SelectedGenres = movie.Genres;
            response.SelectedMovieTheathers = movie.MovieTheathers;
            response.NonSelectedGenres = noneSelectedGenreDTO;
            response.NonSelectedMovieTheathers = noneSelectedMovieTheterDTO;
            response.Actors = movie.Actors;
            return response;

        }

        [AllowAnonymous]
        [HttpGet("Index")]
        public async Task<ActionResult<LandingPageDTO>> Index()
        {
            var top = 6;
            var today = DateTime.Now;
            LandingPageDTO page = new LandingPageDTO();
            var inTheater = await context.Movie.Where(x => x.Intheaters == true).OrderBy(x => x.ReleaseDate).ToListAsync();
            page.InTheaters = mapper.Map<List<MovieDTO>>(inTheater);
            var upcomingRelease = await context.Movie.Where(x => x.ReleaseDate > today).OrderBy(x => x.ReleaseDate).Take(top).ToListAsync();
            page.UpComingRelease = mapper.Map<List<MovieDTO>>(upcomingRelease);
            return page;
        }
        [HttpGet]
        public async Task<ActionResult<List<MovieDTO>>> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = context.Movie.AsQueryable();
            await HttpContext.InsertParametersPaginationInHeader(queryable);
            var movie = await queryable.OrderBy(x => x.Title).Paginate(pagination).ToListAsync();
            return mapper.Map<List<MovieDTO>>(movie);
        }
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<MovieDTO>> Get(int id)
        {
            var movie = await context.Movie.Include(x => x.MoviesGenres).ThenInclude(x => x.Genre).Include(x => x.MovieAndMovieTheater).ThenInclude(x => x.MovieTheather).Include(x => x.MoviesActors).ThenInclude(x => x.Actor).FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            double averageVote = 0.0;
            int userVote = 0;
            if (await context.Rating.AnyAsync(x => x.MovieId == movie.Id))
            {
                averageVote = await context.Rating.Where(x => x.MovieId == movie.Id).AverageAsync(x => x.Rate);
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
                    var user = await userManager.FindByEmailAsync(email);
                    var userId = user.Id;
                    if (await context.Rating.AnyAsync(x => x.MovieId == movie.Id && x.UserId == userId))
                    {
                        var vote = await context.Rating.FirstOrDefaultAsync(x => x.MovieId == movie.Id && x.UserId == userId);
                        userVote = vote.Rate;
                    }
                }
            }
            var dto = mapper.Map<MovieDTO>(movie);
            dto.AvarageVote = averageVote;
            dto.UserVote = userVote;
            dto.Actors = dto.Actors.OrderBy(x => x.Order).ToList();
            return dto;
        }
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromForm] MovieCreationDTO movieCreationDTO)
        {
            try
            {
                var movie = mapper.Map<Movie>(movieCreationDTO);
                if (movieCreationDTO.Poster != null)
                {
                    movie.Poster = await fileStorageService.SaveFile(containerName, movieCreationDTO.Poster);
                }
                AnnotateActorsOrder(movie);
                context.Movie.Add(movie);
                await context.SaveChangesAsync();
                return movie.Id;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movie = await context.Movie.Include(x => x.MoviesActors).Include(x => x.MoviesGenres).Include(x => x.MovieAndMovieTheater).FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            movie = mapper.Map(movieCreationDTO, movie);
            if (movieCreationDTO.Poster != null)
            {
                movie.Poster = await fileStorageService.EditFile(containerName, movieCreationDTO.Poster, movie.Poster);
            }
            AnnotateActorsOrder(movie);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var movie = await context.Movie.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            context.Remove(movie);
            await context.SaveChangesAsync();
            await fileStorageService.DeleteFile(movie.Poster, containerName);
            return NoContent();
        }

        [HttpGet("Filter")]
        [AllowAnonymous]
        public async Task<ActionResult<List<MovieDTO>>> FilterMovie([FromQuery] MovieFilterDTO movieFilterDTO)
        {
            List<Movie> movieList = new List<Movie>();
            var movieQuery = context.Movie.AsQueryable();
            if (movieFilterDTO.Title != null)
            {
                movieQuery = movieQuery.Where(x => x.Title.Contains(movieFilterDTO.Title));
            }
            if (movieFilterDTO.Intheater)
            {
                movieQuery = movieQuery.Where(x => x.Intheaters == movieFilterDTO.Intheater);
            }
            if (movieFilterDTO.UpcomingRelease)
            {
                var today = DateTime.Now;
                movieQuery = movieQuery.Where(x => x.ReleaseDate > today);
            }
            if (movieFilterDTO.GenreId != 0)
            {
                movieQuery = movieQuery.Where(x => x.MoviesGenres.Select(y => y.GenreId).Contains((int)movieFilterDTO.GenreId));

            }
            await HttpContext.InsertParametersPaginationInHeader(movieQuery);
            movieList = await movieQuery.OrderBy(x => x.Title).Paginate(movieFilterDTO.PaginationDTO).ToListAsync();
            return mapper.Map<List<MovieDTO>>(movieList);
        }
        private void AnnotateActorsOrder(Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                int order = 1;
                foreach (var itemMovie in movie.MoviesActors)
                {
                    itemMovie.Order = order;
                    order++;
                }
            }
        }

    }
}
