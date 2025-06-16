using System;

class Program
{
    static void Main()
    {
        float playerhp = 100;
        float enemyhp = 100;
        float damageplayer = 0;
        float damageenemy = 0;
        Console.WriteLine("Damage Player:");
        
        damageplayer = float.Parse(Console.ReadLine());

        playerhp -= damageplayer;
        
        Console.WriteLine(playerhp);
    }
}