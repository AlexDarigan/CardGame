using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CardGame.Client
{
    public static class Library
    {
        public static readonly ReadOnlyDictionary<SetCodes, CardData> Cards;

        static Library()
        {
            Dictionary<SetCodes, CardData> cardData = new();
            foreach (SetCodes setCode in Enum.GetValues(typeof(SetCodes)))
            {
                MemberInfo[] memberInfo = setCode.GetType().GetMember(Enum.GetName(setCode.GetType(), setCode) ?? throw new InvalidOperationException());
                CardResourceAttribute attribute = (CardResourceAttribute) memberInfo[0].GetCustomAttribute(typeof(CardResourceAttribute), false);
                attribute.CardData.SetCode = setCode;
                cardData[setCode] = attribute.CardData;
            }

            Cards = new ReadOnlyDictionary<SetCodes, CardData>(cardData);
        }
    }
    

    public class CardResourceAttribute : System.Attribute
    {
        public CardData CardData { get; }
        public CardResourceAttribute(string filePath)
        {
            CardData = Godot.GD.Load<CardData>($"res://Client/Library/Cards/{filePath}.tres");
        }
    }
}