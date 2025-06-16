using Godot;
using System;
using System.Linq;

namespace UnicornGame
{
	public partial class HotKey : Node
	{
		[Export] private string _hotKeyActionName = ""; // Action name for the hotkey, set in the editor
		private Label _hotKeyLabel;
		private Button _hotKeyButton;
		public override void _Ready()
		{
			SetProcessUnhandledInput(false); // Disable unhandled input processing
			SetActionKeyName(); // Set the action key name on ready

		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		private void SetActionKeyName()
		{
			_hotKeyLabel = GetNode<Label>("HBoxContainer/HotKeyLabel");
			_hotKeyLabel.Text = "Unassigned"; // Default text for unassigned hotkey

			_hotKeyButton = GetNode<Button>("HBoxContainer/Button");
			_hotKeyButton.Text = "Unassigned"; // Default text for unassigned hotkey

			InputEventKey keyEvent = InputMap.ActionGetEvents(_hotKeyActionName).OfType<InputEventKey>().FirstOrDefault();

			string displayKey;

			if (keyEvent != null)
			{
				displayKey = keyEvent.Keycode.ToString();
			}
			else if (keyEvent.PhysicalKeycode != Key.None)
			{
				displayKey = keyEvent.PhysicalKeycode.ToString();
			}
			else
			{
				displayKey = "Unassigned";
			}
			_hotKeyLabel.Text = _hotKeyActionName;
			_hotKeyButton.Text = displayKey;
		}
	}
}