using CardGame.Client;
using Godot;

namespace CardGame
{
    public class Main : Node
    {
        [Signal]
        public delegate void GameBegun();

        [Signal]
        public delegate void RoomsUpdated();

        private Room _room1;
        [Export] private bool _room1IsVisible;
        private Room _room2;
        [Export] private bool _room2IsVisible;
        private int _rooms;
        private int _roomUpdates;


        public override void _Ready()
        {
            GetTree().Connect("node_added", this, nameof(OnNodeAdded));
        }

        public void OnNodeAdded(Node node)
        {
            switch (node)
            {
                case Room room:
                {
                    _rooms++;
                    room.Connect(nameof(Room.Updated), this, nameof(OnRoomUpdated));
                    // ReSharper disable once ConvertIfStatementToSwitchStatement
                    if (_rooms == 1) _room1 = room;
                    if (_rooms == 2) _room2 = room;
                    bool visible = _rooms == 1 ? _room1IsVisible : _room2IsVisible;

                    room.GetNode<Spatial>("Room").Visible = visible;
                    room.GetNode<Control>("Room/GUI").Visible = visible;
 
                    if (_rooms != 2) return;
                    EmitSignal(nameof(GameBegun));
                    break;
                }
            }
        }

        public override void _Input(InputEvent gameEvent)
        {
            if (gameEvent is not InputEventKey {Pressed: true} key) return;
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch ((KeyList) key.Scancode)
            {
                case KeyList.S:
                {
                    SetVisibility(_room1);
                    SetVisibility(_room2);
                    break;
                }
            }
        }

        private static void SetVisibility(Room room)
        {
            room.GetNode<Spatial>("Room").Visible = !room.GetNode<Spatial>("Room").Visible;
            room.GetNode<Control>("Room/GUI").Visible = !room.GetNode<Control>("Room/GUI").Visible;
        }

        public void OnRoomUpdated(States states)
        {
            _roomUpdates++;
            if (_roomUpdates != 2) return;
            _roomUpdates = 0;
            EmitSignal(nameof(RoomsUpdated));
        }
    }
}