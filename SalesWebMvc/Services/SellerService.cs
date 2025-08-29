using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Excepcions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> GetSellersList()
        {
            return _context.Seller.ToList();
        }

        public void Insert(Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }

        public Seller GetById(int id)
        {
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        }

        public void Remove(int id)
        {
            /*O método Find da classe DbSet procura um registro pelo valor da chave primária.
              Ele só funciona quando você tem a chave primária (no caso Id).
              Se encontrar, retorna o objeto correspondente.
              Se não encontrar, retorna null.*/
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }

        public void Update(Seller seller)
        {
            if (!_context.Seller.Any(x => x.Id == seller.Id)) //Se no db não tiver um seller com o Id igual ao Id que se quer atualizar então
            {
                throw new NotFoundException("Seller not found");
            }

            //Ao tentar atualizar, o banco de dados pode lançar uma exceção de conflito de concorrência
            try
            {
                _context.Seller.Update(seller);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
