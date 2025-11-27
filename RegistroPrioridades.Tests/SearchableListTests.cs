using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RegistroPrioridades.Components.Shared;
using RegistroPrioridades.Models.Search;
using Xunit;

namespace RegistroPrioridades.Tests;

public class SearchableListTests : TestContext
{
    private readonly Func<SearchRequest, Task<SearchResult<TestItem>>> _defaultDataProvider;
    private readonly List<SearchRequest> _capturedRequests;

    public SearchableListTests()
    {
        _capturedRequests = new List<SearchRequest>();
        _defaultDataProvider = CreateDataProvider(GenerateTestItems(25));
    }

    private static List<TestItem> GenerateTestItems(int count)
    {
        return Enumerable.Range(1, count)
            .Select(i => new TestItem { Id = i, Name = $"Item {i}" })
            .ToList();
    }

    private Func<SearchRequest, Task<SearchResult<TestItem>>> CreateDataProvider(
        List<TestItem> items,
        int delayMs = 0,
        bool throwException = false,
        string? exceptionMessage = null)
    {
        return async (request) =>
        {
            _capturedRequests.Add(request);

            if (delayMs > 0)
            {
                await Task.Delay(delayMs);
            }

            if (throwException)
            {
                throw new InvalidOperationException(exceptionMessage ?? "Test exception");
            }

            var query = items.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Text))
            {
                query = query.Where(i => i.Name.Contains(request.Text, StringComparison.OrdinalIgnoreCase));
            }

            var filteredList = query.ToList();
            var totalCount = filteredList.Count;

