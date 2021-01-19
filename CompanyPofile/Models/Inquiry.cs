using BoGroent.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyPofile.Models
{
    public class Inquiry
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwe firmy"), StringLength(100, MinimumLength = 4)]
        [Display(Name = "Nazwa firmy")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Proszę podać imię i nazwisko"), StringLength(100, MinimumLength = 4)]
        [Display(Name = "Imię i nazwisko")]
        public string ContactFullName { get; set; }

        [Required(ErrorMessage = "Proszę podać numer telefonu"), DataType(DataType.PhoneNumber)]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Proszę podać email"), EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Proszę wpisać wiadomość"), MinLength(20), MaxLength(5000)]
        [Display(Name = "Wiadomość")]
        public string Message { get; set; }

        [Column(TypeName = "nvarchar(max)"), Display(Name = "Attachment")]
        public string Attachment { get; set; }

        [NotMapped, FileExtension]
        public IFormFile AttachmentUpload { get; set; }

        [DataType(DataType.Date), Display(Name = "Created Date")]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date), Display(Name = "Updated Date")]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyy}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedDate { get; set; }

        public string Browser { get; set; }
        public string OperatingSystem { get; set; }
        public string Device { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string IpAddress { get; set; }

        public string Status { get; set; }

        public string Notes { get; set; }

        [Range(0, 10000), DataType(DataType.Currency), Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "HourS Spend")]
        public decimal HoursSpend { get; set; }

        [Range(0, 1000), DataType(DataType.Currency), Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Hour Price")]
        public decimal HourPrice { get; set; }

        [DataType(DataType.Currency), Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get { return HoursSpend * HourPrice; } }

        public string InvoiceNo { get; set; }

        public string Payment { get; set; }

    }
}
