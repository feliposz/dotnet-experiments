using Microsoft.EntityFrameworkCore;

namespace ContosoUniversityMvc;

public class PaginatedList<T> : List<T>
{
    public int PageNumber { get; private set; }
    public int TotalPages { get; private set; }

    public PaginatedList(List<T> items, int pageNumber, int count, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        this.AddRange(items);
    }

    public bool HasPrevPage
    {
        get
        {
            return PageNumber > 1;
        }
    }

    public bool HasNextPage
    {
        get
        {
            return PageNumber < TotalPages;
        }
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> query, int pageNumber, int pageSize)
    {
        int count = await query.CountAsync();
        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return new PaginatedList<T>(await query.ToListAsync(), pageNumber, count, pageSize);
    }
}