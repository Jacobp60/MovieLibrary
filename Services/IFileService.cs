using System.Collections.Generic;

namespace ApplicationTemplate.Services;

/// <summary>
///     This service interface only exists an example.
///     It can either be copied and modified, or deleted.
/// </summary>
public interface IFileService
{
    public List<int> MovieIds { get; set; }

    public List<string> MovieTitles { get; set; }

    public List<string> MovieGenres { get; set; }

    void Read();

    void Write(int movieId, string movieTitle, string genresString);

    void Display();
}
