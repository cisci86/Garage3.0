using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class MemberHasMembership
    {
        public int Id { get; set; }
        [Required]
        public string MemberId { get; set; }
        public Member Member { get; set; }
        [Required]
        public string MembershipId { get; set; }
        public Membership Membership { get; set; }
        //Nullable allowed, for membership that cannot expire
        public DateTime? ExpiryDate { get; set; }
        public DateTime StartDate { get; set; }
        //Nullable allowed to keep track of the finished date if it's different from the expiry date or similar
        public DateTime? FinishedDate { get; set; }

        private MemberHasMembership()
        {
            MembershipId = null;
            ExpiryDate = null;
        }
        public MemberHasMembership(string membershipId)
        {
            MembershipId = membershipId;
        }
    }
}
