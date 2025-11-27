namespace RegistroPrioridades.Models.Search;

/// <summary>
/// DTO for search results with pagination information.
/// </summary>
/// <typeparam name="T">The type of items in the result set.</typeparam>
public class SearchResult<T>
{
    /// <summary>
    /// The items for the current page.
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// The total count of items matching the search criteria (across all pages).
    /// </summary>
    public int TotalCount { get; set; }
}
