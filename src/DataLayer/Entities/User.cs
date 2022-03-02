using System;
using Domain.Models;

namespace DataLayer.Entities
{
    internal class User : IUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? CreatedDateTimeUtc { get; set; }
    }
}
