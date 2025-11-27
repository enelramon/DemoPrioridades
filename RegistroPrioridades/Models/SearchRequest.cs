namespace RegistroPrioridades.Models;

/// <summary>
/// Represents a search request with pagination parameters.
/// </summary>
public class SearchRequest
{
    /// <summary>
    /// The search text to filter results.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// The current page number (1-based).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// The number of items per page.
    /// </summary>
    public int PageSize { get; set; } = 10;
}
