using Ardalis.GuardClauses;
using NadinSoft.Application.Common.Abstractions;

namespace NadinSoft.Application.Common;

public sealed class PageResult<TResult>
{
    public IReadOnlyList<TResult> Items { get; }
    public int TotalCount { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber * PageSize < TotalCount;

    public List<Link> Links { get; set; } = [];

    private PageResult(List<TResult> results, int totalCount, int pageNumber, int pageSize)
    {
        Items = results.AsReadOnly();
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public static PageResult<TResult> Create(List<TResult> items, int totalCount, int pageNumber,
        int pageSize)
    {
        return new PageResult<TResult>(items, totalCount, pageNumber, pageSize);
    }
    
    public PageResult<TOut> Map<TOut>(Func<TResult, TOut> mapper)
    {
        var mappedItems = Items.Select(mapper).ToList();
        return new PageResult<TOut>(
            mappedItems,
            TotalCount,
            PageNumber,
            PageSize
        );
    }
}