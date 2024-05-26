using UnityEngine;
using TMPro;

public class CheckScoreAkhir : MonoBehaviour
{
    public TMP_Text player1ScoreText;  // Text to display Player 1's score
    public TMP_Text player2ScoreText;  // Text to display Player 2's score

    // Method to update Player 1's score
    public void UpdatePlayer1Score(int score)
    {
        player1ScoreText.SetText(score.ToString());
    }

    // Method to update Player 2's score
    public void UpdatePlayer2Score(int score)
    {
        player2ScoreText.SetText(score.ToString());
    }
}
