using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InAndOut.Models
{
    public class Item
    {
		public string? UserId { get; set; }

		[Key]
        public int id { get; set; }
        [Required]
        public string Borrower { get; set; }

        [Required]
        public string Lender { get; set; }

        [DisplayName("Item name")]
        [Required]
        public string ItemName { get; set; }

    }
}
