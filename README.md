# Dungeon Crawler

A simple turn-based dungeon crawler made in C# as part of an object-oriented programming assignment.

The game runs in the console where the player explores a dungeon, encounters enemies and fights to survive.

## Features

- A dungeon map loaded from a text file.
- Player movement using the arrow keys.
- Two enemy types: rats that move randomly and snakes that move away from the player when nearby.
- Turn-based combat using dice rolls for attack and defense.
- A limited field of view where previously discovered walls remain visible.
- A HUD displaying the player's health, movement count and recent combat events.

## Controls

| Key | Action |
|---|---|
| Arrow keys | Player movement |
| Esc | Exit game |

Moving into an enemy starts a fight. Both the player and enemies can attack and counterattack. The game ends when the player's health reaches zero.
