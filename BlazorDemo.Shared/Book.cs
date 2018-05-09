using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.Shared
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(13)]
        public string ISBN { get; set; }
    }
}
