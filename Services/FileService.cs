using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;

namespace ApplicationTemplate.Services;

/// <summary>
///     This concrete service and method only exists an example.
///     It can either be copied and modified, or deleted.
/// </summary>
public class FileService : IFileService
{
    private string _fileName;
    private readonly ILogger<IFileService> _logger;

    public List<int> MovieIds { get; set; }
    public List<string> MovieTitles { get; set; }
    public List<string> MovieGenres { get; set; }

    #region Constructors

    public FileService(ILogger<IFileService> logger)
    {
        _logger = logger;

        _fileName = $"{Environment.CurrentDirectory}/movies.csv";

        
        MovieIds = new List<int>();
        MovieTitles = new List<string>();
        MovieGenres = new List<string>();
    }



    #endregion
    public void Read()
    {
        _logger.Log(LogLevel.Information, "Reading");
        Console.WriteLine("*** I am reading");

        
        // to populate the lists with data, read from the data file
        try
        {
            StreamReader sr = new StreamReader(_fileName);
            // first line contains column headers
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                // first look for quote(") in string
                // this indicates a comma(,) in movie title
                int idx = line.IndexOf('"');
                if (idx == -1)
                {
                    // no quote = no comma in movie title
                    // movie details are separated with comma(,)
                    string[] movieDetails = line.Split(',');
                    // 1st array element contains movie id
                    MovieIds.Add(int.Parse(movieDetails[0]));
                    // 2nd array element contains movie title
                    MovieTitles.Add(movieDetails[1]);
                    // 3rd array element contains movie genre(s)
                    // replace "|" with ", "
                    MovieGenres.Add(movieDetails[2].Replace("|", ", "));
                }
                else
                {
                    // quote = comma in movie title
                    // extract the movieId
                    MovieIds.Add(int.Parse(line.Substring(0, idx - 1)));
                    // remove movieId and first quote from string
                    line = line.Substring(idx + 1);
                    // find the next quote
                    idx = line.IndexOf('"');
                    // extract the movieTitle
                    MovieTitles.Add(line.Substring(0, idx));
                    // remove title and last comma from the string
                    line = line.Substring(idx + 2);
                    // replace the "|" with ", "
                    MovieGenres.Add(line.Replace("|", ", "));
                }
            }
            // close file when done
            sr.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        _logger.LogInformation("Movies in file {Count}", MovieIds.Count);

    }

    public void Write(int movieId,string movieTitle, string genresString)
    {
        Console.WriteLine("*** I am writing");

        StreamWriter sw = new StreamWriter(_fileName, true);
        sw.WriteLine($"{movieId},{movieTitle},{genresString}");
        sw.Close();

        MovieIds.Add(movieId);
        MovieTitles.Add(movieTitle);
        MovieGenres.Add(genresString);
        // log transaction
        _logger.LogInformation("Movie id {Id} added", movieId);
    }

    public void Display()
    {
        // Display All Movies
        // loop thru Movie Lists
        for (int i = 0; i < MovieIds.Count; i++)
        {
            // display movie details
            Console.WriteLine($"Id: {MovieIds[i]}");
            Console.WriteLine($"Title: {MovieTitles[i]}");
            Console.WriteLine($"Genre(s): {MovieGenres[i]}");
            Console.WriteLine();
        }
    }
}
