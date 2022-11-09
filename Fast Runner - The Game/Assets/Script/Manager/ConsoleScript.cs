using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(ConsoleActionScript))]
public class ConsoleScript : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject consoleCanvas;
    [SerializeField] private TextMeshProUGUI consoleText;
    [SerializeField] private TextMeshProUGUI consoleArrow;

    [Header("Script")]
    [SerializeField] private KeyMapManagerScript keyMapManagerScript;
    private ConsoleActionScript consoleActionScript;

    [Header("Parameter")]
    [SerializeField] private bool isConsoleActive = false;
    
    private void Awake()
    {
        consoleCanvas.SetActive(isConsoleActive);
        consoleActionScript = GetComponent<ConsoleActionScript>();
    }

    private void Update()
    {
        List<string> value = new List<string>();

        value = keyMapManagerScript.GetAction("Console");

        if(isConsoleActive)
        {
            UpdateConsole(value);
        }
        else
        {
            CheckConsole(value);
        }

        consoleCanvas.SetActive(isConsoleActive);

        if(isConsoleActive == false)
        {
            consoleText.text = "";
            consoleArrow.color = new Color(255, 255, 255, 255);
        }
    }

    private void CheckConsole(List<string> command)
    {
        foreach(string cmd in command)
        {
            if(cmd == "Active")
            {
                isConsoleActive = true;
                Time.timeScale = 0f;
                break;
            }
        }
    }

    private void UpdateConsole(List<string> command)
    {
        bool isDone = false;
        
        foreach(string cmd in command)
        {
            if(cmd == "Active")
            {
                isConsoleActive = false;
                Time.timeScale = 1f;
                break;
            }
        }

        if(isConsoleActive)
        {
            foreach(string cmd in command)
            {
                if(cmd == "Enter")
                {
                    isDone = true;
                    break;
                }
            }
        }

        if(isDone)
        {
            CheckConsoleData();
        }
        else
        {
            foreach(string cmd in command)
            {
                if(cmd == "Backspace")
                {
                    consoleText.text = consoleText.text.Remove(consoleText.text.Length - 1);
                }
                else
                {
                    consoleText.text += cmd;
                }
            }
        }
    }

    private void CheckConsoleData()
    {
        string[] word = consoleText.text.Split(' ');

        consoleArrow.color = new Color(0, 255, 0, 255);

        if(word[0] == "RESETPOS")
        {
            consoleActionScript.ResetPos();
        }
        else if(word[0] == "MOVEPOS")
        {
            consoleActionScript.ChangeLocation(word[1]);
        }
        else if(word[0] == "MOVESPEED")
        {
            consoleActionScript.ChangeMovementSpeed(word[1]);
        }
        else if(word[0] == "JUMPFORCE")
        {
            consoleActionScript.ChangeJumpSpeed(word[1]);
        }
        else if(word[0] == "SPRINTSPEED")
        {
            consoleActionScript.ChangeSprintSpeed(word[1]);
        }
        else if(word[0] == "DRAG")
        {
            consoleActionScript.ChangeDrag(word[1]);
        }
        else if(word[0] == "MAXSPEED")
        {
            consoleActionScript.ChangeMaxSpeed(word[1]);
        }
        else if(word[0] == "DASHSPEED")
        {
            consoleActionScript.ChangeDashSpeed(word[1]);
        }
        else if(word[0] == "MAXDASH")
        {
            consoleActionScript.ChangeMaxDash(word[1]);
        }
        else if(word[0] == "CAMERASHAKE")
        {
            consoleActionScript.CameraShake(word[1]);
        }
        else
        {
            consoleArrow.color = new Color(255, 0, 0, 255);
        }

        consoleText.text = "";
    }
}
