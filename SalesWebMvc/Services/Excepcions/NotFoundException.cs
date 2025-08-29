namespace SalesWebMvc.Services.Excepcions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base(message) 
        { 
        }
    }
}
