using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public partial class UiManager : Control
{
    [Signal]
    public delegate void StartGameEventHandler();

    [Export]
    public int MaxLeaderboardEntries = 10;

    private List<int> _highScores = new();

    public override void _Ready()
    {
        // Load leaderboard from disk when the game starts
        loadLeaderboard();

        // Populate UI leaderboard list with saved scores
        populateLeaderboard(GetNode<Control>("Leaderboard"));
    }

    public override void _Process(double delta) { }

    // Updates score label during gameplay
    public void _on_game_manager_score_updated(int score)
    {
        GetNode<Label>("ScoreLabel").Text = $"Score: {score}";
    }

    // Displays Game Over UI and updates leaderboard
    public void _on_game_manager_gameover(int score)
    {
        // Show game over panel
        GetNode<Control>("GameOverPanel").Show();
        GetNode<Label>("GameOverPanel/GameoverVbox/ScoreLabel").Text = $"Score: {score}";

        // Show leaderboard panel
        Control leaderboard = GetNode<Control>("Leaderboard");
        leaderboard.Show();

        // Add current score and sort top entries
        _highScores.Add(score);
        _highScores = _highScores
            .OrderByDescending(s => s)
            .Take(MaxLeaderboardEntries)
            .ToList();

        populateLeaderboard(leaderboard);
        saveLeaderboard();
    }

    // Refreshes the leaderboard UI with top scores
    private void populateLeaderboard(Control leaderboard)
    {
        // Clear existing leaderboard UI entries
        foreach (var child in GetNode("Leaderboard/VBoxContainer").GetChildren())
        {
            child.QueueFree();
        }

        // Display scores in order
        for (int i = 0; i < _highScores.Count; i++)
        {
            var entry = new RichTextLabel
            {
                BbcodeEnabled = true,
                FitContent = true,
                Text = i == 0
                    ? $"[b]{i + 1}. {_highScores[i]}"   // Bold top score
                    : $"{i + 1}. {_highScores[i]}"
            };

            leaderboard.GetNode("VBoxContainer").AddChild(entry);
        }
    }

    // Called when the Play Again button is pressed
    public void _on_play_again_button_down()
    {
        EmitSignal(SignalName.StartGame);
    }

    // Resets UI when a new game starts
    public void _on_game_manager_start_game()
    {
        GetNode<Control>("GameOverPanel").Hide();
        GetNode<Control>("StartGamePanel").Hide();
        GetNode<Control>("Leaderboard").Hide();
    }

    // Saves leaderboard scores to disk in JSON format
    private void saveLeaderboard()
    {
        using var file = FileAccess.Open("user://highscores.save", FileAccess.ModeFlags.Write);

        try
        {
            string jsonString = JsonSerializer.Serialize(_highScores);
            file.StoreString(jsonString);
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error saving leaderboard: {e.Message}");
        }
    }

    // Loads leaderboard scores from disk
    private void loadLeaderboard()
    {
        try
        {
            using var file = FileAccess.Open("user://highscores.save", FileAccess.ModeFlags.Read);
            string jsonString = file.GetAsText();
            var loadedScores = JsonSerializer.Deserialize<List<int>>(jsonString);
            _highScores = loadedScores ?? new List<int>();
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error loading leaderboard: {e.Message}");
            _highScores = new List<int>();
        }
    }
}
