using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManagerScript))]
public class KeyMapManagerScript : MonoBehaviour
{
    [Header("KeyMaps")]
    [SerializeField] private List<KeyMap> characterMap;
    [SerializeField] private List<KeyMap> consoleMap;
    [SerializeField] private InputManagerScript inputManagerScript;

    private void Start()
    {
        characterMap = PrioritySet(characterMap);
        consoleMap = PrioritySet(consoleMap);
    }

    protected internal List<KeyMap> PrioritySet(List<KeyMap> map)
    {
        List<KeyMap> mapTemp = new List<KeyMap>();

        foreach(KeyMap key in map)
        {
            if(key.GetPriority() == "First")
            {
                mapTemp.Add(key);
            }
        }

        foreach(KeyMap key in map)
        {
            if(key.GetPriority() == "Second")
            {
                mapTemp.Add(key);
            }
        }

        foreach(KeyMap key in map)
        {
            if(key.GetPriority() == "Third")
            {
                mapTemp.Add(key);
            }
        }

        return mapTemp;
    }

    protected internal List<string> GetAction(string command)
    {
        List<string> value = new List<string>();

        if(command == "CharacterdMap")
        {
            value = CheckInput(characterMap);
        }
        else if(command == "MouseMove")
        {
            value = MouseMove();
        }
        else if(command == "Console")
        {
            value = CheckInput(consoleMap);
        }

        return value;
    }

    private List<string> CheckInput(List<KeyMap> keyMap)
    {
        return inputManagerScript.MatchKeys(keyMap);
    }
    
    private List<string> MouseMove()
    {
        return inputManagerScript.GetMouseInput();
    }
}

[System.Serializable]
public class KeyMap
{
    [SerializeField] private string actionName;
    [SerializeField] private string keyName;
    private enum MapType {Constant, Primary};
    private enum ActionType {Press, Hold, Release};
    private enum Priority {First, Second, Third};
    [SerializeField] private MapType mapType;
    [SerializeField] private ActionType actionType;
    [SerializeField] private Priority priority;


    protected internal string KeyName()
    {
        return keyName;
    }

    protected internal string ActionName()
    {
        return actionName;
    }

    protected internal string GetMapType()
    {
        return (mapType == MapType.Primary)? "Primary" : "Constant";
    }

    protected internal string GetActionType()
    {
        if(actionType == ActionType.Press)
        {
            return "Press";
        }
        else if(actionType == ActionType.Hold)
        {
            return "Hold";
        }
        else
        {
            return "Release";
        }
    }

    protected internal string GetPriority()
    {
        if(priority == Priority.First)
        {
            return "First";
        }
        else if(priority == Priority.Second)
        {
            return "Second";
        }
        else
        {
            return "Third";
        }
    }
}
