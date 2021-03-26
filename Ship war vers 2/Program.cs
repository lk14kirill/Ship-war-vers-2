using System;

namespace Ship_war_vers_2
{
    #region Ship
    enum ShipState
    {
        alive,
        destroyed,
        unspawned
    }
    class Ship
    {
        public int width;
        public int[] height;
        public int size;
        public ShipState state;
        #region Constructor
        public Ship(int size, int[] height, int width, ShipState state)
        {
            this.size = size;
            this.height = height;
            this.width = width;
            this.state = state;
        }
        #endregion
    }
    #endregion Ship
    #region AI

    #region ShipGeneration
    class ShipGenerationAI
    {   
        public static Ship[] ships;
        public static int[] GenerateCoordinates(int size)
        {
            int[] height = new int[size];
            Random rand = new Random();
            height[0] = rand.Next(1, 10);
            for (int i = 1; i < size; i++)
            {
                height[i] = height[0] + 1;
            }
            return height;
        }
        public static void TryToCreateShip(int size, int width, int number, int[] height)
        {

            if (CanCreateShip(width, height, size))
            {
                GridGenerationAI.DrawAI(height, width, size); ;
            }
            else
            {
                ships[number].state = ShipState.unspawned;
            }
        }

        public static bool CanCreateShip(int width, int[] height, int size)
        {
            for (int i = 0; i < GridGeneration.height && size > 0; i++)
            {
                if (GridGenerationAI.field[height[size - 1], width] != '.')
                {
                    return false;
                }
                if (GridGenerationAI.field[height[size - 1], width + 1] != '.')
                {
                    return false;
                }
                if (GridGenerationAI.field[height[size - 1], width - 1] != '.')
                {
                    return false;
                }
                size--;
            }
            return true;
        }
        public static void GenerateShip(int size, int quantity)
        {
            Random rand = new Random();
            ships = new Ship[quantity];
            for (int i = 0; i < quantity; i++)
            {
                int[] height = GenerateCoordinates(size);
                int width = rand.Next(1, 10);
                ships[i] = new Ship(size, height, width, ShipState.alive);
                TryToCreateShip(size, width, i, height);
            }
        }
    }
    #endregion
    #region GridGeneration
    class GridGenerationAI
    {
        public static int width = 11;
        public static int height = 11;
        public static char[,] field = new char[height, width];
        static char symbol;
        #region AIGeneration
        public static void GenerateFieldAI()
        {
            for (int i = 0; i < height; i++)
            {

                int numberToChar = 64;
                numberToChar++;

                for (int j = 0; j < width; j++)
                {
                    symbol = '.';
                    if (i != 0 && j != 0)
                    {
                        field[i, j] = symbol;
                    }
                }
                field[i, 0] = (char)numberToChar;
            }
        }
        public static void UnlockShot(int height, int width, char bullet)
        {
            Console.WriteLine();
            int numberToChar = 63;
            int number = 47;
            for (int i = 0; i < GridGeneration.height - 2; i++)
            {
                number++;
                for (int j = 0; j < GridGeneration.width; j++)
                {
                    numberToChar++;

                    symbol = field[i, j];
                    if (i == 0 && j >= 1)
                    {
                        symbol = (char)numberToChar;
                    }
                    if (i >= 1 && j == 0)
                    {
                        symbol = (char)number;
                    }
                    if (j == 10)
                    {
                        symbol = '|';
                    }
                    if (symbol == '!')
                    {
                        symbol = '.';
                    }
                    if (height == i && width == j)
                    {
                        field[i, j] = bullet;
                        symbol = bullet;
                    }
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }
        }
        public static void UpdateFieldAIAndDontShowShips()
        {
            Console.WriteLine();
            int numberToChar = 63;
            int number = 47;
            for (int i = 0; i < height - 2; i++)
            {
                number++;
                for (int j = 0; j < width; j++)
                {
                    numberToChar++;
                    symbol = field[i, j];
                    if (i == 0 && j >= 1)
                    {
                        symbol = (char)numberToChar;
                    }
                    if (i >= 1 && j == 0)
                    {
                        symbol = (char)number;
                    }
                    if (j == 10)
                    {
                        symbol = '|';
                    }
                    if (symbol == '!')
                    {
                        symbol = '.';
                    }
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }
        }
        public static void Shoot(int height,int width,char bullet)
        {
            
            GridGeneration.UpdateField();
            UnlockShot(height, width, bullet);
            
            if (bullet == '#' && !GameCycle.isgameEnded)
            {
                EndOfGame.IfPlayerWon();
                if (GameCycle.isgameEnded)
                {
                    GameCycle.GameProcess();

                }
                else
                {
                    PlayerInteraction.Attack();
                    Logic.wasHitFromPlayer();
                }
            }
        }
        public static void DrawAI(int[] height, int width, int size)
        {
            int numberToChar = 63;
            int number = 47;
            int f = 0;
            for (int i = 0; i < GridGeneration.height - size; i++)
            {
                number++;
                for (int j = 0; j < GridGeneration.width; j++)
                {
                    numberToChar++;


                    symbol = '.';
                    if (i == 0 && j >= 1)
                    {
                        symbol = (char)numberToChar;
                    }
                    if (i >= 1 && j == 0)
                    {
                        symbol = (char)number;
                    }
                    if (f < size && height[f] == i && width == j)
                    {
                        field[i, j] = '!';
                        symbol = '!';
                        f++;
                    }
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
    #endregion

    #region Interaction
    class AIInteraction
    {
        public static int attackHeight;
        public static int attackWidth;
        public static (int, int) Attack()
        {
            UI.WriteASentence(ConsoleColor.Cyan, "Wait,enemy is attacking you!");
            System.Threading.Thread.Sleep(1500);
            Random rand = new Random();
            attackHeight = rand.Next(1, 10);
            attackWidth = rand.Next(1, 10);

            return (attackHeight, attackWidth);
        }
    }
    #endregion

    #endregion
    #region Player

    #region ShipGeneration
    class ShipGeneration
    {
        public static Ship[] ships;
        public static int[] GenerateCoordinates(int size)
        {
            int[] height = new int[size];
            Random rand = new Random();
            height[0] = rand.Next(1, 10);
            for (int i = 1; i < size; i++)
            {
                height[i] = height[0] + 1;
            }
            return height;
        }
        public static void TryToCreateShip(int size,int width,int number,int[] height)
        {
            if(CanCreateShip(width, height, size))
            {
                GridGeneration.Draw(height, width, size); ;
            }
            else
            {
                ships[number].state = ShipState.unspawned;
            }
        }
        public static bool CanCreateShip(int width,int[] height,int size)
        {
            for (int i = 0; i < GridGeneration.height && size > 0 ; i++)
            {
                if(GridGeneration.field[height[size - 1],width] != '.')
                {
                    return false;
                }
                if(GridGeneration.field[height[size - 1], width + 1] != '.')
                {
                    return false;
                }
                if (GridGeneration.field[height[size - 1], width - 1] != '.')
                {
                    return false;
                }
                size--;
            }
            return true;
        }
        public static void GenerateShip(int size,int quantity)
        {
            Random rand = new Random();
            ships = new Ship[quantity];
            for (int i = 0; i < quantity; i++)
            {
            int[] height = GenerateCoordinates(size);
            int width = rand.Next(1, 10);
            ships[i] = new Ship(size, height, width,ShipState.alive);
                TryToCreateShip(size, width, i, height);
            
            }
        }
    }
    #endregion
    #region GridGeneration
    class GridGeneration
    {
        public static int width = 11;
        public static int height = 11;
        public static char[,] field = new char[height, width];
        static char symbol;
       
        public static void GenerateField()
        {
            for (int i = 0; i < height; i++)
            {
                
                int numberToChar = 64;
                numberToChar++;

                for (int j = 0; j < width; j++)
                {
                    symbol = '.';
                    if (i != 0 && j != 0)
                    {
                        field[i, j] = symbol;
                    }
                }
                field[i, 0] = (char)numberToChar;
            }
        }
        public static void Draw(int[] height,int width,int size)
        {
            int numberToChar = 63;
            int number = 47;
            int f = 0;
            for (int i = 0; i < GridGeneration.height - size  ; i++)
            {
                number++;
                  for (int j = 0;  j < GridGeneration.width; j++)
                  {
                    numberToChar++;

                    symbol = '.';
                    if (i == 0 && j >= 1)
                    {
                        symbol = (char)numberToChar;
                    }
                    if (i >= 1 && j == 0)
                    {
                        symbol = (char)number;
                    }
                    if ( f < size && height[f] == i && width == j )
                    {
                        field[i, j] = '!';
                        symbol = '!';
                        f++;             
                    }
                    Console.Write(symbol); 
                  }
              Console.WriteLine();
            }
        }
        
        public static void Shoot(int height,int width,char bullet)
        {
            Console.Clear();
            UnlockShot(height, width, bullet);
            GridGenerationAI.UpdateFieldAIAndDontShowShips();
            if (bullet == '#')
            {
                EndOfGame.IfPlayerWon();
                if (GameCycle.isgameEnded)
                {
                    GameCycle.GameProcess();

                }
                else
                {
                    AIInteraction.Attack();
                    Logic.wasHittedFromAI();
                }
            }
        }
        public static void UnlockShot(int height, int width, char bullet)
        {
            int numberToChar = 63;
            int number = 47;
            for (int i = 0; i < GridGeneration.height - 2; i++)
            {
                number++;
                for (int j = 0; j < GridGeneration.width; j++)
                {
                    numberToChar++;

                    symbol = field[i, j];
                    if (i == 0 && j >= 1)
                    {
                        symbol = (char)numberToChar;
                    }
                    if (i >= 1 && j == 0)
                    {
                        symbol = (char)number;
                    }
                    if (j == 10)
                    {
                        symbol = '|';
                    }
                    if (height == i && width == j)
                    {
                        field[i, j] = bullet;
                        symbol = bullet;
                    }
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }
        }
       
        public static void UpdateField()
        {
            Console.Clear();
            int numberToChar = 63;
            int number = 47;
            for (int i = 0; i < height - 2; i++)
            {
                number++;
                for (int j = 0; j < width; j++)
                {
                    numberToChar++;
                    symbol = field[i, j];
                    if (i == 0 && j >= 1)
                    {
                        symbol = (char)numberToChar;
                    }
                    if (i >= 1 && j == 0)
                    {
                        symbol = (char)number;
                    }
                    if (j == 10)
                    {
                        symbol = '|';
                    }
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }           
        }
    }
    #endregion
    #region Interation
    class PlayerInteraction
    {
        public static int attackHeight;
        public static int attackWidth;
        public static (int,int) Attack()
        {
            UI.WriteASentence(ConsoleColor.Cyan, "Enter the number,then letters,where you wanna hit");

            attackHeight = int.Parse(Console.ReadLine());
            char letter = char.Parse(Console.ReadLine());
            attackWidth = (int)letter - 64;
           
            return (attackHeight, attackWidth);
        }
    }
    #endregion

    #endregion
    #region UI
    class UI
    {
        public static void GameSettings()
        {
            WriteASentence(ConsoleColor.Cyan, "Welcome to ships war!");
            WriteASentence(ConsoleColor.Cyan, "You can play agains our bot.Here some instructions:");
            WriteASentence(ConsoleColor.Cyan, "^ - means you missed");
            WriteASentence(ConsoleColor.Cyan, "# - means you hit");
            WriteASentence(ConsoleColor.Cyan, "There are no stable quantity of ships,it can either 3,nor 5.But not more than 7");
            WriteASentence(ConsoleColor.Cyan, "First,who destroyed all of ships - wins!");
            Console.WriteLine();
            WriteASentence(ConsoleColor.Cyan, "Type 'start' to start the game");
        }
        public static void WriteASentence(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
    #endregion
    #region Logic

    #region AttackLogic
    class Logic
    {
        public static void wasHitFromPlayer()
        {
            int playerAttackY = PlayerInteraction.attackHeight;
            int playerAttackX = PlayerInteraction.attackWidth;

            for (int i = 0; i < ShipGenerationAI.ships.Length; i++)
            {
                Ship tempShip = ShipGenerationAI.ships[i];
                if (playerAttackY == tempShip.height[0] && playerAttackX == tempShip.width)
                {
                    ShipGenerationAI.ships[i].state = ShipState.destroyed;
                    GridGenerationAI.Shoot(tempShip.height[0], tempShip.width, '#');
                    UI.WriteASentence(ConsoleColor.Cyan, "You ve hit!");
                    return;
                }
            }
            GridGenerationAI.Shoot(playerAttackY, playerAttackX, '^');
            UI.WriteASentence(ConsoleColor.Cyan, "You havent hit");


        }
        public static void wasHittedFromAI()
        {
            int AIAttackY = AIInteraction.attackHeight;
            int AIAttackX = AIInteraction.attackWidth;

            for (int i = 0; i < ShipGeneration.ships.Length; i++)
            {
                Ship tempShip = ShipGeneration.ships[i];
                if (AIAttackY == tempShip.height[0] && AIAttackX == tempShip.width)
                {
                    ShipGeneration.ships[i].state = ShipState.destroyed;
                    GridGeneration.Shoot(AIAttackY, AIAttackX, '#');
                    UI.WriteASentence(ConsoleColor.Cyan, "Enemy hit!");
                    return;
                }
            }
            GridGeneration.Shoot(AIAttackY, AIAttackX, '^');
            UI.WriteASentence(ConsoleColor.Cyan, "Enemy havent hit");
        }
    }
    #endregion
    #region GameEnd
    class EndOfGame
    {
        public static void IfPlayerWon()
        {
            for (int i = 0; i < ShipGenerationAI.ships.Length; i++)
            {
                if (ShipGenerationAI.ships[i].state == ShipState.alive)
                {
                    return;
                }
            }
            End("You");
        }
        public static void IfAIWon()
        {
            for (int i = 0; i < ShipGeneration.ships.Length; i++)
            {
                if (ShipGeneration.ships[i].state == ShipState.alive)
                {
                    return;
                }
            }
            End("Enemy");
        }
        private static  void End(string winner)
        {
            UI.WriteASentence(ConsoleColor.Cyan, winner + " won the game!");
            UI.WriteASentence(ConsoleColor.Cyan,"Congratulations!");
            GameCycle.isgameEnded = true;
        }
    }
    #endregion
    #region GameCycle
    class GameCycle
    {
        public static bool isgameEnded;
        private static int counter;
        private static  void PlayerStart()
        {
            GridGeneration.GenerateField();
            ShipGeneration.GenerateShip(1,7);
        }
        private static void AIStart()
        {
            GridGenerationAI.GenerateFieldAI();
            ShipGenerationAI.GenerateShip(1, 7);
        }
        private static void Menu()
        {
            UI.GameSettings();
            string start = Console.ReadLine();
        }
        public static void StartGame()
        {
            Menu();
            PlayerStart();
            AIStart();
            GridGeneration.UpdateField();
            GridGenerationAI.UpdateFieldAIAndDontShowShips();
        }
        public static void GameProcess()
        {
            while (!isGameEnded( ref isgameEnded))
            {
                Cycle();
            }
        }
        private static void Cycle()
        {
            if(counter == 0)
            {
                PlayerCycle();
                counter++;
            }
            else
            {
                AICycle();
                counter--;
            }
        }
        private static void PlayerCycle()
        {
            PlayerInteraction.Attack();
            Logic.wasHitFromPlayer();
            EndOfGame.IfPlayerWon();
            
        }
        private static void AICycle()
        {
            AIInteraction.Attack();
            Logic.wasHittedFromAI();
            EndOfGame.IfAIWon();
        }
       public static bool isGameEnded( ref bool _isGameEnded)
        {
            return _isGameEnded;
        }
    }
    #endregion

    #endregion
    class Game
    {  
        static void Main(string[] args)
        {
           GameCycle.StartGame();
           GameCycle.GameProcess();
        }
    }
}
