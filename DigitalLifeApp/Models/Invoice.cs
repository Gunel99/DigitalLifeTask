using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalLifeApp.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int InvoiceNumber  { get; set; }
        [Required]
        public DateTime InvoiceDate { get; set; }
        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        [Required]
        public double NetAmount { get; set; }
        [Required]
        public double TaxAmount { get; set; }
        [Required]
        public double TotalAmount { get; set; } 
        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string Note { get; set; }
        public bool PaymentStatus { get; set; }

        public double TotalPrice()
        {
            return ((NetAmount * TaxAmount) / 100);
        }

    }

    
}
