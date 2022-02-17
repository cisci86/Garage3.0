using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class MemberHasMembership
    {
        [Required]
        public string MemberId { get; set; }
        [Required]
        public string MembershipId { get; set; }
        //Nullable allowed, for membership that cannot expire
        public DateTime? ExpiryDate { get; set; }

        private MemberHasMembership()
        {
            MemberId = null;
            MembershipId = null;
        }
        public MemberHasMembership(string memberId, string membershipId)
        {
            MemberId = memberId;
            MembershipId = membershipId;
        }
    }
}
