using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerScript : MonoBehaviour
{
    [Header("Keyboard Input")]
    [SerializeField] private List<Key> keys;
    [SerializeField] private List<string> keyPressed;
    [SerializeField] private List<string> keyHold;
    [SerializeField] private List<string> keyRelease;

    [Header("Mouse Input")]
    [SerializeField] private Vector2 mouseMove;

    private void Update()
    {
        ClearKeyHistory();
        KeyPress();
        MouseInput();
    }

    private void KeyPress()
    {
        foreach(Key key in keys)
        {
            if(Input.GetKeyDown(key.KeyCode()))
            {
                keyPressed.Add(key.KeyName());
            }
            else if(Input.GetKey(key.KeyCode()))
            {
                keyHold.Add(key.KeyName());
            }
            else if(Input.GetKeyUp(key.KeyCode()))
            {
                keyRelease.Add(key.KeyName());
            }
        }
    }

    private void MouseInput()
    {
        mouseMove = Vector2.zero;

        mouseMove.x = Input.GetAxis("Mouse X");
        mouseMove.y = Input.GetAxis("Mouse Y");
    }

    protected internal List<string> GetMouseInput()
    {
        List<string> result = new List<string>();
        result.Add(mouseMove.y.ToString());
        result.Add(mouseMove.x.ToString());

        return result;
    }

    private void ClearKeyHistory()
    {
        keyPressed = new List<string>();
        keyHold = new List<string>();
        keyRelease = new List<string>();
    }

    protected internal List<string> MatchKeys(List<KeyMap> keyMap)
    {
        List<string> result = new List<string>();

        foreach(KeyMap key in keyMap)
        {
            if(CheckKey(key.KeyName(), key.GetActionType()))
            {
                result.Add(key.ActionName());
            }
        }

        return result;
    }

    protected internal bool CheckKey(string name, string type)
    {
        bool result = false;

        if(type == "Press")
        {
            foreach(string key in keyPressed)
            {
                if(key == name)
                {
                    result = true;
                }
            }
        }
        else if(type == "Hold")
        {
            foreach(string key in keyHold)
            {
                if(key == name)
                {
                    result = true;
                }
            }
        }
        else
        {
            foreach(string key in keyRelease)
            {
                if(key == name)
                {
                    result = true;
                }
            }
        }

        return result;
    }
}

[System.Serializable]
public class Key
{
    [SerializeField] private string keyName;
    [SerializeField] private KeyCode keyCode;

    protected internal string KeyName()
    {
        return keyName;
    }

    protected internal KeyCode KeyCode()
    {
        return keyCode;
    }
}
