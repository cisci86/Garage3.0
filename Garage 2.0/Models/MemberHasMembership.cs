using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class MemberHasMembership
    {
        [Required]
        public string MemberId { get; set; }
        public Member Member { get; set; }
        [Required]
        public string MembershipId { get; set; }
        public Membership Membership { get; set; }
        //Nullable allowed, for membership that cannot expire
        public DateTime? ExpiryDate { get; set; }
    }
}
