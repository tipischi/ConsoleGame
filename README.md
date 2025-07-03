### Console Warrior - A Turn-Based Combat Adventure

This is an engaging turn-based console game written in C#.  
The entire game runs inside a single `Program.cs` file with rich gameplay features.

### 🎯 Goal
Battle through waves of enemies, level up your character, and see how long you can survive!

### ✨ Features
- **🗡️ Turn-based Combat**: Strategic combat with Attack, Defend, and Heal actions
- **⚡ Special Attacks**: Powerful abilities with limited charges
- **📈 Leveling System**: Gain experience and level up to increase your maximum HP
- **👺 Multiple Enemy Types**: Fight Goblins, Orcs, Trolls, and Dragons with unique behaviors
- **🎨 Rich UI**: Colorful interface with health bars, emojis, and ASCII art
- **📊 Game Statistics**: Track your progress with detailed stats
- **🔄 Difficulty Scaling**: Enemies become stronger as you progress
- **🛡️ Strategic Gameplay**: Each enemy type has different combat patterns

### 🎮 How to Play
- **[A]** Attack - Deal damage to enemies
- **[D]** Defend - Reduce incoming damage  
- **[H]** Heal - Restore your health
- **[S]** Special Attack - Devastating ability (limited charges)
- **[Q]** Quit - Exit with your final stats

### 🏗️ Building & Running
```bash
dotnet build
dotnet run --project ConsoleApp1
```

Built using .NET 8.0 with JetBrains Rider.

### 🎯 Game Mechanics
- **Level Up**: Gain 20 EXP per enemy defeated, level up every 20 EXP × current level
- **Special Charges**: Earn 1 charge per enemy defeated (max 3)
- **Enemy Types**:
  - 👺 **Goblins**: Aggressive attackers (60% attack rate)
  - 🧌 **Orcs**: Balanced fighters (33% each action)
  - 👹 **Trolls**: Defensive tanks (40% defend, 30% heal)
  - 🐉 **Dragons**: Magical beings (40% attack, 40% heal)
