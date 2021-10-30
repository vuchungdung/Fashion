using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fashion.Models
{
    [Table("Discounts")]
    public class Discount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(256)]
        [Column(TypeName = "VARCHAR")]
        [Index(IsUnique = true)]
        public string Code { get; set; }
        [Required]
        public int Value { get; set; }
        public string Detail { get; set; }
        [Required]
        public int Time { get; set; }
        [Required]
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}