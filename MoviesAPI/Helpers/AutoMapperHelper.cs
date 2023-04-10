using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MoviesAPI.Dto;
using MoviesAPI.Entities;
using NetTopologySuite.Geometries;

namespace MoviesAPI.Helpers
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper(GeometryFactory geometry)
        {
            CreateMap<GenreDTO, Genre>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();
            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>().ForMember(x => x.Picture, options => options.Ignore());
            CreateMap<MovieTheather, MovieTheatherDTO>().ForMember(x => x.Latitude, dto => dto.MapFrom(prop => prop.Location.Y)).ForMember(x => x.Longitude, dto => dto.MapFrom(prop => prop.Location.X));
            CreateMap<MovieTheatherCreationDTO, MovieTheather>().ForMember(x => x.Location, x => x.MapFrom(dto => geometry.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));
            CreateMap<MovieCreationDTO, Movie>().ForMember(x => x.Poster, options => options.Ignore()).ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenres)).ForMember(x => x.MovieAndMovieTheater, options => options.MapFrom(MapMovietheaterMovie)).ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActors));
            CreateMap<Movie, MovieDTO>().ForMember(x => x.Genres, options => options.MapFrom(MapMoviesToGenre)).ForMember(x => x.MovieTheathers, options => options.MapFrom(MapMovieToMovieTheter)).ForMember(x => x.Actors, options => options.MapFrom(MovieToActor));
            CreateMap<IdentityUser, UserDTO>();

        }
        private List<GenreDTO> MapMoviesToGenre(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<GenreDTO>();
            if (movie.MoviesGenres == null)
            {
                return result;
            }
            else
            {
                foreach (var genre in movie.MoviesGenres)
                {
                    result.Add(new GenreDTO() { Id = genre.GenreId, Name = genre.Genre.Name });
                }
            }
            return result;
        }
        private List<MovieTheatherDTO> MapMovieToMovieTheter(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<MovieTheatherDTO>();
            if (movie.MovieAndMovieTheater == null)
            {
                return result;
            }
            else
            {
                foreach (var movieTheather in movie.MovieAndMovieTheater)
                {
                    result.Add(new MovieTheatherDTO() { Id = movieTheather.MovieTheatherId, Name = movieTheather.MovieTheather.Name, Latitude = movieTheather.MovieTheather.Location.Y, Longitude = movieTheather.MovieTheather.Location.X });
                }
            }
            return result;
        }
        private List<ActorsMovieDTO> MovieToActor(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<ActorsMovieDTO>();
            if (movie.MoviesActors == null)
            {
                return result;
            }
            else
            {
                foreach (var actor in movie.MoviesActors)
                {
                    result.Add(new ActorsMovieDTO() { Id = actor.ActorId, Name = actor.Actor.Name, Character = actor.Character, Order = actor.Order, Picture = actor.Actor.Picture });
                }
            }
            return result;
        }
        private List<MoviesGenres> MapMoviesGenres(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesGenres>();
            if (movieCreationDTO.GenresIds == null)
            {
                return result;
            }
            else
            {
                foreach (var id in movieCreationDTO.GenresIds)
                {
                    result.Add(new MoviesGenres() { GenreId = id });
                }
            }
            return result;
        }
        private List<MovieAndMovieTheater> MapMovietheaterMovie(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            List<MovieAndMovieTheater> result = new List<MovieAndMovieTheater>();
            if (movieCreationDTO.MovieTheaterIds == null)
            {
                return result;
            }
            else
            {
                foreach (var id in movieCreationDTO.MovieTheaterIds)
                {
                    result.Add(new MovieAndMovieTheater() { MovieTheatherId = id });
                }
            }
            return result;
        }
        private List<MoviesActors> MapMoviesActors(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            List<MoviesActors> result = new List<MoviesActors>();
            if (movieCreationDTO.Actors == null)
            {
                return result;
            }
            else
            {
                foreach (var actor in movieCreationDTO.Actors)
                {
                    result.Add(new MoviesActors() { ActorId = actor.Id, Character = actor.Character });
                }
            }
            return result;
        }
    }
}
