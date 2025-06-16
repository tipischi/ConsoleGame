using System;

class Program
{
    static void Main()
    {
        float playerHp = 100;
        float enemyHp = 100;
        float damagePlayer = 0;
        float damageEnemy = 0;
        float roundNumber = 0;
        Console.WriteLine("Round " + roundNumber + ":");
        
        damagePlayer = float.Parse(Console.ReadLine());

        playerHp -= damagePlayer;
        
        Console.WriteLine(playerHp);
    }
}