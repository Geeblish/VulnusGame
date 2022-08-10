using Godot;
using System;

public class Options : View
{
	private bool active => IsActive;
	private Tween tween;
	private bool opening = false;
	public override void _Ready()
	{
		base._Ready();
		tween = GetNode<Tween>("Tween");
		var topbar = GetNode<Control>("Topbar");
		var closeBtn = topbar.GetNode<Button>("Close");
		closeBtn.Connect("pressed", this, nameof(SetActive), new Godot.Collections.Array(false));
	}
	public override void _PhysicsProcess(float delta)
	{
		if (Input.IsActionJustPressed("options"))
		{
			SetActive(!active);
		}
	}
	public override void OnShow()
	{
		if (this.Visible)
			return;
		opening = true;
		this.Visible = true;
		tween.InterpolateProperty(this, "modulate:a", 0, 1, 0.15f, Tween.TransitionType.Sine);
		tween.InterpolateProperty(this, "rect_scale", new Vector2(0.8f, 0.8f), new Vector2(1, 1), 0.2f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		tween.Start();
	}
	public override void OnHide()
	{
		if (!this.Visible)
			return;
		opening = false;
		tween.InterpolateProperty(this, "modulate:a", 1, 0, 0.15f, Tween.TransitionType.Sine);
		tween.InterpolateProperty(this, "rect_scale", new Vector2(1, 1), new Vector2(0.9f, 0.9f), 0.2f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		tween.Start();
	}
}
