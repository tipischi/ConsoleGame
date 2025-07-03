using System;
using System.Threading;

class Program
{
    public enum EnemyType
    {
        Goblin,
        Orc,
        Troll,
        Dragon
    }

    public static (string name, string emoji, float hpMultiplier, string behavior) GetEnemyInfo(EnemyType type)
    {
        return type switch
        {
            EnemyType.Goblin => ("Goblin", "👺", 0.8f, "aggressive"),
            EnemyType.Orc => ("Orc", "🧌", 1.0f, "balanced"),
            EnemyType.Troll => ("Troll", "👹", 1.3f, "defensive"),
            EnemyType.Dragon => ("Dragon", "🐉", 1.8f, "magical"),
            _ => ("Unknown", "❓", 1.0f, "balanced")
        };
    }
    static void Main()
    {
        ShowGameIntro();
        PlayGame();
    }

    static void ShowGameIntro()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║          🗡️  CONSOLE WARRIOR  🛡️          ║");
        Console.WriteLine("║                                          ║");
        Console.WriteLine("║      A Turn-Based Combat Adventure      ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nYou are a brave warrior facing dangerous enemies!");
        Console.WriteLine("Use your skills wisely to survive and become legendary!");
        Console.WriteLine("\nPress any key to begin your adventure...");
        Console.ReadKey(true);
    }

    static void PlayGame()
    {
        float playerHp = 100;
        float playerMaxHp = 100;
        float enemyHp = 100;
        float enemyMaxHp = 100;
        int roundNumber = 0;
        int playerLevel = 1;
        int playerExp = 0;
        int enemiesDefeated = 0;
        int specialAttackCharges = 1; // Start with 1 special attack
        EnemyType currentEnemyType = EnemyType.Goblin;
        Random random = new Random();
        
        Console.ForegroundColor = ConsoleColor.White;
        
        while (playerHp > 0)
        {
            roundNumber++;
            
            // Check if enemy is defeated, spawn a new one
            if (enemyHp <= 0)
            {
                enemiesDefeated++;
                playerExp += 10;
                specialAttackCharges = Math.Min(specialAttackCharges + 1, 3); // Gain special charge, max 3
                
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("🎉 Victory! Enemy defeated!");
                Console.WriteLine($"You gained 10 experience points!");
                Console.WriteLine($"⚡ Special attack charge gained! ({specialAttackCharges}/3)");
                
                // Level up check
                if (playerExp >= playerLevel * 20)
                {
                    playerLevel++;
                    playerMaxHp += 20;
                    playerHp = Math.Min(playerHp + 20, playerMaxHp); // Heal on level up
                    Console.WriteLine($"🆙 LEVEL UP! You are now level {playerLevel}!");
                    Console.WriteLine($"Max HP increased to {playerMaxHp}!");
                }
                
                Console.WriteLine("\nPress any key to face the next enemy...");
                Console.ReadKey(true);
                
                // Choose enemy type based on progress
                currentEnemyType = enemiesDefeated switch
                {
                    < 3 => EnemyType.Goblin,
                    < 6 => EnemyType.Orc,
                    < 10 => EnemyType.Troll,
                    _ => EnemyType.Dragon
                };
                
                var enemyInfo = GetEnemyInfo(currentEnemyType);
                
                // Spawn new enemy with scaling difficulty and type multiplier
                enemyMaxHp = (80 + (enemiesDefeated * 15)) * enemyInfo.hpMultiplier;
                enemyHp = enemyMaxHp;
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n⚔️  A {enemyInfo.name} {enemyInfo.emoji} appears! (Level {enemiesDefeated + 1})");
                Thread.Sleep(1500);
            }
            
            Console.Clear();
            GameDrawFrame(roundNumber, playerHp, playerMaxHp, enemyHp, enemyMaxHp, playerLevel, playerExp, enemiesDefeated, specialAttackCharges, currentEnemyType);
            
            // Get player action with validation
            string playerAction = GetPlayerAction(specialAttackCharges);
            if (playerAction == "quit")
            {
                ShowGameStats(roundNumber, enemiesDefeated, playerLevel);
                return;
            }
            
            // Consume special attack charge if used
            if (playerAction == "s")
            {
                specialAttackCharges--;
            }
            
            // Get enemy action based on type
            string enemyAction = GetEnemyAction(random, currentEnemyType);
            
            Console.Clear();
            GameDrawFrame(roundNumber, playerHp, playerMaxHp, enemyHp, enemyMaxHp, playerLevel, playerExp, enemiesDefeated, specialAttackCharges, currentEnemyType);
            
            // Show actions
            ShowActionResults(playerAction, enemyAction);
            
            Thread.Sleep(2000);
            (playerHp, enemyHp) = GameProcessActions(playerHp, enemyHp, playerMaxHp, enemyMaxHp, playerAction, enemyAction);
            
            // Check if player died
            if (playerHp <= 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("💀 GAME OVER! You have been defeated...");
                ShowGameStats(roundNumber, enemiesDefeated, playerLevel);
                return;
            }
            
            Thread.Sleep(1000);
        }
    }

    static void GameDrawFrame(int roundNumber, float playerHp, float playerMaxHp, float enemyHp, float enemyMaxHp, int playerLevel, int playerExp, int enemiesDefeated, int specialCharges, EnemyType enemyType)
    {
        var enemyInfo = GetEnemyInfo(enemyType);
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"╔═══════════════ ROUND {roundNumber} ═══════════════╗");
        
        // Player stats
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("║ 🧙 PLAYER ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"HP: {playerHp:F0}/{playerMaxHp:F0} ");
        DrawHealthBar(playerHp, playerMaxHp, ConsoleColor.Green);
        Console.WriteLine();
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"║ 📊 Level: {playerLevel} | EXP: {playerExp}/{playerLevel * 20} | Defeated: {enemiesDefeated}");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($" | ⚡: {specialCharges}");
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("╠════════════════════════════════════════════╣");
        
        // Enemy stats
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"║ {enemyInfo.emoji} {enemyInfo.name.ToUpper()}  ");
        Console.Write($"HP: {enemyHp:F0}/{enemyMaxHp:F0} ");
        DrawHealthBar(enemyHp, enemyMaxHp, ConsoleColor.Red);
        Console.WriteLine();
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("╚════════════════════════════════════════════╝");
        Console.ForegroundColor = ConsoleColor.White;
    }

    static void DrawHealthBar(float currentHp, float maxHp, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        int barLength = 20;
        int filledLength = (int)((currentHp / maxHp) * barLength);
        
        Console.Write("[");
        for (int i = 0; i < barLength; i++)
        {
            if (i < filledLength)
                Console.Write("█");
            else
                Console.Write("░");
        }
        Console.Write("]");
    }

    static string GetPlayerAction(int specialCharges)
    {
        while (true)
        {
            Console.WriteLine("\n⚔️  Choose your action:");
            Console.WriteLine("  [A] 🗡️  Attack  - Deal damage to enemy");
            Console.WriteLine("  [D] 🛡️  Defend  - Reduce incoming damage");
            Console.WriteLine("  [H] ❤️  Heal    - Restore your health");
            
            if (specialCharges > 0)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"  [S] ⚡ Special - Devastating attack ({specialCharges} charges)");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  [S] ⚡ Special - No charges available");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.WriteLine("  [Q] 🚪 Quit    - Exit the game");
            Console.Write("\nEnter your choice: ");
            
            string? input = Console.ReadLine()?.ToLower().Trim();
            
            if (string.IsNullOrEmpty(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input! Please try again.");
                Console.ForegroundColor = ConsoleColor.White;
                continue;
            }
            
            switch (input)
            {
                case "a":
                case "attack":
                    return "a";
                case "d":
                case "defend":
                    return "d";
                case "h":
                case "heal":
                    return "h";
                case "s":
                case "special":
                    if (specialCharges > 0)
                        return "s";
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No special attack charges available!");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                case "q":
                case "quit":
                    return "quit";
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid action! Please choose A, D, H, S, or Q.");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }

    static string GetEnemyAction(Random random, EnemyType enemyType)
    {
        var enemyInfo = GetEnemyInfo(enemyType);
        
        return enemyInfo.behavior switch
        {
            "aggressive" => // Goblins prefer attacking
                random.Next(1, 11) switch
                {
                    <= 6 => "a", // 60% attack
                    <= 8 => "d", // 20% defend  
                    _ => "h"     // 20% heal
                },
            "defensive" => // Trolls prefer defending and healing
                random.Next(1, 11) switch
                {
                    <= 3 => "a", // 30% attack
                    <= 7 => "d", // 40% defend
                    _ => "h"     // 30% heal
                },
            "magical" => // Dragons use complex patterns
                random.Next(1, 11) switch
                {
                    <= 4 => "a", // 40% attack
                    <= 6 => "d", // 20% defend
                    _ => "h"     // 40% heal (dragons recover quickly)
                },
            _ => // Balanced behavior (Orcs)
                random.Next(1, 4) switch
                {
                    1 => "a",
                    2 => "d",
                    _ => "h"
                }
        };
    }

    static void ShowActionResults(string playerAction, string enemyAction)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("🧙 You are ");
        Console.WriteLine(playerAction switch
        {
            "a" => "⚔️  ATTACKING!",
            "d" => "🛡️  DEFENDING!",
            "h" => "❤️  HEALING!",
            "s" => "⚡ UNLEASHING SPECIAL ATTACK!",
            _ => "doing nothing..."
        });
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("👹 Enemy is ");
        Console.WriteLine(enemyAction switch
        {
            "a" => "⚔️  ATTACKING!",
            "d" => "🛡️  DEFENDING!",
            "h" => "❤️  HEALING!",
            _ => "doing nothing..."
        });
        
        Console.ForegroundColor = ConsoleColor.White;
    }

    static void ShowGameStats(int rounds, int enemiesDefeated, int playerLevel)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n╔═══════════════════════════════════════════╗");
        Console.WriteLine("║              GAME STATISTICS              ║");
        Console.WriteLine("╠═══════════════════════════════════════════╣");
        Console.WriteLine($"║ Rounds Survived: {rounds,-23} ║");
        Console.WriteLine($"║ Enemies Defeated: {enemiesDefeated,-22} ║");
        Console.WriteLine($"║ Final Level: {playerLevel,-27} ║");
        Console.WriteLine("╚═══════════════════════════════════════════╝");
        
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nThank you for playing Console Warrior!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
    }
        
    static (float playerHp, float enemyHp) GameProcessActions(float playerHp, float enemyHp, float playerMaxHp, float enemyMaxHp, string playerAction, string enemyAction)
    {
        float playerDamage = 0;
        float enemyDamage = 0;
        bool playerDefending = playerAction == "d";
        bool enemyDefending = enemyAction == "d";
        
        // Process player action
        switch (playerAction)
        {
            case "a": // Attack
                playerDamage = enemyDefending ? 5 : 15; // Reduced damage if enemy is defending
                break;
            case "s": // Special Attack
                playerDamage = enemyDefending ? 20 : 35; // Powerful attack, less affected by defense
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("💫 SPECIAL ATTACK! Lightning courses through your weapon!");
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case "d": // Defend - no immediate effect, reduces incoming damage
                break;
            case "h": // Heal
                playerHp = Math.Min(playerHp + 20, playerMaxHp);
                Console.WriteLine("💚 You heal for 20 HP!");
                break;
        }
        
        // Process enemy action
        switch (enemyAction)
        {
            case "a": // Attack
                enemyDamage = playerDefending ? 3 : 12; // Reduced damage if player is defending
                break;
            case "d": // Defend - no immediate effect, reduces incoming damage
                break;
            case "h": // Heal
                enemyHp = Math.Min(enemyHp + 15, enemyMaxHp);
                Console.WriteLine("🔴 Enemy heals for 15 HP!");
                break;
        }
        
        // Apply damage
        if (playerDamage > 0)
        {
            enemyHp -= playerDamage;
            if (playerAction == "s")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"⚡ Your special attack deals {playerDamage} devastating damage!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine($"⚔️  You deal {playerDamage} damage to the enemy!");
            }
        }
        
        if (enemyDamage > 0)
        {
            playerHp -= enemyDamage;
            Console.WriteLine($"💥 Enemy deals {enemyDamage} damage to you!");
        }
        
        // Show defend messages
        if (playerDefending && enemyAction == "a")
        {
            Console.WriteLine("🛡️  Your defense reduced the incoming damage!");
        }
        
        if (enemyDefending && playerAction == "a")
        {
            Console.WriteLine("🛡️  Enemy's defense reduced your damage!");
        }
        
        if (enemyDefending && playerAction == "s")
        {
            Console.WriteLine("🛡️  Enemy's defense slightly reduced your special attack!");
        }
        
        // Ensure HP doesn't go below 0
        playerHp = Math.Max(0, playerHp);
        enemyHp = Math.Max(0, enemyHp);
        
        return (playerHp, enemyHp);
    }
}