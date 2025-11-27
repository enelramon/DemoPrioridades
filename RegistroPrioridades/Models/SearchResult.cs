namespace RegistroPrioridades.Models;

/// <summary>
/// Represents a paginated search result.
/// </summary>
/// <typeparam name="T">The type of items in the result.</typeparam>
public class SearchResult<T>
{
    /// <summary>
    /// The list of items for the current page.
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// The total count of items matching the search criteria.
    /// </summary>
    public int TotalCount { get; set; }
}
