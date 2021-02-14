![Tests](https://github.com/AlexDarigan/CardGame/workflows/Tests/badge.svg)


# Card Game

## Commit Style Guide


- **Add** [ _Asset_ ] [ _Class_ ] [ _Feature_ ] [ _Test_ ]
- **Remove** [ _Asset_ ] [ _Class_ ] [ _Feature_ ] [ _Test_ ]
- **Create** [ _Namespace_ ]  [ _Directory_ ]
- **Delete** [ _Namespace_ ]  [ _Directory_ ]
- **Refactor** [ _Class_ ] [ _Feature_ ] [ _Namespace_ ] [ _Directory_ ]
- **Fix** [ _Bug_ ]

 
# Milestones
## Base Game

## [ ] Matchmaking
- _Features_
    - [X] Add Server
    - [X] Add Client
    - [X] Add Player
    - [X] Add Player Queue
- _Tests_
    - [X] Is the Server the Server?
    - [X] Is the server connected?
    - [X] Is the client a client?
    - [X] Is the client connected?
    - [X] When the first clients is there one player on the server?
    - [ ] When the second client joins is does the game start?
    - [ ] When the game starts do both clients setup local rooms?

## [ ] Base Game Loop
- _Features_
    - [ ] Add Card
    - [ ] Add Card Data
    - [ ] Add Card Register
    - [ ] Add Decklist
    - [ ] Add Player Zones
    - [ ] Add Game Start Function 
    - [ ] Add Load Deck Function
    - [ ] Add Draw Function
- _Tests_
    - [ ] Are Player deck contents equal to decklist contents (in setcodes)
    - [ ] Are Player decks equal in size to decklist?
    - [ ] Is Card Register count equal to sum of both players decks?
    - [ ] Are our decks reduced in size when we draw?
## [ ] Victory Conditions
- _Features_
    - [ ] Add Player States
    - [ ] Add End Turn Function
    - [ ] Add ServerSide Win/Lose Function
    - [ ] Add ClientSide Win/Lose Function
    - [ ] Add Game Cleanup
- _Tests_
    - [ ] When a player tries to draw a card with 0 cards, do they lose?
    - [ ] When a player loses does the other player win?
    - [ ] When a game ends are the players informed?

## [ ] Player Actions
- _Features_
    - [ ] Add Support Card Data
    - [ ] Add Additional Player States
    - [ ] Add Deploy Function
    - [ ] Add DeclareAttack
    - [ ] Add DeclareDirectAttack
    - [ ] Add SetFaceDown Function
    - [ ] Add Activate Support Card
    - [ ] Add Disqualify Function
- _Tests_
    - [ ] A Creature cannot attack the turn it is played
    - [ ] A Support cannot be activated the turn it is set
    - [ ] A Creature can attack after the turn it has been played
    - [ ] A Support can be activated during either player's turn
    - [ ] A Creature can only be played during its owners turn
    - [ ] A Creature can only be set during its owners turn

## [ ] Battle System

    TODO

## Future Milestones

    These are significant in size and are too far in the future to accurately talk about immediatly.

    - Link System
    - GUI
    - Custom Rules

    
    

