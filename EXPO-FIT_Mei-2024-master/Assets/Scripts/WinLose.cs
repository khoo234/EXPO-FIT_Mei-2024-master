using UnityEngine;
using TMPro;

public class WinLose : MonoBehaviour
{
    public TMP_Text player1ResultText; // Text to display Player 1's result (Winner/Draw/Lose)
    public TMP_Text player2ResultText; // Text to display Player 2's result (Winner/Draw/Lose)

    // Function to set the result text for Player 1
    public void SetPlayer1Result(GameResult result)
    {
        switch (result)
        {
            case GameResult.Winner:
                player1ResultText.SetText("Winner");
                break;
            case GameResult.Draw:
                player1ResultText.SetText("Draw");
                break;
            case GameResult.Lose:
                player1ResultText.SetText("Lose");
                break;
            default:
                break;
        }
    }

    // Function to set the result text for Player 2
    public void SetPlayer2Result(GameResult result)
    {
        switch (result)
        {
            case GameResult.Winner:
                player2ResultText.SetText("Winner");
                break;
            case GameResult.Draw:
                player2ResultText.SetText("Draw");
                break;
            case GameResult.Lose:
                player2ResultText.SetText("Lose");
                break;
            default:
                break;
        }
    }
}

public enum GameResult
{
    Winner,
    Draw,
    Lose
}
