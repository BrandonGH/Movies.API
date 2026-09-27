using System.Security.Cryptography;
using System.Text;
using CsvHelper.Configuration;
using Movies.Domain.Movies;

namespace Movies.Infrastructure.Data.SeedData;

public sealed class MovieCsvMap : ClassMap<Movie>
{
    public MovieCsvMap()
    {
        // To prevent IDs being generated randomly every time a migration is created,
        // we deterministically generate an ID based on the title and release date
        Map(x => x.Id)
            .Convert(args =>
            {
                var title = args.Row.GetField("Title");
                var releaseDate = args.Row.GetField("Release_Date");
                var input = $"{title}_{releaseDate}";
                var hash = MD5.HashData(Encoding.UTF8.GetBytes(input));
                return new Guid(hash);
            });

        Map(x => x.ReleaseDate)
            .Name("Release_Date")
            .TypeConverterOption.Format("yyyy-MM-dd");

        Map(x => x.Title)
            .Name("Title");

        Map(x => x.Overview)
            .Name("Overview");

        Map(x => x.Popularity)
            .Name("Popularity");

        Map(x => x.VoteCount)
            .Name("Vote_Count");

        Map(x => x.VoteAverage)
            .Name("Vote_Average");

        Map(x => x.OriginalLanguage)
            .Name("Original_Language");

        Map(x => x.Genre)
            .Name("Genre")
            .Convert(args =>
                args.Row.GetField("Genre")!
                    .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .ToList());

        Map(x => x.PosterUrl)
            .Name("Poster_Url");
    }
}