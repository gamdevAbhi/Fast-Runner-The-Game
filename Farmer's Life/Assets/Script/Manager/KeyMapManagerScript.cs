using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManagerScript))]
public class KeyMapManagerScript : MonoBehaviour
{
    [Header("KeyMaps")]
    [SerializeField] private List<KeyMap> characterNonFixedMap;
    [SerializeField] private List<KeyMap> characterFixedMap;
    [SerializeField] private List<KeyMap> consoleMap;
    [SerializeField] private InputManagerScript inputManagerScript;

    protected internal List<string> GetAction(string command)
    {
        List<string> value = new List<string>();

        if(command == "CharacterNonFixedMap")
        {
            value = CheckInput(characterNonFixedMap);
        }
        else if(command == "CharacterFixedMap")
        {
            value = CheckInput(characterFixedMap);
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
    [SerializeField] private MapType mapType;
    [SerializeField] private ActionType actionType;

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
}
