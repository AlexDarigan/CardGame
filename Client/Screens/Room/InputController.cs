using System;
using Godot;
using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace CardGame.Client
{
	[UsedImplicitly]
	public class InputController : Control
	{
		private bool _drawing = false;
		private Line2D _line;
		private Vector2 _start;

		private delegate States Play(Card card);
		private ReadOnlyDictionary<CardStates, Play> Plays { get; }
		public event Declaration Declare;
		public States State { get; set; } = States.Passive;
		private Card Attacker { get; set; }
		public Action<Card> Activated;

		public InputController()
		{
			Plays = new ReadOnlyDictionary<CardStates, Play>(new System.Collections.Generic.Dictionary<CardStates, Play>
			{
				{CardStates.Deploy, Deploy}, {CardStates.SetFaceDown, SetFaceDown}, {CardStates.Activate, Activate},
				{CardStates.AttackPlayer, AttackPlayer}, {CardStates.AttackUnit, AttackUnit}, {CardStates.None, None}
			});
		}

		public void OnRivalAvatarPressed()
		{
			if (Attacker is not null && Attacker.CardState == CardStates.AttackPlayer) { CommitAttack(); }
		}

		private void StartDrawingLine()
		{
			_drawing = true;
			_start = GetGlobalMousePosition();
			_line = new Line2D();
		}

		private void StopDrawingLine()
		{
			_drawing = false;
			_line.ClearPoints();
		}

		public override void _Process(float delta) { Update(); }

		public override void _Draw()
		{
			if (!_drawing) return;
			DrawLine(_start, GetGlobalMousePosition(), new Godot.Color(255, 0, 0), 10, true);
		}

		public void OnCardPressed(Card pressed)
		{
		
			if (pressed == Attacker)
			{
				CancelAttack();
			}
			else if (Attacker is not null && Attacker.CardState == CardStates.AttackUnit)
			{
				CommitAttack(pressed);
			}
			else if (State != States.Passive)
			{
				Plays[pressed.CardState](pressed);
			}
		}

		private States Deploy(Card card)
		{
			Declare?.Invoke(CommandId.Deploy, card.Id);
			return States.Passive;
		}

		private States AttackUnit(Card card)
		{
			Attacker = card;
			StartDrawingLine();
			return State;
		}

		private States AttackPlayer(Card card)
		{
			Attacker = card;
			StartDrawingLine();
			return State;
		}

		private States SetFaceDown(Card card)
		{
			Declare?.Invoke(CommandId.SetFaceDown, card.Id);
			return State;
		}

		private States Activate(Card card)
		{
		
			card.RotationDegrees = new Vector3(card.RotationDegrees.x, card.RotationDegrees.y, 0);
			Activated(card);
			Declare?.Invoke(CommandId.Activate, card.Id);
			return State; // The client can make assumptions about our state so we can trigger things immediatly
		}

		private void CommitAttack(Card card)
		{
			StopDrawingLine();
			Declare?.Invoke(CommandId.DeclareAttack, Attacker.Id, card.Id);
			Attacker = null;
		}

		private void CommitAttack()
		{
			StopDrawingLine();
			Declare?.Invoke(CommandId.DeclareDirectAttack, Attacker.Id);
			Attacker = null;
		}

		private void CancelAttack()
		{
			Attacker = null;
			StopDrawingLine();
		}

		private States None(Card card) { return State; }

		public void OnPassPlayPressed()
		{
			switch (State)
			{
				case States.Active:
					Declare?.Invoke(CommandId.PassPlay);
					break;
				case States.IdleTurnPlayer:
					Declare?.Invoke(CommandId.EndTurn);
					break;
			}
		}
	}
}