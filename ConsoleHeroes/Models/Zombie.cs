using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleHeroes.Models
{
    public class Zombie : Hero
    {
        public Zombie()
            : base("Zombie", 2, 25)
        {
            this.ImagePath = "Assets/zombie.png";
        }
    }
}