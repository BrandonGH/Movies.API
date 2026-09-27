using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Core;
using Movies.Domain.Movies;
using Movies.Infrastructure.Data.SeedData;

namespace Movies.Infrastructure.Data.EntityConfigurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.ReleaseDate).IsRequired();
        builder.Property(m => m.Title).IsRequired();
        builder.Property(m => m.Overview).IsRequired();
        builder.Property(m => m.Popularity).IsRequired();
        builder.Property(m => m.VoteCount).IsRequired();
        builder.Property(m => m.VoteAverage).IsRequired();
        builder.Property(m => m.OriginalLanguage).IsRequired();
        builder.Property(m => m.Genre).IsRequired();
        builder.Property(m => m.PosterUrl).IsRequired();

        builder.Property(m => m.Title)
            .HasMaxLength(256);
        builder.Property(m => m.Overview)
            .HasMaxLength(2048);
        builder.Property(m => m.OriginalLanguage)
            .HasMaxLength(64);
        builder.Property(m => m.PosterUrl)
            .HasMaxLength(256);
        
        var movies = 
            DataService.LoadFromCsv<Movie, MovieCsvMap>(
                "../../../../Movies.Infrastructure/Data/SeedData",
                "mymoviedb.csv");

        builder.HasData(movies);
    }
}