            var pagedItems = filteredList
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new SearchResult<TestItem>
            {
                Items = pagedItems,
                TotalCount = totalCount
            };
        };
    }

    [Fact]
    public void InitialRender_ShowsLoadingSpinner()
    {
        // Arrange - Use a provider with delay to ensure loading state is visible
        var slowProvider = CreateDataProvider(GenerateTestItems(5), delayMs: 1000);

        // Act
        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, slowProvider)
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        // Assert - During loading, spinner should be visible
        Assert.Contains("spinner-border", cut.Markup);
    }

    [Fact]
    public async Task InitialRender_DisplaysItems_AfterLoading()
    {
        // Arrange
        var items = GenerateTestItems(5);
        var provider = CreateDataProvider(items);

        // Act
        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, provider)
            .Add(p => p.PageSize, 10)
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "test-item");
                builder.AddContent(2, item.Name);
                builder.CloseElement();
            }));

        // Wait for initial load to complete
        await Task.Delay(100);
        cut.Render();

        // Assert
        var markup = cut.Markup;
        Assert.Contains("Item 1", markup);
        Assert.Contains("Item 5", markup);
        Assert.Contains("test-item", markup);
    }

    [Fact]
    public async Task InitialRender_ShowsSearchInput()
    {
        // Arrange & Act
        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, _defaultDataProvider)
            .Add(p => p.SearchPlaceholder, "Buscar productos...")
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        await Task.Delay(100);
        cut.Render();

        // Assert
        var input = cut.Find("input#searchInput");
        Assert.NotNull(input);
        Assert.Equal("Buscar productos...", input.GetAttribute("placeholder"));
    }

    [Fact]
    public async Task Debounce_TriggersSearchAfterDelay()
    {
        // Arrange
        var capturedRequests = new List<SearchRequest>();
        var provider = async (SearchRequest request) =>
        {
            capturedRequests.Add(request);
            return new SearchResult<TestItem>
            {
                Items = new List<TestItem>(),
                TotalCount = 0
            };
        };

        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, provider)
            .Add(p => p.DebounceMilliseconds, 300)
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        // Wait for initial load
        await Task.Delay(200);
        cut.Render();
        
        // Clear request history after initial load
        capturedRequests.Clear();

        // Act - Type in search box
        var input = cut.Find("input#searchInput");
        await input.InputAsync(new ChangeEventArgs { Value = "test" });

        // Wait for debounce to complete
        await Task.Delay(500);

        // Assert - Search should have been triggered with the typed text
        Assert.NotEmpty(capturedRequests);
        Assert.Equal("test", capturedRequests.Last().Text);
    }

    [Fact]
    public async Task Debounce_CancelsIntermediateRequests_WhenTypingQuickly()
    {
        // Arrange
        var capturedRequests = new List<SearchRequest>();
        var provider = async (SearchRequest request) =>
        {
            capturedRequests.Add(request);
            await Task.Delay(50); // Small delay to simulate async
            return new SearchResult<TestItem>
            {
                Items = new List<TestItem>(),
                TotalCount = 0
            };
        };

        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, provider)
            .Add(p => p.DebounceMilliseconds, 300)
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        // Wait for initial load
        await Task.Delay(100);
        capturedRequests.Clear();

        // Act - Type quickly (faster than debounce)
        var input = cut.Find("input#searchInput");
        await input.InputAsync(new ChangeEventArgs { Value = "a" });
        await Task.Delay(100);
        await input.InputAsync(new ChangeEventArgs { Value = "ab" });
        await Task.Delay(100);
        await input.InputAsync(new ChangeEventArgs { Value = "abc" });

        // Wait for debounce to complete
        await Task.Delay(400);

        // Assert - Only the final search text should be in the last request
        Assert.True(capturedRequests.Count >= 1);
        Assert.Equal("abc", capturedRequests.Last().Text);
    }

    [Fact]
    public async Task Pagination_CallsCorrectPage_WhenNextClicked()
    {
        // Arrange
        var items = GenerateTestItems(25);
        var capturedRequests = new List<SearchRequest>();
        var provider = async (SearchRequest request) =>
        {
            capturedRequests.Add(request);
            var skip = (request.Page - 1) * request.PageSize;
            var pagedItems = items.Skip(skip).Take(request.PageSize).ToList();
            return new SearchResult<TestItem>
            {
                Items = pagedItems,
                TotalCount = items.Count
            };
        };

        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, provider)
            .Add(p => p.PageSize, 10)
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        // Wait for initial load
        await Task.Delay(100);
        cut.Render();

        // Assert initial page
        Assert.Equal(1, capturedRequests.Last().Page);
        Assert.Contains("Item 1", cut.Markup);

        // Act - Click next page
        var nextButton = cut.FindAll("button.page-link").Last();
        await nextButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        await Task.Delay(100);
        cut.Render();

        // Assert - Page 2 should be requested
        Assert.Equal(2, capturedRequests.Last().Page);
        Assert.Contains("Item 11", cut.Markup);
    }

    [Fact]
    public async Task Pagination_CallsCorrectPage_WhenPreviousClicked()
    {
        // Arrange
        var items = GenerateTestItems(25);
        var capturedRequests = new List<SearchRequest>();
        var provider = async (SearchRequest request) =>
        {
            capturedRequests.Add(request);
            var skip = (request.Page - 1) * request.PageSize;
            var pagedItems = items.Skip(skip).Take(request.PageSize).ToList();
            return new SearchResult<TestItem>
            {
                Items = pagedItems,
                TotalCount = items.Count
            };
        };

        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, provider)
            .Add(p => p.PageSize, 10)
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        // Wait for initial load and go to page 2
        await Task.Delay(100);
        var nextButton = cut.FindAll("button.page-link").Last();
        await nextButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
        await Task.Delay(100);
        cut.Render();

        Assert.Equal(2, capturedRequests.Last().Page);

        // Act - Click previous page
        var prevButton = cut.FindAll("button.page-link").First();
        await prevButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        await Task.Delay(100);
        cut.Render();

        // Assert - Page 1 should be requested
        Assert.Equal(1, capturedRequests.Last().Page);
    }

    [Fact]
    public async Task Pagination_ShowsCorrectPageInfo()
    {
        // Arrange
        var items = GenerateTestItems(25);
        var provider = CreateDataProvider(items);

        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, provider)
            .Add(p => p.PageSize, 10)
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        // Wait for initial load
        await Task.Delay(100);
        cut.Render();

        // Assert - Shows correct pagination info
        Assert.Contains("Mostrando 1 - 10 de 25 resultados", cut.Markup);
        Assert.Contains("Página 1 de 3", cut.Markup);
    }

    [Fact]
    public async Task Error_DisplaysErrorMessage_WhenProviderThrows()
    {
        // Arrange
        var callCount = 0;
        var provider = async (SearchRequest request) =>
        {
            callCount++;
            if (callCount > 1) // Throw on second call
            {
                throw new InvalidOperationException("Database connection failed");
            }
            return new SearchResult<TestItem>
            {
                Items = GenerateTestItems(5),
                TotalCount = 5
            };
        };

        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, provider)
            .Add(p => p.DebounceMilliseconds, 50)
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        // Wait for initial load
        await Task.Delay(100);
        cut.Render();

        // Trigger a search that will fail
        var input = cut.Find("input#searchInput");
        await input.InputAsync(new ChangeEventArgs { Value = "test" });

        // Wait for debounce and error
        await Task.Delay(200);
        cut.Render();

        // Assert
        Assert.Contains("Database connection failed", cut.Markup);
        Assert.Contains("alert-danger", cut.Markup);
    }

    [Fact]
    public async Task Error_CanBeDismissed()
    {
        // Arrange
        var shouldFail = false;
        var provider = async (SearchRequest request) =>
        {
            if (shouldFail)
            {
                throw new InvalidOperationException("Test error");
            }
            return new SearchResult<TestItem>
            {
                Items = GenerateTestItems(5),
                TotalCount = 5
            };
        };

        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, provider)
            .Add(p => p.DebounceMilliseconds, 50)
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        // Wait for initial load
        await Task.Delay(100);
        shouldFail = true;

        // Trigger error
        var input = cut.Find("input#searchInput");
        await input.InputAsync(new ChangeEventArgs { Value = "test" });
        await Task.Delay(200);
        cut.Render();

        // Verify error is shown
        Assert.Contains("alert-danger", cut.Markup);

        // Act - Dismiss error
        var dismissButton = cut.Find(".alert-danger .btn-close");
        await dismissButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
        cut.Render();

        // Assert - Error should be dismissed
        Assert.DoesNotContain("alert-danger", cut.Markup);
    }

    [Fact]
    public async Task NoResults_DisplaysNoResultsMessage()
    {
        // Arrange
        var provider = async (SearchRequest request) =>
        {
            return new SearchResult<TestItem>
            {
                Items = new List<TestItem>(),
                TotalCount = 0
            };
        };

        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, provider)
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        // Wait for load
        await Task.Delay(100);
        cut.Render();

        // Assert
        Assert.Contains("No se encontraron resultados", cut.Markup);
        Assert.Contains("alert-info", cut.Markup);
    }

    [Fact]
    public async Task ClearButton_AppearsWhenTextEntered()
    {
        // Arrange
        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, _defaultDataProvider)
            .Add(p => p.DebounceMilliseconds, 50)
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        await Task.Delay(100);
        cut.Render();

        // Initially, no clear button
        var clearButtons = cut.FindAll("button[aria-label='Limpiar búsqueda']");
        Assert.Empty(clearButtons);

        // Type some text
        var input = cut.Find("input#searchInput");
        await input.InputAsync(new ChangeEventArgs { Value = "test" });
        cut.Render();

        // Assert - Clear button should appear
        clearButtons = cut.FindAll("button[aria-label='Limpiar búsqueda']");
        Assert.Single(clearButtons);
    }

    [Fact]
    public async Task ClearButton_ClearsSearchAndReloads()
    {
        // Arrange
        var capturedRequests = new List<SearchRequest>();
        var items = GenerateTestItems(10);
        var provider = async (SearchRequest request) =>
        {
            capturedRequests.Add(request);
            var filtered = items.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(request.Text))
            {
                filtered = filtered.Where(i => i.Name.Contains(request.Text));
            }
            return new SearchResult<TestItem>
            {
                Items = filtered.ToList(),
                TotalCount = filtered.Count()
            };
        };

        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, provider)
            .Add(p => p.DebounceMilliseconds, 50)
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        await Task.Delay(100);

        // Type some text
        var input = cut.Find("input#searchInput");
        await input.InputAsync(new ChangeEventArgs { Value = "test" });
        await Task.Delay(200);
        cut.Render();

        // Click clear button
        var clearButton = cut.Find("button[aria-label='Limpiar búsqueda']");
        await clearButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
        await Task.Delay(100);
        cut.Render();

        // Assert - Last request should have empty text
        Assert.Equal(string.Empty, capturedRequests.Last().Text);
    }

    [Fact]
    public async Task Accessibility_HasAriaLiveRegion()
    {
        // Arrange & Act
        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, _defaultDataProvider)
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        await Task.Delay(100);
        cut.Render();

        // Assert
        var liveRegion = cut.Find("[aria-live='polite']");
        Assert.NotNull(liveRegion);
    }

    [Fact]
    public async Task Accessibility_SearchInputHasAriaLabel()
    {
        // Arrange & Act
        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, _defaultDataProvider)
            .Add(p => p.SearchPlaceholder, "Buscar items")
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        await Task.Delay(100);
        cut.Render();

        // Assert
        var input = cut.Find("input#searchInput");
        Assert.Equal("Buscar items", input.GetAttribute("aria-label"));
    }

    [Fact]
    public async Task Accessibility_ResultsHaveListRole()
    {
        // Arrange
        var cut = RenderComponent<SearchableList<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, _defaultDataProvider)
            .Add(p => p.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            }));

        await Task.Delay(100);
        cut.Render();

        // Assert
        var resultsList = cut.Find("[role='list']");
        Assert.NotNull(resultsList);

        var listItems = cut.FindAll("[role='listitem']");
        Assert.NotEmpty(listItems);
    }

    /// <summary>
    /// Test item class for testing purposes.
    /// </summary>
    public class TestItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
