using ConsoleHeroes.ViewModels;
using System;
using System.ComponentModel;
using System.Numerics;
using System.Text;

namespace ConsoleHeroes.Models
{
    public abstract class Hero : BaseViewModel, ICloneable
    {
        private string name;
        private long damagePerSecond;
        private long goldCost;
        private int level;
        private long totalDamage;

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged("Name");
            }
        }

        public long DamagePerSecond
        {
            get { return this.damagePerSecond; }
            set
            {
                this.damagePerSecond = value;
                this.OnPropertyChanged("DamagePerSecond");
            }
        }

        public long GoldCost
        {
            get { return this.goldCost; }
            set
            {
                this.goldCost = value;
                this.OnPropertyChanged("GoldCost");
            }
        }

        public int Level
        {
            get { return this.level; }
            set
            {
                this.level = value;
                this.OnPropertyChanged("Level");
            }
        }

        public long TotalDamage
        {
            get { return this.totalDamage; }
            private set
            {
                this.totalDamage = value;
                this.OnPropertyChanged("TotalDamage");
            }
        }

        public string ImagePath { get; set; }

        public long BaseHeroDamage { get; private set; }

        public long BaseHeroGoldCost { get; private set; }

        public Hero(string name, long damagePerSecond, long goldCost)
        {
            this.Name = name;
            this.DamagePerSecond = damagePerSecond;
            this.GoldCost = goldCost;
            this.Level = 1;

            this.BaseHeroDamage = damagePerSecond;
            this.BaseHeroGoldCost = goldCost;
            this.TotalDamage = damagePerSecond;
        }

        public override string ToString()
        {
            Hero tempHero = this.Clone();
            tempHero.LevelUp();

            StringBuilder output = new StringBuilder();

            output.AppendLine(string.Format(this.GetType().Name).PadLeft(10, '-'));
            output.AppendFormat("Level - {0}", this.Level);
            output.AppendLine();
            output.AppendFormat("Next LVL DPS - +{0}", tempHero.DamagePerSecond);
            output.AppendLine();
            output.AppendFormat("DPS - {0}", this.TotalDamage);
            output.AppendLine();
            output.AppendFormat("Lvl up - {0}", this.GoldCost);
            output.AppendLine();

            return output.ToString();
        }

        /// <summary>
        /// Upgrading the stats and the drops of the monsters
        /// </summary>
        public void LevelUp()
        {
            this.Level++;
            this.DamagePerSecond += this.BaseHeroDamage;
            this.GoldCost += this.BaseHeroGoldCost;
            this.TotalDamage += this.DamagePerSecond;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Implement the deep cloning of Hero
        /// </summary>
        /// <returns>new copy of hero</returns>
        public Hero Clone()
        {
            Hero newHero = this.MemberwiseClone() as Hero;

            return newHero;
        }
    }
}