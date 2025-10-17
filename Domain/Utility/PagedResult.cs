using Microsoft.EntityFrameworkCore;

namespace Domain.Utility
{
    public static class PagingDefaults
    {
        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 100;
    }

    public sealed record PagedResult<T>(
        IReadOnlyList<T> Items,
        int PageNumber,
        int PageSize,
        int TotalCount,
        int TotalPages,
        bool HasPrevious,
        bool HasNext);

    public static class PagingExtensions
    {
        public static int ClampPage(int page) =>
            page < 1 ? 1 : page;

        public static int ClampSize(int size) =>
            size < 1 ? PagingDefaults.DefaultPageSize :
            size > PagingDefaults.MaxPageSize ? PagingDefaults.MaxPageSize : size;

        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            pageNumber = ClampPage(pageNumber);
            pageSize = ClampSize(pageSize);

            var totalCount = await query.CountAsync(ct);
            var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);

            if (totalPages > 0 && pageNumber > totalPages) pageNumber = totalPages;

            var items = totalCount == 0
                ? new List<T>()
                : await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(ct);

            return new PagedResult<T>(
                items,
                pageNumber,
                pageSize,
                totalCount,
                totalPages,
                pageNumber > 1,
                totalPages > 0 && pageNumber < totalPages
            );
        }
    }
}