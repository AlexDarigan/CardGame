using System;
using System.Collections.Generic;
using Godot;

namespace CardGame.Server
{
    public class Card
    {
        public readonly int Id;
        public SetCodes SetCodes;
        public CardType CardType;
        public Faction Faction;
        public readonly Player Owner;
        public Player Controller;
        public string Title;
        public int Power;
        public bool IsReady = false;
        public Skill Skill;
        public IList<Card> Zone;

        public Card(int id, Player owner)
        {
            Id = id;
            Owner = owner;
            Controller = owner;
        }

        public void Activate()
        {
            Skill.Activate();
        }
    }
}