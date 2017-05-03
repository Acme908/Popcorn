﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Popcorn.Models.Genres;
using Popcorn.Models.Shows;

namespace Popcorn.Services.Shows.Show
{
    public interface IShowService
    {
        /// <summary>
        /// Get show by its Imdb code
        /// </summary>
        /// <param name="imdbCode">Show's Imdb code</param>
        /// <returns>The show</returns>
        Task<ShowJson> GetShowAsync(string imdbCode);

        /// <summary>
        /// Get popular shows by page
        /// </summary>
        /// <param name="page">Page to return</param>
        /// <param name="limit">The maximum number of shows to return</param>
        /// <param name="ct">Cancellation token</param>
        /// <param name="genre">The genre to filter</param>
        /// <param name="sortBy">The sort</param>
        /// <param name="ratingFilter">Used to filter by rating</param>
        /// <returns>Popular shows and the number of shows found</returns>
        Task<(IEnumerable<ShowJson> shows, int nbShows)> GetShowsAsync(int page,
            int limit,
            double ratingFilter,
            string sortBy,
            CancellationToken ct,
            GenreJson genre = null);

        /// <summary>
        /// Search shows by criteria
        /// </summary>
        /// <param name="criteria">Criteria used for search</param>
        /// <param name="page">Page to return</param>
        /// <param name="limit">The maximum number of movies to return</param>
        /// <param name="genre">The genre to filter</param>
        /// <param name="ratingFilter">Used to filter by rating</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Searched shows and the number of movies found</returns>
        Task<(IEnumerable<ShowJson> shows, int nbShows)> SearchShowsAsync(string criteria,
            int page,
            int limit,
            GenreJson genre,
            double ratingFilter,
            CancellationToken ct);
    }
}
