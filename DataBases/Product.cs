using System;
using System.Collections.Generic;

namespace ApiPetShop.DataBases
{
    public partial class Product
    {
        public Product()
        {
            CarShopping = new HashSet<CarShopping>();
            Purchale = new HashSet<Purchale>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public byte[][] Photo { get; set; }
        public int? Price { get; set; }

        public virtual ICollection<CarShopping> CarShopping { get; set; }
        public virtual ICollection<Purchale> Purchale { get; set; }
    }
}
