# Project Proposal

"Meta-troidvania"  
*Name subject to change*

Tyler Allen  
Brigham Campbell  
Toby Reid

## Game Overview

> Describe your proposed game at a high level.  
> Explain the core fantasy or experience, the basic premise or story (if any), who the target players are, and what a typical play session will feel like.
> This section should give a reader who has never seen your project a clear picture of what you intend to build.

Our game is effectively a "metroidvania"-style game, but instead of gathering new abilities as you go, you gain the basic functionalities that one might normally expect from a side-scrolling platformer.

For example, the ability to open the pause menu, or to jump, would be abilities you gain along the way; at first, you can only run in one direction.  
Other ideas include unlocking your health bar (so the game would crash without it, if you take damage) and other such essentials.

### The Story

The player begins in a tutorial-like level, where they learn to move, jump, climb, attack and defend, collect coins, and so forth; however, when they try to touch the level goal, the game exhibits a BSOD and crashes.
When they reopen the game, they discover that all functionality has been lost, and the game has been corrupted.
They must now restore the game to its original glory.

## Core Mechanics, Features, and Tasks

> Break your project down into concrete gameplay features and implementation tasks.  
> Identify the main systems and components of your game (for example: player controls, enemy AI, level structure, UI, audio, game loop, save/load) and list the major tasks you will need to complete for each.
> Aim for enough detail that someone could roughly estimate the amount of work from this section.

### Core Mechanics

* Movement & Platforming
  * The player should be able to move left & right
  * The player should be able to jump & fall
  * This may also later include the ability to climb/descend ladders
  * This may also later include the ability to swim in water
* Enemies & Combat
  * The player should be able to defeat enemies in some way
    * This may take the form of either jumping down, or a melee/ranged attack, or both
  * The player should be able to take damage from enemies
    * This includes an on-screen health bar
* Collectibles & Goals
  * The player should be able to collect coins
    * This includes an on-screen coin count indicator
  * The player should be able to collect unlockables
    * e.g., the ability to jump should be unlockable in-game in some manner
  * This may also later include other collectibles, such as better music
* Menus & Settings
  * The player should be able to save & load their game
    * This can be very simple, to the tune of just remembering the current room and other variables
  * The player should be able to exit the game
  * The player should be able to adjust simple settings, such as volume
    * Fullscreen mode and other visual settings should not be necessary; we can just keep it fullscreen (to enhance the BSOD)

### Main Features

*All* of the above mechanics should be toggleable based on certain global variables; i.e., if that variable does not match the expected value(s), the feature is nonexistent or may exhibit undesired behavior (for the user).

Beyond that, the game will feature only two levels: the tutorial and the "main" area.
We may split the main area into multiple separate scenes, but it will all be one cohesive region.

The main menu and in-game menu can be kept simplistic, reminiscient of old-school start menus, with only the bare minimum visible.

Visuals and audio can be retrieved from the interwebs.  
Some manual modification may be desirable for (for example) creating annoying 1-measure music loops that can later be expanded, but this should not be a primary focus.

Saving and loading will be possible, by a simple approach.
Essentially, we need only record information about the player character and load that in along with its room.

### Primary Tasks

* Implement core movement mechanics
  * Player can move left/right
  * Player can jump/fall
  * Player can climb/descend ladders
* Implement GUI features
  * Health bar or hearts
  * Coin counts
  * Oxygen meter (if including water & swimming)
* Implement menus
  * Main menu (complete with its scene)
  * In-game menu
    * Including settings submenu
* Implement tutorial level
  * Featuring *all* known game mechanics; make it like a tech demo
  * If we have the time, also implement the BSOD crash screen
* Implement the main level
  * This will be a large undertaking
  * Fortunately, we can build it from the middle out, so if we run out of time on certain features, we can just stop expanding the main level

## Scope and Complexity

> Explain why this project is appropriately sized for the final project. 
> Discuss the planned scope, level of polish, and technical difficulty. 
> Your proposal should make a clear argument that the project will require approximately 30–35 hours of work per person on your team over the duration of the project. 
> If you are planning stretch goals, note which features are core (must-have) and which are optional (nice-to-have) if time permits.

Our focus on this project is to implement core logic and gameplay mechanics. 
Graphics, animations, music, and map design will be present but less polished; there will be enough to present the game we made.
We anticipate that our game design will allow easily changing the scope of our project if it ends up being easier or more difficult than anticipated. 

### Must-haves
* Player character must be able to unlock and use basic mechanics (move, jump, attack)
* Unlock other gameplay mechanics (menus, health bar, etc.)
* A large-enough map to hold all abilities / items
  * Map will be made of different rooms loaded when a player enters
* At least a few enemy types with animations and ai
* At least one soundtrack
* Sound effects for major actions
* Saving/Loading file

### Nice-to-haves
* Optional collectables (unlock new animations, better sounds, etc.)
* Additional player actions (such as climb)
* Final boss/challenge
* Un-lockable visual changes (maybe needing to recover RGB channels)
* Map size varying on difficulty of other tasks
* Number of enemies depending on difficulty

## New Knowledge and Open Questions

> Reflect on what you will need to learn in order to complete this project successfully.  
> List the new topics, tools, engine features, libraries, or design techniques you plan to research, and identify any risks or open questions you have (for example: performance concerns, art pipeline issues, complex mechanics you are not yet sure how to implement).
> This section should make it clear that you have thought ahead about your learning plan and possible blockers.

One major element of research is to investigate other games, especially those with somewhat similar mechanics, to determine industry standard for implementation of various mechanics.

If we manage to have enough time, we will also need to investigate implementation of visual shaders in Godot for our implementations of potentially toggleable Red/Green/Blue channels.
These would be relatively simple shaders, but it would give insight into how they work generally.

Sound design will be interesting, especially if we implement spatial sound or design our own.
Procedurally-generated sound may also be an interesting approach for the early parts, so the "corrupted" game feels more spontaneous and arbitrary.

Saving by brute force is relatively simple, but we also hope to implement additional complexity such as encrypting save files to prevent the user from modifying game files.
We have some experience with basic bitwise XOR encryption, but one potential deep dive would be into various game-saving encryption algorithms.
