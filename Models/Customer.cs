using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fashion.Models
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        [Column(TypeName = "NVARCHAR")]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        [Required]
        [StringLength(250)]
        [Column(TypeName = "VARCHAR")]
        [Index(IsUnique = true)]
        public string Email { get; set; }
        [Required]
        [StringLength(250)]
        public string Address { get; set; }
        public string AddressMore { get; set; }
        [Required]
        [StringLength(10)]
        [Column(TypeName = "VARCHAR")]
        [Index(IsUnique = true)]
        public string Phone { get; set; }
        [StringLength(250)]
        [Column(TypeName = "NVARCHAR")]
        [Index(IsUnique = true)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}