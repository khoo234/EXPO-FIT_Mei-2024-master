using UnityEngine;
using TMPro;

public class CheckScoreAkhir : MonoBehaviour
{
    public TMP_Text player1ScoreText;  // Text to display Player 1's score
    public TMP_Text player2ScoreText;  // Text to display Player 2's score

    public GameObject WinLosePanel;
    public GameObject MainPanel;

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

    public void CheckPointCondition(int pointPlayer1, int pointPlayer2, GameObject AlgorithmGO)
    {
        if(pointPlayer1 > pointPlayer2)
        {
            AlgorithmGO.GetComponent<WinLose>().SetPlayer1Result(GameResult.Winner);
            AlgorithmGO.GetComponent<WinLose>().SetPlayer2Result(GameResult.Lose);
            
        }else if(pointPlayer1 < pointPlayer2)
        {
            AlgorithmGO.GetComponent<WinLose>().SetPlayer1Result(GameResult.Lose);
            AlgorithmGO.GetComponent<WinLose>().SetPlayer2Result(GameResult.Winner);

        }else if(pointPlayer1 == pointPlayer2)
        {
            AlgorithmGO.GetComponent<WinLose>().SetPlayer1Result(GameResult.Draw);
            AlgorithmGO.GetComponent<WinLose>().SetPlayer2Result(GameResult.Draw);
        }

        WinLosePanel.SetActive(true);
        MainPanel.SetActive(false);

        player1ScoreText.SetText(pointPlayer1.ToString());
        player2ScoreText.SetText(pointPlayer2.ToString());

    }
}
