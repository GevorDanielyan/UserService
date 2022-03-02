using System;

namespace Domain.Models
{
    public interface IUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? CreatedDateTimeUtc { get; set; }
    }
}
