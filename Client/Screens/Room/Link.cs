using Godot;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CardGame.Client
{
    [UsedImplicitly]
    public class Link : VBoxContainer
    {
        private Stack<Linked> Linked { get; } = new();
        
        public void Activate(Card card)
        {
            // Do we put the animation here?
            Linked linked = new Linked(card.Art);
            Linked.Push(linked);
            AddChild(linked);
        }

        public void Resolve()
        {
            // We're returning this but we don't really need to use it for anything
            Linked linked = Linked.Pop();
            RemoveChild(linked);
        }
    }

    public class Linked: TextureRect
    {
        public Linked(Texture cardArt)
        {
            RectMinSize = new Vector2(100, 150);
            Expand = true;
            StretchMode = StretchModeEnum.Scale;
            Texture = cardArt;
        }
    }

}

