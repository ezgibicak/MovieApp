﻿namespace MoviesAPI.Dto
{
    public class MovieFilterDTO
    {
        public int Page { get; set; }
        public int RecordsPerPage { get; set; }
        public PaginationDTO PaginationDTO
        {
            get { return new PaginationDTO() { Page = Page, RecordsPerPage = RecordsPerPage }; }
        }
        public string? Title { get; set; }
        public int? GenreId { get; set; }
        public bool UpcomingRelease { get; set; }
        public bool Intheater { get; set; }

    }
}
