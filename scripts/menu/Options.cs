using Godot;
using System;

public class Options : View
{
	public bool CanOpen = true;
	private bool moving = false;
	public override void _Ready()
	{
		base._Ready();
		var topbar = GetNode<Control>("Topbar");
		var closeBtn = topbar.GetNode<Button>("Close");
		closeBtn.Connect("pressed", this, nameof(SetActive), new Godot.Collections.Array(false));
	}
	public override void _PhysicsProcess(float delta)
	{
		if (CanOpen && Input.IsActionJustPressed("options"))
		{
			SetActive(!IsActive);
		}
		if (!IsActive)
			return;
		bool rankable = true;
		if (Settings.ApproachRate > 100f)
			rankable = false;
		if (Settings.ApproachDistance > 100f)
			rankable = false;
		if (Settings.ApproachTime > 2f)
			rankable = false;
		GetNode<Label>("RankStatus").Text = rankable ? "These settings are rankable." : "These settings are not rankable.";
	}
	public override async void OnShow()
	{
		if (moving || IsActive)
			return;
		moving = true;
		this.Visible = true;
		ViewTween.InterpolateProperty(this, "modulate:a", 0, 1, 0.15f, Tween.TransitionType.Sine);
		ViewTween.InterpolateProperty(this, "rect_scale", new Vector2(0.8f, 0.8f), new Vector2(1, 1), 0.2f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		ViewTween.Start();
		await ToSignal(ViewTween, "tween_all_completed");
		IsActive = true;
		moving = false;
	}
	public override async void OnHide()
	{
		if (moving || !IsActive)
			return;
		moving = true;
		ViewTween.InterpolateProperty(this, "modulate:a", 1, 0, 0.15f, Tween.TransitionType.Sine);
		ViewTween.InterpolateProperty(this, "rect_scale", new Vector2(1, 1), new Vector2(0.9f, 0.9f), 0.2f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		ViewTween.Start();
		await ToSignal(ViewTween, "tween_all_completed");
		IsActive = false;
		this.Visible = false;
		moving = false;
	}
}
