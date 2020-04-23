using System;
using System.Collections.Generic;

namespace ApiPetShop.DataBases
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string FirshName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
