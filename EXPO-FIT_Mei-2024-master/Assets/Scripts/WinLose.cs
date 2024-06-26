using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinLose : MonoBehaviour
{
    public Image player1ResultImage; // Text to display Player 1's result (Winner/Draw/Lose)
    public Image player2ResultImage; // Text to display Player 2's result (Winner/Draw/Lose)

    public Image rubahJantan, rubahBetina;

    public Sprite jantanAngry, jantanHappy;
    public Sprite betinaAngry, betinaHappy;

    public Sprite win, lose;

    // Function to set the result text for Player 1
    public void SetPlayer1Result(GameResult result)
    {
        switch (result)
        {
            case GameResult.Winner:
                player1ResultImage.sprite = win;
                rubahJantan.sprite = jantanHappy;
                break;
            case GameResult.Draw:
                
                break;
            case GameResult.Lose:
                player1ResultImage.sprite = lose;
                rubahJantan.sprite= jantanAngry;
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
                player2ResultImage.sprite = win;
                rubahBetina.sprite = betinaHappy;
                break;
            case GameResult.Draw:
                
                break;
            case GameResult.Lose:
                player2ResultImage.sprite = lose;
                rubahBetina.sprite = betinaAngry;
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
