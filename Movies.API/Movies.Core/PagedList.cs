namespace Movies.Core;

public class PagedList<T>(
    List<T> items,
    int totalCount,
    int pageNumber,
    int pageSize)
    where T : class
{
    public List<T> Items { get; } = items;
    public int TotalCount { get; } = totalCount;
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
}