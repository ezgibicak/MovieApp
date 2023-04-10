using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Dto;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [Route("api/ratings")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ILogger<ActorController> logger;
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public RatingController(IMapper mapper, ILogger<ActorController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.context = context;
            this.userManager = userManager;
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] RatingDTO ratingDTO)
        {
            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
            var user = await userManager.FindByEmailAsync(email);
            var userId = user.Id;
            var currentRating = await context.Rating.FirstOrDefaultAsync(x => x.MovieId == ratingDTO.MovieId && x.UserId == userId);
            if (currentRating == null)
            {
                Rating rating = new Rating();
                rating.Rate = ratingDTO.Rate;
                rating.MovieId = ratingDTO.MovieId;
                rating.UserId = userId;
                await context.Rating.AddAsync(rating);
            }
            else
            {
                currentRating.Rate =ratingDTO.Rate;
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
