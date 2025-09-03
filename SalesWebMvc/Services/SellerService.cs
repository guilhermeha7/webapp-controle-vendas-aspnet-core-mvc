using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Excepcions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        //readonly significa que o campo só pode receber valor na declaração ou dentro do construtor da classe
        private readonly SalesWebMvcContext _context; 

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> GetSellersListAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> GetByIdAsync(int id)
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            /*O método Find da classe DbSet procura um registro pelo valor da chave primária.
              Ele só funciona quando você tem a chave primária (no caso Id).
              Se encontrar, retorna o objeto correspondente.
              Se não encontrar, retorna null.*/
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seller seller)
        {
            if (!await _context.Seller.AnyAsync(x => x.Id == seller.Id)) //Se no db não tiver um seller com o Id igual ao Id que se quer atualizar, então
            {
                throw new NotFoundException("Seller not found");
            }

            //Ao tentar atualizar, o banco de dados pode lançar uma exceção de conflito de concorrência
            try
            {
                _context.Seller.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
