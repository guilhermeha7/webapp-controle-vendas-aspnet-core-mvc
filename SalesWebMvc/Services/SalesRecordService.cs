using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> GetSalesListByDateAsync(DateTime minDate, DateTime maxDate)
        {
            return await _context.SalesRecord
                .Where(sr => sr.Date >= minDate && sr.Date <= maxDate)
                .Include(sr => sr.Seller) //Inclui a coluna Seller
                .Include(sr => sr.Seller.Department) //Inclui a coluna Department
                .OrderByDescending(sr => sr.Date)
                .ToListAsync();
        }
    }
}
