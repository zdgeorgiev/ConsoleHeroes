using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using ConsoleHeroes.Commands;
using ConsoleHeroes.Models;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleHeroes.ViewModels
{
    public class ConsoleHeroesModelView : BaseViewModel
    {
        private const string Exit = "-exit";
        private const string Buy = "-buy";

        private const string PlayerSavePath = "../../save.txt";

        private static int currentMonsterLife;

        private Player player;
        private Monster monster;

        private ICommand attack;
        private ICommand buyHero;

        public Player Player
        {
            get { return this.player; }
            set
            {
                this.player = value;
                this.OnPropertyChanged("Player");
            }
        }

        public Monster Monster
        {
            get { return this.monster; }
            set
            {
                this.monster = value;
                this.OnPropertyChanged("Monster");
            }
        }

        public ConsoleHeroesModelView()
        {
            this.Player = new Player(1, 1000);
            this.Monster = Monster.CreateMonster();

            this.Player.AllHeroes = new ObservableCollection<Hero>()
            {
                new Zombie(),
                new Paladin()
            };

            this.Player.DamagePerSecond += this.Player.AllHeroes[0].DamagePerSecond;
            this.Player.DamagePerSecond += this.Player.AllHeroes[1].DamagePerSecond;
        }

        public ICommand Attack
        {
            get
            {
                if (this.attack == null)
                {
                    this.attack = new RelayCommand(this.PerformAttack);
                }

                return this.attack;
            }
        }

        public ICommand BuyHero
        {
            get
            {
                if (this.buyHero == null)
                {
                    this.buyHero = new RelayCommand(this.BuyZombie);
                }

                return this.buyHero;
            }
        }

        private void BuyZombie(object obj)
        {
            this.ExecuteCommand("-buy zombie");
        }

        /// <summary>
        /// Load from external text file if the previous save exists in the following format
        /// 1. n~m(gold~souls)
        /// 2. n,m,q(monster: level,life,gold)
        /// 3. xN..zM(hero1[count]hero2[count]...)
        /// <example>1500~30 (1500 gold, 30 souls)</example>
        /// <example>20,500,280 (20 lvl monster(500 life, 280 gold)</example>
        /// <example>Z5P20 (5lvl zombie, 20lvl paladin)</example>
        /// </summary>
        private void LoadProgressIfExist(object obj)
        {
            using (StreamReader reader = new StreamReader(PlayerSavePath))
            {
                string[] goldAndSouls = reader.ReadLine().Split('~');
                int playerGold = int.Parse(goldAndSouls[0]);
                int playerSouls = int.Parse(goldAndSouls[1]);

                //Claim the gold and souls via Claim reward method
                Player.ClaimReward(new Dictionary<string, int>()
                    {
                        { "Gold", playerGold },
                        { "Souls", playerSouls }
                    }
                );

                string[] monsterStats = reader.ReadLine().Split(',');
                int level = int.Parse(monsterStats[0]);
                int life = int.Parse(monsterStats[1]);
                int goldDropped = int.Parse(monsterStats[2]);

                this.Monster = Monster.CreateMonster(level, life, goldDropped);
                currentMonsterLife = Monster.Life;

                string pattern = "[A-Z][0-9]*";

                foreach (Match match in Regex.Matches(reader.ReadLine(), pattern))
                {
                    string currentMatch = match.ToString();

                    switch (currentMatch[0])
                    {
                        case 'Z': Player.AllHeroes.Add(new Zombie()); break;
                        case 'P': Player.AllHeroes.Add(new Paladin()); break;
                        default:
                            break;
                    }

                    int targetHeroLevel = int.Parse(currentMatch.Substring(1));

                    for (int i = 0; i < Player.AllHeroes.Count; i++)
                    {
                        Hero targetHero = Player.AllHeroes[i];

                        if (targetHero.Name.Substring(0, 1) == currentMatch[0].ToString())
                        {
                            for (int j = 1; j < targetHeroLevel; j++)
                            {
                                //Simulate to add the gold needed to level up the monster via Claim reward method.
                                Player.ClaimReward(new Dictionary<string, int>()
                                {
                                    {"Gold", targetHero.GoldCost},
                                    {"Souls", 0}
                                });

                                Player.BuyHero(targetHero.Name);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Player performs attack to the current monster
        /// </summary>
        private void PerformAttack(object obj)
        {
            this.Monster.Life -= this.Player.Attack();

            if (this.Monster.Life <= 0)
            {
                this.Monster.Lifes--;
                this.Player.ClaimReward(Monster.Reward());
                this.Monster.Life = currentMonsterLife;
            }

            if (this.Monster.Lifes == 0)
            {
                this.Monster = this.Monster.LevelUp();
                currentMonsterLife = this.Monster.Life;
            }
        }

        /// <summary>
        /// Execute the commands input from another console program and 
        /// Returns the player log to external file
        /// </summary>
        /// <param name="command">command input</param>
        private void ExecuteCommand(string command)
        {
            string[] rawData = command.Split();

            switch (rawData[0])
            {
                case Buy:
                    int amount = 1;

                    if (rawData.Length == 3)
                    {
                        amount = int.Parse(rawData[2]);
                    }

                    for (int i = 0; i < amount; i++)
                    {
                        this.Player.BuyHero(rawData[1]);
                    }

                    break;

                case Exit:

                    //Before exit overrides the last save code in the external text file
                    int[] rawGoldAndSouls = Player.GetCurrentGoldAndSouls();
                    string goldAndSouls = rawGoldAndSouls[0] + "~" + rawGoldAndSouls[1];

                    string monsterStats = Monster.Level + "," + currentMonsterLife + "," + Monster.Reward()["Gold"];

                    string heroesLibrary = String.Empty;

                    foreach (Hero hero in Player.AllHeroes)
                    {
                        heroesLibrary += hero.Name[0] + hero.Level.ToString();
                    }

                    using (StreamWriter writer = new StreamWriter(PlayerSavePath, false))
                    {
                        writer.WriteLine(goldAndSouls);
                        writer.WriteLine(monsterStats);
                        writer.WriteLine(heroesLibrary);
                    }

                    Environment.Exit(0);
                    break;

                default:
                    throw new ArgumentException(string.Format("Invalid command '{0}'", command));
            }
        }
    }
}