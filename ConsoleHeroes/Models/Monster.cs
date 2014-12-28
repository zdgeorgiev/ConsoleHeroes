using ConsoleHeroes.ViewModels;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ConsoleHeroes.Models
{
    public class Monster : BaseViewModel
    {
        private const int PrimalBossChance = 15;
        private const int MonsterLifes = 10;

        private int ConsoleSouls = 0;

        private string name;
        private int life;
        private int level;
        private int lifes;
        private MonsterType monsterType;
        private bool isPrimal;

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged("Name");
            }
        }

        public int Life
        {
            get { return this.life; }
            set
            {
                this.life = value;
                this.OnPropertyChanged("Life");
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

        public int Lifes
        {
            get { return this.lifes; }
            set
            {
                this.lifes = value;
                this.OnPropertyChanged("Lifes");
            }
        }

        public MonsterType MonsterType
        {
            get { return this.monsterType; }
            set
            {
                this.monsterType = value;
                this.OnPropertyChanged("MonsterType");
            }
        }

        public bool IsPrimal
        {
            get { return this.isPrimal; }
            set
            {
                this.isPrimal = value;
                this.OnPropertyChanged("IsPrimal");
            }
        }

        private Dictionary<string, int> Drop { get; set; }

        //All monsters have the same health so only the names will be different
        private readonly List<string> MonsterNames =
            new List<string>() { "Monster1", "Monster2", "Monster3", "Monster4" };

        private Monster(int level, int life, int goldDrop, MonsterType monsterType = MonsterType.Normal)
        {
            this.Level = level;

            if (this.Level % 5 == 0)
            {
                this.MonsterType = MonsterType.Boss;
                this.Lifes = 1;
                this.Life = life * 7;
                CheckForPrimal();
            }
            else
            {
                this.Lifes = MonsterLifes;
                this.MonsterType = monsterType;
                this.Life = life;
            }

            this.Drop = new Dictionary<string, int>();
            this.Drop.Add("Gold", goldDrop);
            this.Drop.Add("Souls", this.ConsoleSouls);

            //return random name for the current monster
            this.Name = this.MonsterNames[new Random().Next(0, this.MonsterNames.Count)];
        }

        /// <summary>
        /// Returns new hero object called from level up method.
        /// </summary>
        /// <param name="level">level of the new hero</param>
        /// <param name="life">life of the new hero</param>
        /// <param name="goldDrop">gol drop of the new hero</param>
        /// <returns></returns>
        public static Monster CreateMonster(int level = 1, int life = 20, int goldDrop = 1)
        {
            return new Monster(level, life, goldDrop);
        }

        /// <summary>
        /// Create new hero witch is stronger than last one
        /// </summary>
        /// <returns></returns>
        public Monster LevelUp()
        {
            return CreateMonster(
                this.Level += 1,
                this.Life += 7,
                this.Drop["Gold"] += 3
            );
        }

        /// <summary>
        /// If the current stage monster is a boss there a 15% chance
        /// to that monster will be primal which gives console souls
        /// </summary>
        private void CheckForPrimal()
        {
            int randomNumber = new Random().Next(0, 101);

            if (randomNumber <= PrimalBossChance)
            {
                this.IsPrimal = true;
                this.ConsoleSouls = 6; // <- Implement ConsoleSouls Formula
            }
        }

        /// <summary>
        /// When monster is dead player recieve the gold console souls monster gives.
        /// </summary>
        /// <returns>golds and console souls</returns>
        public Dictionary<string, int> Reward()
        {
            return this.Drop;
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            output.AppendFormat("Name - {0}", this.Name);
            output.AppendLine();
            output.AppendFormat("Life - {0}", this.Life);
            output.AppendLine();
            output.AppendFormat("Monster Type - {0}", this.MonsterType);

            if (this.MonsterType == MonsterType.Boss)
            {
                output.AppendLine();
                output.AppendFormat("Primal - {0}", this.IsPrimal);
            }

            output.AppendLine();
            return output.ToString();
        }
    }
}
