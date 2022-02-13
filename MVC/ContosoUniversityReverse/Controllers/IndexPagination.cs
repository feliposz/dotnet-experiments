using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ContosoUniversityReverse.Controllers
{
    internal static class IndexPagination
    {
        internal static async Task<IEnumerable<T>> QueryAndViewData<T>(IQueryable<T> query, string searchText, string sortOrder, int? page, ViewDataDictionary viewData, int pageSize = 10)
        {
            int count = await query.CountAsync();

            viewData["CurrentFilter"] = searchText;
            viewData["CurrentOrder"] = sortOrder;
            viewData["CurrentPage"] = page ?? 1;
            viewData["TotalPages"] = (int)Math.Ceiling(count / (double)pageSize);

            query = query.Skip(((page ?? 1) - 1) * pageSize).Take(pageSize);
 
            return await query.ToListAsync();
        }
    }
}