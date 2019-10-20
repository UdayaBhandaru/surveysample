using System;

namespace Identity.Service.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string UserProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool DeletedInd { get; set; }
        public string StateId { get; set; }
        public int? OrganizationId { get; set; }
        public int TenantId { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
    }
}
