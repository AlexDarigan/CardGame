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
    - [X] Add Room
- _Tests_
    - [X] Is the Server the Server?
    - [X] Is the server connected?
    - [X] Is the client a client?
    - [X] Is the client connected?
    - [X] When the first clients is there one player on the server?
    - [X] When the second client joins is does the game start?
    - [X] When the game starts do both clients setup local rooms?
    - [X] The Client Rooms share the same name
  
## [ ] Base Game Loop
- _Features_
    - [X] Add Card
    - [X] Add Card Data
    - [X] Add Card Register
    - [X] Add DeckList
    - [X] Add Player Zones
    - [X] Add Game Start Function 
    - [X] Add Load Deck Function
    - [X] Add Draw Function
- _Tests_
    - [X] Are Player deck contents equal to deckList contents (in setCodes)
    - [X] Are Player decks reduced in size when we draw?
## [ ] Victory Conditions
- _Features_
    - [X] Add Player States
    - [X] Add End Turn Function
    - [X] Add ServerSide Win/Lose Function
- _Tests_
    - [X] When a player tries to draw a card with 0 cards, is the game over?
    - [X] When a player loses does the other player win?
    - [X] When a game ends are the players informed?
    - [X] A Player is disqualified when
        - [X] They try to end their turn during their opponents turn
        - [X] They try to end their turn when in a non-idle state

## [ ] Player Actions
- _Features_
    - [ ] Add Support Card Data
    - [ ] Add Additional Player States
    - [X] Add Deploy Function
    - [ ] Add DeclareAttack
    - [ ] Add DeclareDirectAttack
    - [ ] Add SetFaceDown Function
    - [ ] Add Activate Support Card
    - [X] Add Disqualify Function
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

    
    

