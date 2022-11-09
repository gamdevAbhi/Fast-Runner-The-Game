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
    [SerializeField] private CameraShakeScript cameraShakeScript;

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

    protected internal void ChangeDrag(string value)
    {
        playerTransform.GetComponent<Rigidbody>().drag = float.Parse(value) / 10f;
    }

    protected internal void ChangeMaxSpeed(string value)
    {
        characterScript.SendMessage("ChangeMaxSpeed", float.Parse(value));
    }

    protected internal void ChangeDashSpeed(string value)
    {
        characterScript.SendMessage("ChangeDashSpeed", float.Parse(value));
    }

    protected internal void ChangeMaxDash(string value)
    {
        characterScript.SendMessage("ChangeMaxDash", int.Parse(value));
    }

    protected internal void CameraShake(string value)
    {
        if(value == "ON")
        {
            cameraShakeScript.ChangeCameraShake(true);
        }
        else if(value == "OFF")
        {
            cameraShakeScript.ChangeCameraShake(false);
        }
    }
}
