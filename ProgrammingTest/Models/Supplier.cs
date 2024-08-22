using System.ComponentModel.DataAnnotations;

namespace ProgrammingTest.Models
{
    public class Supplier
    {
        public int Id { get; set; }    
        public string SupplierCode { get; set; }       
        public string SupplierName { get; set; }
        public string Address { get; set; }       
        public string Province { get; set; }   
        public string City { get; set; }   
        public string PIC { get; set; }
    }
}
