using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO.Ports;

public class MainAlgorithm : MonoBehaviour
{
    public int totalQuestion;           // Variable for max question
    public float delayPerAnswer = 3f;        // Variable for delay between answer that show

    public int maxPoint = 3;

    [SerializeField] bool Show_QA_text = false;
    

    [SerializeField] int question;      // Variable for shown question
    [SerializeField] int answer;        // Variable for shown answer

    [SerializeField] int player1Points, player2Points;      // Variable for points of both players
    
    bool isPlayer1CanAnswer, isPlayer2CanAnswer;            // Variable that use for ban player after give wrong respond

    Coroutine waitAnswer = null;                            // Variable for IEnumerator WaitForAnswer(). Use this so we can use StopCoroutine

    int maxWrongAnswer;                                     // Variable for maximum wrong answer every wave

    public TMP_Text questionUI, answerUI, player1PtsUI, player2PtsUI, isPlayer1RightUI, isPlayer2RightUI;   // Variable for UI Text

    public GameObject AlgorithmGO;

    public Sprite[] images;                 // Array of images

    public Image QuestionImage;          // Image Component for question
    public Image AnswerImage;           // Image Component for answer

    public GameObject rubahJantan, rubahBetina;

    int wrongPlayer = 0;

    [Space]
    public AudioSource rightSFX, wrongSFX;

    int rightPlayer;

    SerialPort serialPort = new SerialPort("COM7", 115200); // Adjust COM port as needed


