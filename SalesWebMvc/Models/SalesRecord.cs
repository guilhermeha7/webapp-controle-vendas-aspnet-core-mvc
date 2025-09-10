using SalesWebMvc.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMvc.Models
{
    public class SalesRecord
    {
        public int Id { get; set; }

        [DataType(DataType.Date)] //É retirado o tempo dos inputs relacionados ao atributo Date
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "{0} is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public SaleStatus Status { get; set; }

        public Seller? Seller { get; set; }
        
        [Display(Name = "Seller")]
        [Required(ErrorMessage = "{0} is required")]
        public int SellerId { get; set; }

        public SalesRecord()
        {

        }

        public SalesRecord(int id, DateTime date, double amount, SaleStatus saleStatus, Seller seller)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Status = saleStatus;
            Seller = seller;
        }
    }
}
