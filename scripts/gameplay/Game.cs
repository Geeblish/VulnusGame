using Godot;
using System;

public class Game : Spatial
{
	public static BeatmapSet LoadedMap;
	public static BeatmapData LoadedMapData;

	public static Score Score;

	public GameCamera Camera;
	public Spatial Cursor;
	public Spatial GhostCursor;

	public NoteManager NoteManager;
	public SyncManager SyncManager;

	public bool Ended;

	public override void _Ready()
	{
		Camera = GetNode<GameCamera>("Camera");
		Cursor = GetNode<Spatial>("Cursor");
		GhostCursor = GetNode<Spatial>("GhostCursor");

		NoteManager = GetNode<NoteManager>("NoteManager");
		SyncManager = GetNode<SyncManager>("SyncManager");

		Score = new Score();

		Ended = false;

		Camera.Cursor = Cursor;
		Camera.GhostCursor = GhostCursor;

		if (LoadedMap == null || LoadedMapData == null)
		{
			Global.Instance.GotoScene("res://scenes/MainMenu.tscn");
			return;
		}

		SyncManager.SetStream(LoadedMap.LoadAudio());
		SyncManager.Connect("Ended", this, nameof(GameEnded));

		NoteManager.Connect("NoteHit", this, nameof(OnNoteHit));
		NoteManager.Connect("NoteMiss", this, nameof(OnNoteMiss));
	}
	public override void _PhysicsProcess(float delta)
	{
		if (Input.IsActionJustPressed("skip") && SyncManager.CanSkip())
			SyncManager.AttemptSkip();
	}
	public void OnNoteHit(Note note)
	{
		Score.Points += 25 * Score.Multiplier;
		Score.Miniplier += 1;
		if (Score.Miniplier >= 8)
		{
			Score.Miniplier = 0;
			Score.Multiplier = Mathf.Min(8, Score.Multiplier + 1);
		}
		Score.Combo += 1;
		if (Score.Combo > Score.HighestCombo)
			Score.HighestCombo = Score.Combo;
		Score.Total += 1;
	}
	public void OnNoteMiss(Note note)
	{
		Score.Miniplier = 0;
		Score.Multiplier = Mathf.Max(1, Score.Multiplier - 1);
		Score.Combo = 0;
		Score.Misses += 1;
		Score.Total += 1;
	}
	public void GameEnded()
	{
		Ended = true;
		Global.Instance.GotoScene("res://scenes/MainMenu.tscn");
	}
}
