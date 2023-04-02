using Sat.Recruitment.Domain.Entities.Base;
using Sat.Recruitment.Domain.Enumerartions;

namespace Sat.Recruitment.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public UserType UserType { get; set; }
        public decimal Money { get; set; }

        public User() { }

        public User(string name, string email, string address, string phone, UserType userType, decimal money) : this()
        {
            Name = name;
            Email = email;
            Address = address;
            Phone = phone;
            UserType = userType;
            Money = money;
        }

        public override string ToString() => $"Id: {Id} - Name: {Name} - Email: {Email} - Type: {UserType}";
    }
}