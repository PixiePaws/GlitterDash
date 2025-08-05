using Godot;
using System;

namespace UnicornGame
{
	public partial class AudioSlider : Node
	{
		// Called when the node enters the scene tree for the first time.

		[Export] public string SliderLabelText = "Volume";
		private Slider _volumeSlider;

		private Label _audioNameLabel;
		private Label _audioNumberLabel;
		public override void _Ready()
		{
			_audioNameLabel = GetNode<Label>("HBoxContainer/AudioNameLabel");

			_audioNameLabel.Text = SliderLabelText;

			_audioNumberLabel = GetNode<Label>("HBoxContainer/AudioNumberLabel");

			_volumeSlider = GetNode<Slider>("HBoxContainer/HSlider");

			if (_volumeSlider != null)
			{
				_volumeSlider.Value = Mathf.DbToLinear(AudioManager.Instance._bgMusic.VolumeDb);
				_volumeSlider.ValueChanged += OnVolumeChanged;
			}
			else
			{
				GD.Print("_volumSlider Not FOund");
			}
		}
		private void OnVolumeChanged(double value)
		{
			float db = Mathf.LinearToDb((float)value);
			AudioManager audio = AudioManager.Instance;
			audio._bgMusic.VolumeDb = db;

			UpdateAudioNumberLabel(value);
		}
		private void UpdateAudioNumberLabel(double value)
		{
			int percentage = Mathf.RoundToInt((float)(value * 100));
			_audioNumberLabel.Text = $"{percentage}";
		}
	}
}
