using Godot;
using System;

namespace UnicornGame
{
	public partial class AudioSliderSFX : Node
	{
		// Called when the node enters the scene tree for the first time.

		[Export] public string SliderLabelText = "Volume";
		private Slider _volumeSlider;
		private Label _audioNameLabel;
		public override void _Ready()
		{
			_audioNameLabel = GetNode<Label>("HBoxContainer/AudioNameLabel");
			_audioNameLabel.Text = SliderLabelText;


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
		}
	}
}
