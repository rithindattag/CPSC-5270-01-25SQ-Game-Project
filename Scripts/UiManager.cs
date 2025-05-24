using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;



public partial class UiManager : Control
{
	[Signal]
	public delegate void StartGameEventHandler();
	private List<int> _highScores = new List<int>();
	[Export]
	public int MaxLeaderboardEntries = 10;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		loadLeaderboard();
		Control leaderboard = GetNode<Control>("Leaderboard");
		populateLeaderboard(leaderboard);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_game_manager_score_updated(int score) {
		GetNode<Label>("ScoreLabel").Text = $"Score: {score}";
	}

	public void _on_game_manager_gameover(int score) {
		GetNode<Control>("GameOverPanel").Show();
		GetNode<Label>("GameOverPanel/GameoverVbox/ScoreLabel").Text = $"Score: {score}";
		Control leaderboard = GetNode<Control>("Leaderboard");
		leaderboard.Show();
		_highScores.Add(score);
		_highScores = _highScores.OrderByDescending(s => s).ToList();
		if(_highScores.Count > MaxLeaderboardEntries) {
			_highScores.RemoveRange(MaxLeaderboardEntries, _highScores.Count - MaxLeaderboardEntries);
		}

		populateLeaderboard(leaderboard);
		saveLeaderboard();
	}	

	private void populateLeaderboard(Control leaderboard) {

		foreach (var item in GetNode("Leaderboard/VBoxContainer").GetChildren()) {
			item.QueueFree();
		}

		for(int i = 0; i < _highScores.Count; i++) {
			var entry = new RichTextLabel();
			entry.BbcodeEnabled = true;
			entry.FitContent = true;

			if(i == 0) {
				entry.Text = $"[b]{i + 1}. {_highScores[i]}";
			} else {
				entry.Text = $"{i + 1}. {_highScores[i]}";
			}
			leaderboard.GetNode("VBoxContainer").AddChild(entry);
		}
	}
	

	public void _on_play_again_button_down() {
		EmitSignal(SignalName.StartGame);
	}

	public void _on_game_manager_start_game() {
		GetNode<Control>("GameOverPanel").Hide();
		GetNode<Control>("StartGamePanel").Hide();
		GetNode<Control>("Leaderboard").Hide();
	}

	private void saveLeaderboard() {
		using var file = FileAccess.Open("user://highscores.save", FileAccess.ModeFlags.Write);

		try {
			string jsonString = JsonSerializer.Serialize(_highScores);
			file.StoreString(jsonString);
		} catch(Exception e) {
			GD.PrintErr($"error in saving leaderboard: {e.Message}");
		}
	}

	private void loadLeaderboard() {
		using var file = FileAccess.Open("user://highscores.save", FileAccess.ModeFlags.Read);

		try {
			string jsonString = file.GetAsText();
			var loadedScores = JsonSerializer.Deserialize<List<int>>(jsonString);
			_highScores = loadedScores ?? new List<int>();
		} catch(Exception e) {
			GD.PrintErr($"error in saving leaderboard: {e.Message}");
			_highScores = new List<int>();
		}
	}
}
