# GMTK-Game-Jam-2022
Buff Frog Games
## Game Mechanics MVP brief
Player:
   * Can move right and left
   * Must reach the end of the level to win
   * Has HP
   * Takes damage if hitting an enemy
   * Player can instantly die by hitting traps or out of bounds area
   * Camera follows player

Dice system
   * Player have a dice slot queue
      - Min 1 dice. Decided at start of level
   * A dice is usable once it has finished rolling and has a set value
  (*) We can have different dice types (d6, d4, d20..)
   * Player has ability cards
      - Min 1, max 4. Decided at start of level.
   * Using an ability will consume the dice at the top of the queue
      - This will cause the last slot to be empty
   * When a slot is empty, a dice roll happens
      - The dice roll takes some time
         = This time needs to be adjusted per level
        (=) This time can be dynamic (debuffs, traps, powerups)
   * Abilities have individual cooldows (depending on animation, etc..) 
   * The effects of the ability are affected by the number of the dice used

Abilities:
   * Jump
      - Dice multiplies jump distance
   * Dash
      - Dice multiplies distance
      - Player is invulnerable
   * Attack (melee or ranged)
      - Dice multiplies damage
      - Direction player faces (left/right)
   (*) Special ability

Levels:
   * Platform
      (-) Moving platforms
   * Kill area

Enemies:
   * Hitting an enemy will damage the player
   * Enemies have HP
      (-) Healthbars
   * Patterns:
      - Simple patrol (sideways until hitting a wall, the turn around)
      - Follow player
         = Flying?

Power ups:
   (*) Extra dice
   (*) Extra slot
   (*) Extra health point