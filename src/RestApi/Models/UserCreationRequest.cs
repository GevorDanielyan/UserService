namespace RestApi.Models
{
    public class UserCreationRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
    }
}
