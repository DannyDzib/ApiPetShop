using System;
using System.Collections.Generic;

namespace ApiPetShop.DataBases
{
    public partial class User
    {
        public User()
        {
            CarShopping = new HashSet<CarShopping>();
            Purchale = new HashSet<Purchale>();
        }

        public int Id { get; set; }
        public string FirshName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Password { get; set; }
        public byte[] Photo { get; set; }

        public virtual ICollection<CarShopping> CarShopping { get; set; }
        public virtual ICollection<Purchale> Purchale { get; set; }
    }
}
