using Godot;
using System;
using System.Collections.ObjectModel;
using CardGame;
using CardGame.Client;
using Godot.Collections;

public class InputController : Control
{
	public bool drawing = false;
	public Line2D line;
	private Vector2 start;
	private delegate States Play(Card card);
	private ReadOnlyDictionary<CardStates, Play> Plays { get; }
	public event Declaration Declare;
	public States State { get; set; } = States.Passive;
	private Card Attacker { get; set; }

	public InputController()
	{
		Plays = new ReadOnlyDictionary<CardStates, Play>(new System.Collections.Generic.Dictionary<CardStates, Play> {
			{CardStates.Deploy, Deploy}, {CardStates.SetFaceDown, SetFaceDown}, {CardStates.Activate, Activate}, 
			{CardStates.AttackPlayer, AttackPlayer}, {CardStates.AttackUnit, AttackUnit}, {CardStates.None, None}});
	}
	
	public void OnRivalAvatarPressed()
	{
		// Note: When testing two scenes, have to make sure focus isn't being stolen by the other copy
		if (Attacker is not null && Attacker.CardState.Get() == CardStates.AttackPlayer) { CommitAttack(); }
	}

	public void OnAttackDeclared()
	{
		drawing = true;
		start = GetGlobalMousePosition();
		line = new Line2D();
	}

	public void OnAttackCancelled()
	{
		drawing = false;
		line.ClearPoints();
	}

	public override void _Process(float delta)
	{
		Update();
	}

	public override void _Draw()
	{
		if (!drawing) return;
		DrawLine(start, GetGlobalMousePosition(), new Godot.Color(255, 0, 0), 10, true);
	}
	
	public void OnCardPressed(Card pressed)
    {
        if (pressed == Attacker) { CancelAttack(); }
        else if (Attacker is not null && Attacker.CardState.Get() == CardStates.AttackUnit) { CommitAttack(pressed); }
        else if(State != States.Passive) { Plays[pressed.CardState.Get()](pressed); }
    }
    
    private States Deploy(Card card)
    {
        Declare?.Invoke(CommandId.Deploy, card.Id.Get());
        return States.Passive;
    }

    private States AttackUnit(Card card)
    {
        // Could we make this async?
        Attacker = card;
        OnAttackDeclared(); //?.Invoke();
        return State; // This seems wrong? Targeting maybe?
    }

    private States AttackPlayer(Card card)
    {
        Attacker = card;
        OnAttackDeclared(); //?.Invoke();
        return State;
    }

    private States SetFaceDown(Card card)
    {
        Declare?.Invoke(CommandId.SetFaceDown, card.Id.Get());
        return State;
    }

    private States Activate(Card card) { return State; }

    private void CommitAttack(Card card)
    {
        // Is Defender ValId.Get()
        OnAttackCancelled(); //.Invoke(); // Committed?
        Declare?.Invoke(CommandId.DeclareAttack, Attacker.Id.Get(), card.Id.Get());
        Attacker = null;
    }
    
    private void CommitAttack()
    {
        OnAttackCancelled(); //?.Invoke(); // Committed?
        Declare?.Invoke(CommandId.DeclareDirectAttack, Attacker.Id.Get());
        Attacker = null;
    }

    private void CancelAttack()
    {
        Attacker = null;
        OnAttackCancelled(); //?.Invoke();)
    }

    private States None(Card card) { return State; }
    
    public void PassPlay() { }

    public void EndTurn() { if (State == States.IdleTurnPlayer) { Declare?.Invoke(CommandId.EndTurn); } }
}
