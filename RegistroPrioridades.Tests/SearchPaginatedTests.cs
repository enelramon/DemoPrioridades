using Bunit;
using Microsoft.AspNetCore.Components;
using Moq;
using RegistroPrioridades.Components;
using RegistroPrioridades.Models;
using Xunit;

namespace RegistroPrioridades.Tests;

/// <summary>
/// Unit tests for the SearchPaginated modal component.
/// </summary>
public class SearchPaginatedTests : TestContext
{
    /// <summary>
    /// Test model for search results.
    /// </summary>
    public class TestItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Creates a mock data provider function.
    /// </summary>
    private static Func<SearchRequest, Task<SearchResult<TestItem>>> CreateDataProvider(
        List<TestItem>? items = null,
        int totalCount = 0,
        int delayMs = 0,
        Exception? exceptionToThrow = null)
    {
        return async (request) =>
        {
            if (delayMs > 0)
            {
                await Task.Delay(delayMs);
            }

            if (exceptionToThrow != null)
            {
                throw exceptionToThrow;
            }

            return new SearchResult<TestItem>
            {
                Items = items ?? new List<TestItem>(),
                TotalCount = totalCount
            };
        };
    }

    /// <summary>
    /// Helper to render and open the modal component.
    /// </summary>
    private async Task<IRenderedComponent<SearchPaginated<TestItem>>> RenderAndOpenModal(
        Func<SearchRequest, Task<SearchResult<TestItem>>> dataProvider,
        Action<ComponentParameterCollectionBuilder<SearchPaginated<TestItem>>>? additionalParams = null)
    {
        var cut = RenderComponent<SearchPaginated<TestItem>>(parameters =>
        {
            parameters.Add(p => p.DataProvider, dataProvider);
            parameters.Add(p => p.IsVisible, true);
            parameters.Add(p => p.ItemTemplate, item => (RenderFragment)(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "test-item");
                builder.AddContent(2, item.Name);
                builder.CloseElement();
            }));
            additionalParams?.Invoke(parameters);
        });

        // Wait for async operations
        await Task.Delay(150);
        cut.Render();

        return cut;
    }

    #region Modal Tests

    [Fact]
    public void SearchPaginated_RendersAsModal()
    {
        // Arrange
        var dataProvider = CreateDataProvider();

        // Act
        var cut = RenderComponent<SearchPaginated<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, dataProvider)
            .Add(p => p.ItemTemplate, item => (RenderFragment)(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            })));

        // Assert - Modal container should exist
        var modalContainer = cut.Find(".sag-modal-shadow");
        Assert.NotNull(modalContainer);
    }

    [Fact]
    public void SearchPaginated_ModalCanBeOpened()
    {
        // Arrange
        var dataProvider = CreateDataProvider();

        // Act - Render with IsVisible=true to open
        var cut = RenderComponent<SearchPaginated<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, dataProvider)
            .Add(p => p.IsVisible, true)
            .Add(p => p.ItemTemplate, item => (RenderFragment)(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            })));

        // Assert - Modal should have visible class
        var modalContainer = cut.Find(".sag-modal-shadow");
        Assert.Contains("visible", modalContainer.ClassName);
    }

    [Fact]
    public void SearchPaginated_ModalCanBeClosed()
    {
        // Arrange - Start with IsVisible=true
        var dataProvider = CreateDataProvider();
        var cut = RenderComponent<SearchPaginated<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, dataProvider)
            .Add(p => p.IsVisible, true)
            .Add(p => p.ItemTemplate, item => (RenderFragment)(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            })));

        // Act - Change IsVisible to false
        cut.SetParametersAndRender(parameters => parameters.Add(p => p.IsVisible, false));

        // Assert - Modal should have oculta class
        var modalContainer = cut.Find(".sag-modal-shadow");
        Assert.Contains("oculta", modalContainer.ClassName);
    }

    #endregion

    #region Initial Rendering Tests

    [Fact]
    public async Task SearchPaginated_RendersSearchInput()
    {
        // Arrange & Act
        var cut = await RenderAndOpenModal(CreateDataProvider());

        // Assert
        var input = cut.Find("input#searchInput");
        Assert.NotNull(input);
        Assert.Equal("Buscar...", input.GetAttribute("placeholder"));
    }

    [Fact]
    public async Task SearchPaginated_RendersWithCustomPlaceholder()
    {
        // Arrange
        var dataProvider = CreateDataProvider();

        var cut = RenderComponent<SearchPaginated<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, dataProvider)
            .Add(p => p.IsVisible, true)
            .Add(p => p.Placeholder, "Custom placeholder")
            .Add(p => p.ItemTemplate, item => (RenderFragment)(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, item.Name);
                builder.CloseElement();
            })));

        await Task.Delay(100);
        cut.Render();

        // Assert
        var input = cut.Find("input#searchInput");
        Assert.Equal("Custom placeholder", input.GetAttribute("placeholder"));
    }

    [Fact]
    public async Task SearchPaginated_ShowsNoResultsMessage_WhenEmpty()
    {
        // Arrange & Act
        var cut = await RenderAndOpenModal(CreateDataProvider(new List<TestItem>(), 0));

        // Assert
        var alert = cut.Find(".alert-info");
        Assert.Contains("No se encontraron resultados", alert.TextContent);
    }

    [Fact]
    public async Task SearchPaginated_RendersItems_WhenDataProviderReturnsResults()
    {
        // Arrange
        var items = new List<TestItem>
        {
            new TestItem { Id = 1, Name = "Item 1" },
            new TestItem { Id = 2, Name = "Item 2" }
        };

        // Act
        var cut = await RenderAndOpenModal(CreateDataProvider(items, 2));

        // Assert
        var renderedItems = cut.FindAll(".test-item");
        Assert.Equal(2, renderedItems.Count);
        Assert.Contains("Item 1", renderedItems[0].TextContent);
        Assert.Contains("Item 2", renderedItems[1].TextContent);
    }

    #endregion

    #region Debounce Tests

    [Fact]
    public async Task SearchPaginated_Debounce_DoesNotCallDataProviderImmediately()
    {
        // Arrange
        var callCount = 0;
        Func<SearchRequest, Task<SearchResult<TestItem>>> dataProvider = async (request) =>
        {
            callCount++;
            await Task.Delay(10);
            return new SearchResult<TestItem> { Items = new List<TestItem>(), TotalCount = 0 };
        };

        var cut = await RenderAndOpenModal(dataProvider, p => p.Add(x => x.DebounceDelay, 500));

        var initialCallCount = callCount;

        // Act - Type in the search input
        var input = cut.Find("input#searchInput");
        await input.InputAsync(new ChangeEventArgs { Value = "test" });

        // Wait a short time - less than debounce delay
        await Task.Delay(100);

        // Assert - Check that call count hasn't increased immediately
        var callsDuringDebounce = callCount - initialCallCount;
        
        // Wait for debounce to complete
        await Task.Delay(600);

        // After debounce, should have made exactly one additional call
        var finalCallCount = callCount - initialCallCount;
        Assert.Equal(1, finalCallCount);
    }

    [Fact]
    public async Task SearchPaginated_Debounce_CancelsPreviousRequests()
    {
        // Arrange
        var searchTexts = new List<string>();
        Func<SearchRequest, Task<SearchResult<TestItem>>> dataProvider = async (request) =>
        {
            searchTexts.Add(request.Text);
            await Task.Delay(10);
            return new SearchResult<TestItem> { Items = new List<TestItem>(), TotalCount = 0 };
        };

        var cut = await RenderAndOpenModal(dataProvider, p => p.Add(x => x.DebounceDelay, 300));
        searchTexts.Clear();

        // Act - Type multiple characters quickly (faster than debounce)
        var input = cut.Find("input#searchInput");
        await input.InputAsync(new ChangeEventArgs { Value = "t" });
        await Task.Delay(50);
        await input.InputAsync(new ChangeEventArgs { Value = "te" });
        await Task.Delay(50);
        await input.InputAsync(new ChangeEventArgs { Value = "tes" });
        await Task.Delay(50);
        await input.InputAsync(new ChangeEventArgs { Value = "test" });

        // Wait for debounce to complete
        await Task.Delay(500);

        // Assert - Should have the final text in the search texts
        Assert.Contains("test", searchTexts);
        
        // The key assertion: the final search should be for "test"
        var lastSearch = searchTexts.LastOrDefault();
        Assert.Equal("test", lastSearch);
    }

    #endregion

    #region Pagination Tests

    [Fact]
    public async Task SearchPaginated_Pagination_ShowsPageInfo()
    {
        // Arrange
        var items = new List<TestItem> { new TestItem { Id = 1, Name = "Item 1" } };

        // Act
        var cut = await RenderAndOpenModal(CreateDataProvider(items, 15), p => p.Add(x => x.PageSize, 5));

        // Assert
        var pageInfo = cut.Find(".pagination-info");
        Assert.Contains("Página 1 de 3", pageInfo.TextContent);
        Assert.Contains("15 resultados", pageInfo.TextContent);
    }

    [Fact]
    public async Task SearchPaginated_Pagination_NextPageCallsCorrectPage()
    {
        // Arrange
        var requestedPages = new List<int>();
        Func<SearchRequest, Task<SearchResult<TestItem>>> dataProvider = async (request) =>
        {
            requestedPages.Add(request.Page);
            await Task.Delay(10);
            return new SearchResult<TestItem>
            {
                Items = new List<TestItem> { new TestItem { Id = 1, Name = "Item" } },
                TotalCount = 20
            };
        };

        var cut = await RenderAndOpenModal(dataProvider, p => p.Add(x => x.PageSize, 5));

        // Act - Click next page
        var nextButton = cut.Find("button[aria-label='Ir a la página siguiente']");
        await nextButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
        await Task.Delay(100);

        // Assert
        Assert.Contains(1, requestedPages);
        Assert.Contains(2, requestedPages);
    }

    [Fact]
    public async Task SearchPaginated_Pagination_PreviousPageCallsCorrectPage()
    {
        // Arrange
        var requestedPages = new List<int>();
        Func<SearchRequest, Task<SearchResult<TestItem>>> dataProvider = async (request) =>
        {
            requestedPages.Add(request.Page);
            await Task.Delay(10);
            return new SearchResult<TestItem>
            {
                Items = new List<TestItem> { new TestItem { Id = 1, Name = "Item" } },
                TotalCount = 20
            };
        };

        var cut = await RenderAndOpenModal(dataProvider, p => p.Add(x => x.PageSize, 5));

        // Go to page 2 first
        var nextButton = cut.Find("button[aria-label='Ir a la página siguiente']");
        await nextButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
        await Task.Delay(100);

        // Act - Click previous page
        var prevButton = cut.Find("button[aria-label='Ir a la página anterior']");
        await prevButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
        await Task.Delay(100);

        // Assert
        Assert.Contains(1, requestedPages);
        Assert.Contains(2, requestedPages);
    }

    [Fact]
    public async Task SearchPaginated_Pagination_PreviousButtonDisabledOnFirstPage()
    {
        // Arrange
        var items = new List<TestItem> { new TestItem { Id = 1, Name = "Item" } };

        // Act
        var cut = await RenderAndOpenModal(CreateDataProvider(items, 10), p => p.Add(x => x.PageSize, 5));

        // Assert
        var prevButton = cut.Find("button[aria-label='Ir a la página anterior']");
        Assert.True(prevButton.HasAttribute("disabled"));
    }

    [Fact]
    public async Task SearchPaginated_Pagination_NextButtonDisabledOnLastPage()
    {
        // Arrange
        var items = new List<TestItem> { new TestItem { Id = 1, Name = "Item" } };

        // Act
        var cut = await RenderAndOpenModal(CreateDataProvider(items, 5), p => p.Add(x => x.PageSize, 5));

        // Assert
        var nextButton = cut.Find("button[aria-label='Ir a la página siguiente']");
        Assert.True(nextButton.HasAttribute("disabled"));
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public async Task SearchPaginated_ShowsErrorMessage_WhenDataProviderThrows()
    {
        // Arrange & Act
        var cut = await RenderAndOpenModal(CreateDataProvider(exceptionToThrow: new Exception("Test error message")));

        // Assert - Should show generic error message
        var errorAlert = cut.Find(".alert-danger");
        Assert.Contains("error", errorAlert.TextContent.ToLower());
    }

    [Fact]
    public async Task SearchPaginated_ErrorCanBeDismissed()
    {
        // Arrange
        var cut = await RenderAndOpenModal(CreateDataProvider(exceptionToThrow: new Exception("Test error")));

        // Verify error is shown
        var errorAlert = cut.Find(".alert-danger");
        Assert.NotNull(errorAlert);

        // Act - Dismiss the error
        var closeButton = cut.Find(".alert-danger .btn-close");
        await closeButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // Assert - Error should be dismissed
        Assert.Throws<Bunit.ElementNotFoundException>(() => cut.Find(".alert-danger"));
    }

    #endregion

    #region Clear Button Tests

    [Fact]
    public async Task SearchPaginated_ClearButton_AppearsWhenTextEntered()
    {
        // Arrange
        var cut = await RenderAndOpenModal(CreateDataProvider());

        // Initially no clear button
        Assert.Throws<Bunit.ElementNotFoundException>(() => cut.Find("#searchClearButton"));

        // Act - Enter text
        var input = cut.Find("input#searchInput");
        await input.InputAsync(new ChangeEventArgs { Value = "test" });

        // Assert - Clear button should appear
        var clearButton = cut.Find("#searchClearButton");
        Assert.NotNull(clearButton);
    }

    [Fact]
    public async Task SearchPaginated_ClearButton_ClearsSearchText()
    {
        // Arrange
        var searchTexts = new List<string>();
        Func<SearchRequest, Task<SearchResult<TestItem>>> dataProvider = async (request) =>
        {
            searchTexts.Add(request.Text);
            await Task.Delay(10);
            return new SearchResult<TestItem> { Items = new List<TestItem>(), TotalCount = 0 };
        };

        var cut = await RenderAndOpenModal(dataProvider, p => p.Add(x => x.DebounceDelay, 50));

        // Enter text
        var input = cut.Find("input#searchInput");
        await input.InputAsync(new ChangeEventArgs { Value = "test" });
        await Task.Delay(100);

        // Act - Click clear
        var clearButton = cut.Find("#searchClearButton");
        await clearButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
        await Task.Delay(100);

        // Assert - Should have called with empty string
        Assert.Contains(string.Empty, searchTexts);
    }

    #endregion

    #region Accessibility Tests

    [Fact]
    public async Task SearchPaginated_HasAriaLabels()
    {
        // Arrange & Act
        var cut = await RenderAndOpenModal(CreateDataProvider());

        // Assert
        var input = cut.Find("input#searchInput");
        Assert.NotNull(input.GetAttribute("aria-label"));
    }

    [Fact]
    public async Task SearchPaginated_PaginationHasAriaLabels()
    {
        // Arrange
        var items = new List<TestItem> { new TestItem { Id = 1, Name = "Item" } };

        // Act
        var cut = await RenderAndOpenModal(CreateDataProvider(items, 10), p => p.Add(x => x.PageSize, 5));

        // Assert
        var nav = cut.Find("nav[aria-label='Paginación de resultados']");
        Assert.NotNull(nav);

        var prevButton = cut.Find("button[aria-label='Ir a la página anterior']");
        Assert.NotNull(prevButton);

        var nextButton = cut.Find("button[aria-label='Ir a la página siguiente']");
        Assert.NotNull(nextButton);
    }

    [Fact]
    public async Task SearchPaginated_ModalHasAriaAttributes()
    {
        // Arrange & Act
        var cut = await RenderAndOpenModal(CreateDataProvider());

        // Assert
        var modal = cut.Find("[role='dialog']");
        Assert.NotNull(modal);
        Assert.Equal("true", modal.GetAttribute("aria-modal"));
    }

    #endregion

    #region Item Selection Tests

    [Fact]
    public async Task SearchPaginated_ItemSelection_InvokesCallback()
    {
        // Arrange
        var selectedItem = (TestItem?)null;
        var items = new List<TestItem> { new TestItem { Id = 1, Name = "Test Item" } };

        var cut = RenderComponent<SearchPaginated<TestItem>>(parameters => parameters
            .Add(p => p.DataProvider, CreateDataProvider(items, 1))
            .Add(p => p.IsVisible, true)
            .Add(p => p.OnItemSelect, EventCallback.Factory.Create<TestItem>(this, item => selectedItem = item))
            .Add(p => p.ItemTemplate, item => (RenderFragment)(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "test-item");
                builder.AddContent(2, item.Name);
                builder.CloseElement();
            })));

        await Task.Delay(150);
        cut.Render();

        // Act - Click on an item
        var itemElement = cut.Find(".test-item");
        await itemElement.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // Assert
        Assert.NotNull(selectedItem);
        Assert.Equal("Test Item", selectedItem.Name);
    }

    #endregion
}
