using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RubahScript : MonoBehaviour
{
    public Image head;

    [Space]

    public Sprite idle, happy, angry, sad;
    
    
    public void ChangeFace(string state)
    {
        switch (state)
        {
            case "idle":
                head.sprite = idle;
                break;
            case "happy":
                head.sprite = happy;
                break;
            case "angry":
                head.sprite = angry;
                break;
            case "sad":
                head.sprite = sad;
                break;
            default:
                break;
        }
    }
}