    // Start is called before the first frame update
    void Start()
    {
        ShowQuestion();         // Show 1st Question

        // Make sure both player can respond question
        isPlayer1CanAnswer = true;
        isPlayer2CanAnswer = true;

        totalQuestion = images.Length;

        try
        {
            // Initialize and open the serial port
            serialPort = new SerialPort("COM7", 115200);
            serialPort.ReadTimeout = 500; // Set a timeout for reading
            serialPort.Open();
            Debug.Log("Serial port opened successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    // Update is called every frame(?)
    void Update()
    {
        // Get Input from both player
        PlayerOne();
        PlayerTwo();

        // Set text for the UI variable

        if(Show_QA_text)
        {
            questionUI.SetText(question.ToString());            // Set text for question
            answerUI.SetText(answer.ToString());                // Set text for answer
        }else{
            questionUI.SetText("");            // Set text for question
            answerUI.SetText("");                // Set text for answer
        }

        
        player1PtsUI.SetText(player1Points.ToString());     // Set text for 1st player points
        player2PtsUI.SetText(player2Points.ToString());     // Set text for 2nd player poimts
        
    }

    // Function for input and show question
    void ShowQuestion()
    {
        waitAnswer = StartCoroutine(WaitForAnswer());   // Execute and input value of time between answers
    }

    // Function for input and show answer
    void ShowAnswer(){
        // Make sure both player can give respond again
        isPlayer1CanAnswer = true;
        isPlayer2CanAnswer = true;

        if (rightPlayer != 1)
        {
            rubahJantan.GetComponent<RubahScript>().ChangeFace("idle");
        }else
        {
            rubahBetina.GetComponent<RubahScript>().ChangeFace("idle");
        }
        
        answer = Random.Range(0, totalQuestion);        // Create random answer that ranged between 0 and maximum questions
        wrongPlayer = 0;


        // Set the UI of right or wrong is empty
        isPlayer1RightUI.SetText("");
        isPlayer2RightUI.SetText("");

        // If answer is more than the maximum wrong, make the answer == question
        if(maxWrongAnswer <= 0)
        {
            answer = question;
        }

        AnswerImage.sprite = images[answer];
    }

    // Function to show answer, and add delay between answers
    IEnumerator WaitForAnswer()
    {
        if(answer == question && wrongPlayer >= 2)
        {
              NextQuestion();
        }
        

        yield return new WaitForSeconds(delayPerAnswer);        // delay between answers
        rubahJantan.GetComponent<RubahScript>().ChangeFace("idle");
        rubahBetina.GetComponent<RubahScript>().ChangeFace("idle");
                
        question = Random.Range(1, totalQuestion);      // Create question from the range between 0 and maximum questions
        maxWrongAnswer = Random.Range(0, 4);            // Create maximum wrong answer every wave
        
        QuestionImage.sprite = images[question];

        ShowAnswer();   // Execute the function for show answer
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
        if (isPlayer1CanAnswer && serialPort.IsOpen)
        {
            try
            {
                if (serialPort.BytesToRead > 0) // Check if there's data in the buffer
                {
                    string input = serialPort.ReadLine().Trim();
                    Debug.Log("Received input: " + input); // Should show "TEST" from the basic sketch

                    if (input == "NOD")
                    {
                        GuestAnswer(1, true);
                    }
                    else if (input == "SHAKE")
                    {
                        GuestAnswer(1, false);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Serial read error: " + ex.Message); // Capture errors
            }
        }
    }

    // Function for get input from player two
    void PlayerTwo()
    {
        // Only run if player 2 isn't give wrong respond for that question
        if (isPlayer2CanAnswer)
        {
            // THIS MUST CHANGE, because the goals is respond with our HEAD, not the keyboard

            // Execute if player 2 think answer == question
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                GuestAnswer(2, false);
            }

            // Execute if player 2 think answer != question
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
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
                if(waitAnswer != null)
                {
                    StopCoroutine(waitAnswer);                          // Stop all coroutine, do this to avoid bug
                }
                
                waitAnswer = StartCoroutine(WaitForAnswer());       // Start new coroutine
            }
        }
        // Execute if player give wrong respond
        else
        {
            PlayerIsFalse(player);
            wrongPlayer++;
        }
    }

    // Execute if player give right respond
    // Execute if player give right respond
    void PlayerIsRight(int player)
    {
        isPlayer1CanAnswer = false;
        isPlayer2CanAnswer = false;
        rightSFX.Play();
        // If Player is right, he/she will get points

        // Execute if the right player is player 1
        if (player == 1)
        {
            player1Points += 1;             // Give point for player 1
            isPlayer1RightUI.SetText("O");
            rubahJantan.GetComponent<RubahScript>().ChangeFace("happy");
            rightPlayer = 1;
        }
        // Execute if the right player is player 2
        else if (player == 2)
        {
            player2Points += 1;             // Give point for player 2
            isPlayer2RightUI.SetText("O");
            rubahBetina.GetComponent<RubahScript>().ChangeFace("happy");
            rightPlayer = 2;
        }

        if (player1Points == maxPoint || player2Points == maxPoint)
        {
            AlgorithmGO.GetComponent<CheckScoreAkhir>().CheckPointCondition(player1Points, player2Points, AlgorithmGO);
        }
    }


    // Execute if player give wrong respond
    void PlayerIsFalse(int player)
    {
        wrongSFX.Play();
        // If Player is wrong, he/she will get ban for give respond to the same question

        // Execute if the wrong one is player 1
        if(player == 1)
        {
            isPlayer1CanAnswer = false;     // Ban player 1
            isPlayer1RightUI.SetText("X");
            rubahJantan.GetComponent<RubahScript>().ChangeFace("sad");
        }
        // Execute if the wrong one is player 2
        else if(player == 2)
        {
            isPlayer2CanAnswer = false;     // Ban player 2
            isPlayer2RightUI.SetText("X");
            rubahBetina.GetComponent<RubahScript>().ChangeFace("sad");
        }
    }

    
    
    // Run the next question
    void NextQuestion()
    {
        if(waitAnswer != null)
        {
            StopCoroutine(waitAnswer);                          // Stop all coroutine, do this to avoid bug
        }
        ShowQuestion();                 // Run new question
    }

    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Arduino connected!");
        else
            Debug.Log("Failed to connect to Arduino.");
    }
}
