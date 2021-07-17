using System;
using Godot;

namespace CardGame.Client
{
	public delegate void Declare(Declaration declaration, params object[] args);
	
	public class Room : Node
	{
		public static Room Instance() => (Room) GD.Load<PackedScene>("res://Client/Screens/Room/Room.tscn").Instance();
		public InputController InputController { get; set; }
		private CommandQueue CommandQueue { get; set; }
		public Tween Effects { get; set; }
		public AudioStreamPlayer BGM { get; set; }
		public AudioStreamPlayer SFX { get; set; }
		public Participant Player { get; private set; }
		public Participant Rival { get; private set; }
		public Cards Cards { get; private set; }
		public Table Table { get; private set; }
		public Text Text { get; private set; }
		public Link Link { get; private set; }
		public Room() { }

		public override void _Ready()
		{
			InputController = GetNode<InputController>("InputController");
			CommandQueue = GetNode<CommandQueue>("CommandQueue");
			BGM = GetNode<AudioStreamPlayer>("BGM");
			SFX = GetNode<AudioStreamPlayer>("SFX");
			Player = GetNode<Participant>("Player");
			Rival = GetNode<Participant>("Rival");
			Effects = GetNode<Tween>("Effects");
			Table = GetNode<Table>("Table");
			Cards = GetNode<Cards>("Cards");
			Text = GetNode<Text>("Text");
			Link = GetNode<Link>("Link");
			
			Text.Id = CustomMultiplayer.GetNetworkUniqueId();
			Table.PassPlayPressed = InputController.OnPassPlayPressed;
			Rival.Avatar.Pressed += InputController.OnRivalAvatarPressed;
			InputController.Declare += (commandId, args) => { RpcId(1, Enum.GetName(commandId.GetType(), commandId), args); };
			Cards.InputController = InputController;
			RpcId(1, "OnClientReady");
		}


		[Puppet] private void Update() { CommandQueue.Execute(this); }
		[Puppet] private void Queue(CommandId commandId, object[] args) { CommandQueue.Enqueue(commandId, args); }
	}
}
