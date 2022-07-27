using System;

namespace Service.Outputs
{
    public class UserDetails
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Balance { get; set; }
    }
}