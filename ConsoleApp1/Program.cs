using System;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("setting up game");
        float playerHp = 100;
        float enemyHp = 100;
        float roundNumber = 0;
        string playerAction = "";
        
        ConsoleColor color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.White;
        
        while (true)
        {
            Console.Clear();
            roundNumber += 1;
            GameDrawFrame(roundNumber, playerHp, enemyHp);
            Console.WriteLine("Enter A for ATTACK, D for DEFEND and H for HEAL:");
            string enemyAction = "";
            Random state = new Random();
            int randomState = state.Next(1, 4);
            
            playerAction = Console.ReadLine();

            Console.Clear();
            
            GameDrawFrame(roundNumber, playerHp, enemyHp);
            
            if (playerAction == "a")
                Console.WriteLine("attacking");
            else if (playerAction == "d")
                Console.WriteLine("defending");
            else if (playerAction == "h")
                Console.WriteLine("healing");
            
            if (randomState == 1)
            {
                enemyAction = "a";
                Console.WriteLine("Enemy is Attacking");
            }
            else if (randomState == 2)
            {
                enemyAction = "d";
                Console.WriteLine("Enemy is Defending");
            }
            else if (randomState == 3)
            {
                enemyAction = "h";
                Console.WriteLine("Enemy is Healing");
            }

            Thread.Sleep(2000);
            (playerHp, enemyHp) = GameProcessActions(playerHp, enemyHp, playerAction, enemyAction);
        }
    }

    static void GameDrawFrame(float roundNumber, float playerHp, float enemyHp)
    {
        Console.WriteLine("Round " + roundNumber + ":");
        Console.Write("----------  PlayerHP = " + playerHp + "  ");
        Console.WriteLine("EnemyHP = " + enemyHp + "  ----------");
    }
        
    static (float playerHp, float enemyHp) GameProcessActions(float playerHp, float enemyHp, string playerAction, string enemyAction)
    {
        if (playerAction == "a")
        {
            if (enemyAction == "a")
            {
                playerHp -= 5;
                enemyHp -= 5;
            }

            if (enemyAction == "h")
            {
                enemyHp -= 10;
            }
            
        }
        
        if (playerAction == "d")
        {
            if (enemyAction == "h")
            {
                enemyHp += 10;
            }
        }
        
        if (playerAction == "h")
        {
            if (enemyAction == "a")
            {
                playerHp -= 10;
                
            }

            if (enemyAction == "h")
            {
                enemyHp += 10;
                playerHp += 10;
            }
            if (enemyAction == "d")
            {
                playerHp += 10;
            }
        }
        return (playerHp, enemyHp);
    }
}