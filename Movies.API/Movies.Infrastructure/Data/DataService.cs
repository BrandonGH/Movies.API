using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Movies.Infrastructure.Data;

public static class DataService
{
    public static List<T> LoadFromCsv<T, TMap>(string filePath, string fileName)
        where T : class
        where TMap : ClassMap<T>
    {
        var path = Path.Combine(AppContext.BaseDirectory, filePath, fileName);
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            BadDataFound = null,
            NewLine = "\n"
        });
        
        csv.Context.RegisterClassMap<TMap>();
        
        return csv.GetRecords<T>().ToList();
    }
}