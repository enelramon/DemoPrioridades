namespace RegistroPrioridades.Models;

/// <summary>
/// Represents the result of a search operation with pagination metadata.
/// </summary>
/// <typeparam name="T">The type of items in the result.</typeparam>
public class SearchResult<T>
{
    /// <summary>
    /// The list of items for the current page.
    /// </summary>
    public List<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// The total count of items matching the search criteria.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Calculates the total number of pages.
    /// </summary>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>The total number of pages.</returns>
    public int GetTotalPages(int pageSize)
    {
        if (pageSize <= 0) return 0;
        return (int)Math.Ceiling((double)TotalCount / pageSize);
    }
}
