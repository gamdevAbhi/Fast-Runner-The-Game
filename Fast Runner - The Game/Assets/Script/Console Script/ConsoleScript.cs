using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
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

    [Header("Console Codes")]
    [SerializeField] private ConsoleCode[] consoleCode;

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
                if(cmd == "Backspace" && consoleText.text.Length > 0)
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
        bool operationSuccessfull = false;

        foreach(ConsoleCode code in consoleCode)
        {
            if(code.GetCode() == word[0])
            {
                string output = (code._needParam == true)? word[1] : "0";
                code.Run(output);
                operationSuccessfull = true;
                break;
            }
        }

        if(operationSuccessfull == false)
        {
            consoleArrow.color = new Color(255, 0, 0, 255);
        }
        else
        {
            consoleArrow.color = new Color(0, 255, 0, 255);
        }

        consoleText.text = "";
    }
}

[System.Serializable]
public class ConsoleCode
{   
    [SerializeField] private string code;
    [SerializeField] private UnityEvent<string> referenceMethod;
    [SerializeField] private bool needParam = true;
    public bool _needParam
    {
        get
        {
            return needParam;
        }
    }

    protected internal void Run(string value)
    {
        referenceMethod.Invoke(value);
    }

    protected internal string GetCode()
    {
        return code.ToUpper();
    }
}