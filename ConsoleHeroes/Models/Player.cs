using ConsoleHeroes.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ConsoleHeroes.Models
{
    public class Player : BaseViewModel
    {
        private long damagePerSecond;
        private long gold;
        private long consoleSouls;

        public long DamagePerSecond
        {
            get { return this.damagePerSecond; }
            set
            {
                this.damagePerSecond = value;
                this.OnPropertyChanged("DamagePerSecond");
            }
        }

        public long Gold
        {
            get { return this.gold; }
            set
            {
                this.gold = value;
                this.OnPropertyChanged("Gold");
            }
        }

        public long ConsoleSouls
        {
            get { return this.consoleSouls; }
            set
            {
                this.consoleSouls = value;
                this.OnPropertyChanged("ConsoleSouls");
            }
        }

        public ObservableCollection<Hero> AllHeroes { get; set; }

        public Player(long startingDPS = 1, long startingGold = 0)
        {
            this.DamagePerSecond = startingDPS;
            this.Gold = startingGold;
            this.AllHeroes = new ObservableCollection<Hero>();
        }

        /// <summary>
        /// Trying to buy hero by string input if you have enough gold
        /// If you already have this hero will level up it.
        /// </summary>
        /// <param name="hero">Hero you wanna buy</param>
        public void BuyHero(string hero)
        {
            Hero targetHero = null;
            foreach (Hero h in this.AllHeroes)
            {
                if (h.Name.ToLower() == hero.ToLower())
                {
                    targetHero = h;
                    break;
                }
            }

            bool isNowBuyed = false;

            if (targetHero == null)
            {
                isNowBuyed = true;

                switch (hero)
                {
                    case "zombie": targetHero = new Zombie(); break;
                    case "paladin": targetHero = new Paladin(); break;
                    default:
                        throw new ArgumentException(string.Format("Invalid hero '{0}'", hero));
                }
            }

            if (this.Gold >= targetHero.GoldCost)
            {
                this.Gold -= targetHero.GoldCost;

                if (!isNowBuyed)
                {
                    targetHero.LevelUp();
                }
                else
                {
                    this.AllHeroes.Add(targetHero);
                    targetHero.GoldCost *= 2;
                }

                this.DamagePerSecond += targetHero.DamagePerSecond;
            }
        }

        /// <summary>
        /// Player performs a attack to the current stage monster
        /// calculated from the damage of heroes he got
        /// </summary>
        /// <returns>dps</returns>
        public long Attack()
        {
            return this.DamagePerSecond;
        }

        /// <summary>
        /// Returns the current gold and souls that player have
        /// </summary>
        /// <returns></returns>
        public long[] GetCurrentGoldAndSouls()
        {
            long[] goldAndSouls = new long[2];
            goldAndSouls[0] = this.Gold;
            goldAndSouls[1] = this.ConsoleSouls;

            return goldAndSouls;
        }

        /// <summary>
        /// Collect reward in gold and souls
        /// </summary>
        /// <param name="rewards">gold and souls</param>
        public void ClaimReward(Dictionary<string, long> rewards)
        {
            this.Gold += rewards["Gold"];
            this.ConsoleSouls += rewards["Souls"];
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            output.AppendFormat("DPS   - {0,7:0}", this.DamagePerSecond);
            output.AppendLine();
            output.AppendFormat("Gold  - {0,7:0}", this.Gold);
            output.AppendLine();
            output.AppendFormat("Souls - {0,7:0}", this.ConsoleSouls);

            return output.ToString();
        }
    }
}
