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
		private bool _buttonToggle = true;
		public override void _Ready()
		{
			SetActionKeyName(); // Set the action key name on ready

			_hotKeyButton.Pressed += OnButtonPressed;

		}
		public override void _Input(InputEvent @event)
		{
			if (!_buttonToggle && @event is InputEventKey keyEvent && keyEvent.Pressed)
			{
				RebindKey(keyEvent); // Rebind action to this key
				GD.Print("Pressed key: ", keyEvent.Keycode);
				_buttonToggle = true; // Reset toggle
				SetProcessInput(false); // Stop listening
				SetActionKeyName(); // Update UI
			}
		}

		private void SetActionKeyName()
		{
			_hotKeyLabel = GetNode<Label>("HBoxContainer/HotKeyLabel");
			_hotKeyLabel.Text = "Unassigned"; // Default text for unassigned hotkey

			_hotKeyButton = GetNode<Button>("HBoxContainer/Button");
			_hotKeyButton.Text = "Unassigned"; // Default text for unassigned hotkey

			InputEventKey keyEvent = InputMap.ActionGetEvents(_hotKeyActionName).OfType<InputEventKey>().FirstOrDefault(); // This needs "using System.Linq"

			string displayKey;

			if (keyEvent != null)
			{
				// If the key event has a physical keycode, use it; otherwise, use the keycode
				if (keyEvent.PhysicalKeycode != Key.None)
				{
					displayKey = keyEvent.PhysicalKeycode.ToString();
				}
				else
				{
					displayKey = keyEvent.Keycode.ToString();
				}
			}
			else
			{
				displayKey = "Unassigned";
			}
			_hotKeyLabel.Text = _hotKeyActionName;
			_hotKeyButton.Text = displayKey;
		}
		private void OnButtonPressed()
		{
			if (_buttonToggle)
			{
				_buttonToggle = false;
				_hotKeyButton.Text = "Press any key...";
				_hotKeyButton.ReleaseFocus();
				SetProcessInput(true);
			}
		}
		private void RebindKey(InputEventKey newKeyEvent) //This method rebinds the keys. if it finds a dublicate it also removes the old key.
		{
			string newKeyString = OS.GetKeycodeString(newKeyEvent.PhysicalKeycode);

			foreach (Node key in GetTree().GetNodesInGroup("hotKeyButton"))
			{
				if (key is HotKey otherKey)
				{
					string otherActionName = otherKey._hotKeyActionName;
					Godot.Collections.Array<InputEvent> currentKeyEvents = InputMap.ActionGetEvents(otherActionName);
					foreach (InputEvent inputEvent in currentKeyEvents)
					{
						if (inputEvent is InputEventKey keyEvent)
						{
							string currentKeyString = OS.GetKeycodeString(keyEvent.PhysicalKeycode);
							if (currentKeyString == newKeyString && otherActionName != _hotKeyActionName)
							{
								InputMap.ActionEraseEvent(otherActionName, keyEvent);
								GD.Print($"HotKey vaihdettu, {newKeyString} vaihdettiin {currentKeyString}");
							}
						}
					}
				}

			}

			InputMap.ActionEraseEvents(_hotKeyActionName);

			InputMap.ActionAddEvent(_hotKeyActionName, newKeyEvent);

			UpdateAllHotKeyUIs();
		}
		private void UpdateAllHotKeyUIs() // This updates the ui to make sure that the user can see that the keys have been changed if its a dublicate.
		{
			foreach (Node node in GetTree().GetNodesInGroup("hotKeyButton"))
			{
				if (node is HotKey hotkey)
				{
					hotkey.SetActionKeyName();
				}
			}
		}
	}
}