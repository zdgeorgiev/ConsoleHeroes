using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleHeroes.Models
{
    public class Paladin : Hero
    {
        public Paladin() : base("Paladin", 20, 100)
        {
            this.ImagePath = "Assets/paladin.png";
        }
    }
}