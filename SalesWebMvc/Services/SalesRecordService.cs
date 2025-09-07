using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Models.Enums;
using SalesWebMvc.Services.Excepcions;

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

        public async Task<List<IGrouping<Department,SalesRecord>>> GetSalesListByDepartmentAsync(DateTime minDate, DateTime maxDate)
        {
            return await _context.SalesRecord
                .Where(sr => sr.Date >= minDate && sr.Date <= maxDate)
                .Include(sr => sr.Seller)
                .Include(sr => sr.Seller.Department)
                .OrderByDescending(sr => sr.Date)
                .GroupBy(sr => sr.Seller.Department)
                .ToListAsync(); //.Where.Include.GroupBy vão montando a query, mas ela somente é executada quando se usa ToListAsync()
        }

        public List<SelectListItem> GetSaleStatusList()
        {
            return Enum.GetValues(typeof(SaleStatus)) //Enum.GetValues transforma uma enumeração em um array do tipo object.
                .Cast<SaleStatus>() //É um método do LINQ que converte cada elemento de uma coleção para o tipo T
                .Select(s => new SelectListItem { Value = ((int)s).ToString(), Text = s.ToString() })
                .ToList();
        }

        public async Task InsertAsync(SalesRecord salesRecord)
        {
            _context.SalesRecord.Add(salesRecord);
            await _context.SaveChangesAsync();
        }

        public async Task<SalesRecord> GetByIdAsync(int id)
        {
            return await _context.SalesRecord.FirstOrDefaultAsync(sr => sr.Id == id);
        }

        public async Task UpdateAsync(SalesRecord salesRecord)
        {
            if (!await _context.SalesRecord.AnyAsync(sr => sr.Id == salesRecord.Id)) 
            {
                throw new NotFoundException("Seller not found");
            }

            try
            {
                _context.Update(salesRecord);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
