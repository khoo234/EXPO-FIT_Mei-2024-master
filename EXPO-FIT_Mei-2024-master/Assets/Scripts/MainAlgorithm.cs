using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainAlgorithm : MonoBehaviour
{
    public int totalQuestion;           // Variable for max question
    public float delayPerAnswer;        // Variable for delay between answer that show
    

    [SerializeField] int question;      // Variable for shown question
    [SerializeField] int answer;        // Variable for shown answer

    [SerializeField] int player1Points, player2Points;      // Variable for points of both players
    
    bool isPlayer1CanAnswer, isPlayer2CanAnswer;            // Variable that use for ban player after give wrong respond

    Coroutine waitAnswer = null;                            // Variable for IEnumerator WaitForAnswer(). Use this so we can use StopCoroutine

    int maxWrongAnswer;                                     // Variable for maximum wrong answer every wave

    public TMP_Text questionUI, answerUI, player1PtsUI, player2PtsUI, isPlayer1RightUI, isPlayer2RightUI;   // Variable for UI Text

    public CheckScoreAkhir checkScoreAkhir;



    // Start is called before the first frame update
    void Start()
    {
        ShowQuestion();         // Show 1st Question

        // Make sure both player can respond question
        isPlayer1CanAnswer = true;
        isPlayer2CanAnswer = true;
    }

    // Update is called every frame(?)
    void Update()
    {
        // Get Input from both player
        PlayerOne();
        PlayerTwo();

        // Set text for the UI variable
        questionUI.SetText(question.ToString());            // Set text for question
        answerUI.SetText(answer.ToString());                // Set text for answer
        player1PtsUI.SetText(player1Points.ToString());     // Set text for 1st player points
        player2PtsUI.SetText(player2Points.ToString());     // Set text for 2nd player poimts
        
    }

    // Function for input and show question
    void ShowQuestion()
    {
        question = Random.Range(1, totalQuestion);      // Create question from the range between 0 and maximum questions
        maxWrongAnswer = Random.Range(0, 4);            // Create maximum wrong answer every wave
        
        waitAnswer = StartCoroutine(WaitForAnswer());   // Execute and input value of time between answers
    }

    // Function for input and show answer
    void ShowAnswer(){
        answer = Random.Range(0, totalQuestion);        // Create random answer that ranged between 0 and maximum questions

        // Set the UI of right or wrong is empty
        isPlayer1RightUI.SetText("");
        isPlayer2RightUI.SetText("");

        // If answer is more than the maximum wrong, make the answer == question
        if(maxWrongAnswer <= 0)
        {
            answer = question;
        }
    }

    // Function to show answer, and add delay between answers
    IEnumerator WaitForAnswer()
    {
        ShowAnswer();   // Execute the function for show answer

        yield return new WaitForSeconds(delayPerAnswer);        // delay between answers

        // When delay is finish, 
        // Make sure player can answer
        isPlayer1CanAnswer = true;
        isPlayer2CanAnswer = true;

        // And run another delay
        waitAnswer = StartCoroutine(WaitForAnswer());
    }

    // Function for get input from player one
    void PlayerOne()
    {
        // Only run if player 1 isn't give wrong respond for that question
        if(isPlayer1CanAnswer)
        {
            // THIS MUST CHANGE, because the goals is respond with our HEAD, not the keyboard

            // Execute if player 1 think answer == question
            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                GuestAnswer(1, false);
            }

            // Execute if player 1 think answer != question
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                GuestAnswer(1, true);
            }
        }
    }

    // Function for get input from player two
    void PlayerTwo()
    {
        // Only run if player 2 isn't give wrong respond for that question
        if(isPlayer2CanAnswer)
        {
            // THIS MUST CHANGE, because the goals is respond with our HEAD, not the keyboard

            // Execute if player 2 think answer == question
            if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                GuestAnswer(2, false);
            }

            // Execute if player 2 think answer != question
            if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                GuestAnswer(2, true);
            }   
        }     
    }

    // Function for check respond from player
    void GuestAnswer(int player, bool IsRight)
    {
        // Execute if the respond is right
        if((question == answer && IsRight) || (question != answer && !IsRight))
        {
            // Execute the function when player is right
            PlayerIsRight(player);
           
            // Execute If question == answer
            if(question == answer && IsRight)
            {
                NextQuestion();     // Go to next question
            }
            // Execute if quesstion != answer
            else
            {
                maxWrongAnswer -= 1;                                // Decrease the chance that next question, question != answer
                StopCoroutine(waitAnswer);                          // Stop all coroutine, do this to avoid bug
                waitAnswer = StartCoroutine(WaitForAnswer());       // Start new coroutine
            }

            // Make sure both player can give respond again
            isPlayer1CanAnswer = true;
            isPlayer2CanAnswer = true;
            
        }
        // Execute if player give wrong respond
        else
        {
            PlayerIsFalse(player);
        }
    }

    // Execute if player give right respond
    // Execute if player give right respond
    void PlayerIsRight(int player)
    {
        // If Player is right, he/she will get points

        // Execute if the right player is player 1
        if (player == 1)
        {
            player1Points += 1;             // Give point for player 1
            isPlayer1RightUI.SetText("V");
            checkScoreAkhir.UpdatePlayer1Score(player1Points);

            // Set Player 1 as winner if the score is higher
            if (player1Points > player2Points)
            {
                FindObjectOfType<WinLose>().SetPlayer1Result(GameResult.Winner);
                FindObjectOfType<WinLose>().SetPlayer2Result(GameResult.Lose);
            }
            // Set Player 1 and Player 2 as draw if the scores are equal
            else if (player1Points == player2Points)
            {
                FindObjectOfType<WinLose>().SetPlayer1Result(GameResult.Draw);
                FindObjectOfType<WinLose>().SetPlayer2Result(GameResult.Draw);
            }
        }
        // Execute if the right player is player 2
        else if (player == 2)
        {
            player2Points += 1;             // Give point for player 2
            isPlayer2RightUI.SetText("V");
            checkScoreAkhir.UpdatePlayer2Score(player2Points);

            // Set Player 2 as winner if the score is higher
            if (player2Points > player1Points)
            {
                FindObjectOfType<WinLose>().SetPlayer1Result(GameResult.Lose);
                FindObjectOfType<WinLose>().SetPlayer2Result(GameResult.Winner);
            }
            // Set Player 1 and Player 2 as draw if the scores are equal
            else if (player1Points == player2Points)
            {
                FindObjectOfType<WinLose>().SetPlayer1Result(GameResult.Draw);
                FindObjectOfType<WinLose>().SetPlayer2Result(GameResult.Draw);
            }
        }
    }


    // Execute if player give wrong respond
    void PlayerIsFalse(int player)
    {
        // If Player is wrong, he/she will get ban for give respond to the same question

        // Execute if the wrong one is player 1
        if(player == 1)
        {
            isPlayer1CanAnswer = false;     // Ban player 1
            isPlayer1RightUI.SetText("X");
        }
        // Execute if the wrong one is player 2
        else if(player == 2)
        {
            isPlayer2CanAnswer = false;     // Ban player 2
            isPlayer2RightUI.SetText("X");
        }
    }

    
    
    // Run the next question
    void NextQuestion()
    {
        StopCoroutine(waitAnswer);      // Stop coroutine, do this to avoid bug
        ShowQuestion();                 // Run new question
    }





}
