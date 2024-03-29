using Godot;
using System;

public class RenderSettings : Control
{
	public override void _Ready()
	{
		GetNode<CheckButton>("VSync").Disabled = !OS.HasFeature("pc");
		GetNode<CheckButton>("VSync").Pressed = Settings.VSync;
		GetNode<CheckButton>("VSync").Connect("pressed", this, nameof(OnButtonPressed), new Godot.Collections.Array(0));
		GetNode<CheckButton>("Debanding").Pressed = Settings.Debanding;
		GetNode<CheckButton>("Debanding").Connect("pressed", this, nameof(OnButtonPressed), new Godot.Collections.Array(1));
		GetNode<DecimalInput>("FPS").SetValue(Settings.FPSLimit);
		GetNode<DecimalInput>("FPS").Connect("ValueChanged", this, nameof(OnValueChanged));
		GetNode<Slider>("RenderScale").SetValue(Settings.RenderScale * 100f);
		GetNode<Slider>("RenderScale").Connect("ValueChanged", this, nameof(OnSliderChanged));
	}
	public void OnValueChanged(float value)
	{
		Settings.FPSLimit = Mathf.RoundToInt(value);
		Settings.UpdateSettings();
	}
	public void OnSliderChanged(float value)
	{
		Settings.RenderScale = value / 100f;
		Settings.UpdateSettings();
	}
	public void OnButtonPressed(int btn)
	{
		if (btn == 0)
			Settings.VSync = GetNode<CheckButton>("VSync").Pressed;
		else
			Settings.Debanding = GetNode<CheckButton>("Debanding").Pressed;
		Settings.UpdateSettings();
	}
}
