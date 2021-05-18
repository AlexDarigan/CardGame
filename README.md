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
    - [X] Add Deploy Function
    - [X] Add DeclareAttack
    - [X] Add DeclareDirectAttack
    - [X] Add SetFaceDown Function
    - [X] Add Disqualify Function
- _Tests_
    - [X] A Creature cannot attack the turn it is played
    - [X] A Creature can attack after the turn it has been played
    - [X] A Creature can only be played during its owners turn

## [ ] Battle System

    // We have a simple battle system implemented already
    // We should probably focus on making a minimal GUI right now
    // Also I wonder what happens if we treat networking as a singleton?..
    // ..(so we don't have to worry about client-side pathing)

## [ ] Primitive GUI
## [ ] Battle System
## [ ] Link System
    
    

