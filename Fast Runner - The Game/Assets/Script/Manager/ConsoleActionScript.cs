using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleActionScript : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Transform playerTransform;
    private Vector3 resetPos;

    [Header("Script")]
    [SerializeField] private CharacterScript characterScript;

    private void Awake()
    {
        resetPos = playerTransform.position;
    }

    protected internal void ResetPos()
    {
        playerTransform.position = resetPos;
    }

    protected internal void ChangeMovementSpeed(string coord)
    {
        string[] coordToAxis = coord.Split(',');

        Vector3 vector = new Vector3(float.Parse(coordToAxis[0]), float.Parse(coordToAxis[1]), float.Parse(coordToAxis[2]));

        characterScript.SendMessage("ChangeMovement", vector);
    }

    protected internal void ChangeSprintSpeed(string value)
    {
        float valueToFloat = float.Parse(value);

        characterScript.SendMessage("ChangeSprint", valueToFloat);
    }

    protected internal void ChangeJumpSpeed(string value)
    {
        float valueToFloat = float.Parse(value);

        characterScript.SendMessage("ChangeJump", valueToFloat);
    }

    protected internal void ChangeLocation(string coord)
    {
        string[] coordToAxis = coord.Split(',');

        Vector3 vector = new Vector3(float.Parse(coordToAxis[0]), float.Parse(coordToAxis[1]), float.Parse(coordToAxis[2]));

        playerTransform.localPosition += vector;
    }
}
