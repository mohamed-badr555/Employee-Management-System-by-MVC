using System;
using System.Collections.Generic;

namespace Demo.PL.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Id= Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
