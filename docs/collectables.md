# Definition

For the scope of this file, a "collectable" is a unique item that the player picks up once. This may include abilities and other optional items such as soundtracks. This does not include "pickups" such as coins or life.

# Collectable list

Below is a list of collectables we *may* implement. The priority column distinguishes how essential the item is to our game, with 0 being the most essential and 2 being the least essential. I anticipate we should be able to implement all 0's and several 1's. We likely won't get to 2's, but they are there if we like the idea and want to promote it. This should help us prioritize our efforts and cut content. 

The essential filed indicates if the item is necessary to complete the game (if implemented), or an optional collectable. 


| Name                    | Priority | Required | Description                                                                             |
|-------------------------|----------|----------|-----------------------------------------------------------------------------------------|
| right movement          | 0        | Y        | The player can move right                                                               |
| jump                    | 0        | Y        | The player can jump                                                                     |
| attack                  | 0        | Y        | Allows the player to perform a melee attack                                             |
| health bar              | 0        | Y        | Allows the player to take damage, without this any damage will "crash" the game         |
| coin counter            | 0        | Y        | Allows the player to pick up coins, without this, touching a coin will "crash" the game |
| ladder interaction      | 0        | Y        | Allows the player to climb up ladders                                                   |
| drop                    | 0        | Y        | Allows the player to fall through semi-solid platforms                                  |
| enemy ai                | 1        | Y        | Enemies now move, without this some enemies may be blocking a route                     |
| water physics           | 1        | Y        | The player can safely touch water without crashing                                      |
| soundtracks (multiple)  | 1        | N        | Unlocks background music                                                                |
| menu                    | 1        | N        | Allows for opening the menu to pause, save and exit, etc.                               |
| animations (multiple)   | 2        | N        | Replaces static sprites with animated ones                                              |
| oxygen meter            | 2        | Y        | Additional requirement for water areas                                                  |
| RGB channels (multiple) | 2        | Y        | Players can see each channel, necessary to see some secrets                             |
| Dev Notes               | 2        | N        | Optional "lore" collectable                                                             |
| Language Pack           | 2        | Y        | Allows player to read essential text boxes                                              |
| Optimization            | 2        | Y        | Player can enter room with lots of particle effects, game runs above 10 fps             |

# Progression

Below is list of **required** items we intend to implement, in the intended collection order. This should be updated as we adjust the scope of our project

*note: this is currently a very loose order, map designers will be able to make a much more informed ordering*
*I added a few priority 1 options that I though we would be more likely to implement*

1. right movement
2. jump
3. enemy ai
4. attack
5. coin counter
6. water
7. health
8. drop
