using System.ComponentModel.DataAnnotations;

namespace LibraryDemo.Models
{
    public class Shirt
    {
        [Key]
        public int ShirtId { get; set; }

		//[Display(Name = "Shirt Rank")]
        public int rank { get; set; }

        [Display(Name = "Shirt Brand")]
        public string? Brand { get; set; }

        [Display(Name = "Shirt Color")]
        public string? Color { get; set; }

        [Display(Name = "Shirt Size")]
        public int? Size { get; set; }

        [Display(Name = "Shirt Gender")]
        public string? Gender { get; set; }

        [Display(Name = "Shirt Price")]
        public double? price { get; set; }
    }
}
