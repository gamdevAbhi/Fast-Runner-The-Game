using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerScript : MonoBehaviour
{
    [Header("Keyboard Input")]
    [SerializeField] private List<Key> keys;
    [SerializeField] private List<PrimaryAction> primaryKey;
    [SerializeField] private List<ConstantAction> constantKey;

    [Header("Script")]
    [SerializeField] private CharacterScript characterScript;

    private void Update()
    {
        KeyPress();
    }

    private void KeyPress()
    {
        bool anyKeyPress = false;

        foreach(Key key in keys)
        {
            if(Input.GetKeyDown(key.KeyCode()))
            {
                PrimaryKeyCheck(key.KeyName(), "Press");
                anyKeyPress = true;
            }
            else if(Input.GetKey(key.KeyCode()))
            {
                PrimaryKeyCheck(key.KeyName(), "Hold");
                anyKeyPress = true;
            }
            else if(Input.GetKeyUp(key.KeyCode()))
            {
                PrimaryKeyCheck(key.KeyName(), "Release");
                anyKeyPress = true;
            }
        }

        if(anyKeyPress == false)
        {
            ActionCommand("Player", "");
        }
    }

    private void PrimaryKeyCheck(string keyName, string method)
    {
        foreach(PrimaryAction primary in primaryKey)
        {
            if(primary._ActionType() == method && primary.KeyName() == keyName)
            {
                ActionCommand(primary._Target(), primary.ActionName());
                break;
            }
        }
    }

    private KeyCode KeyFind(string keyName)
    {
        foreach(Key key in keys)
        {
            if(key.KeyName() == keyName)
            {
                return key.KeyCode();
            }
        }

        return KeyCode.None;
    }

    private void ActionCommand(string target, string command)
    {
        if(target == "Player")
        {
            characterScript.SendMessage("MovementInput", command);
        }
    }
}


[System.Serializable]
public class PrimaryAction : Action
{
    [SerializeField] private ActionType actionType;
    [SerializeField] private Target target;
    const Prior prior = Prior.Primary;

    protected internal string _ActionType()
    {
        string type = "";

        if(actionType == ActionType.Press)
        {
            type = "Press";
        }
        else if(actionType == ActionType.Hold)
        {
            type = "Hold";
        }
        else
        {
            type = "Release";
        }

        return type;
    }

    protected internal string _Target()
    {
        string type = "";

        if(target == Target.Player)
        {
            type = "Player";
        }
        else 
        {
            type = "Setting";
        }

        return type;
    }
}

[System.Serializable]
public class ConstantAction : Action
{
    const Prior prior = Prior.Constant;
    [SerializeField] private ActionType actionType;
    [SerializeField] private Target target;

    protected internal string _ActionType()
    {
        string type = "";

        if(actionType == ActionType.Press)
        {
            type = "Press";
        }
        else if(actionType == ActionType.Hold)
        {
            type = "Hold";
        }
        else
        {
            type = "Release";
        }

        return type;
    }

    protected internal string _Target()
    {
        string type = "";

        if(target == Target.Player)
        {
            type = "Player";
        }
        else 
        {
            type = "Setting";
        }

        return type;
    }
}

[System.Serializable]
public class Action
{
    [SerializeField] private string acitonName;
    [SerializeField] private string keyName;
    protected internal enum Prior {Constant, Primary};
    protected internal enum ActionType {Press, Hold, Release};
    protected internal enum Target {Player, Setting};

    protected internal string KeyName()
    {
        return keyName;
    }

    protected internal string ActionName()
    {
        return acitonName;
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
