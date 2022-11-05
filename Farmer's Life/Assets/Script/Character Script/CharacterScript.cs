using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterMovementScript))]
[RequireComponent(typeof(CharacterControllerScript))]
public class CharacterScript : MonoBehaviour
{
    private CharacterMovementScript characterMovementScript;
    private CharacterControllerScript characterControllerScript;

    private void Awake()
    {
        characterMovementScript = GetComponent<CharacterMovementScript>();
        characterControllerScript = GetComponent<CharacterControllerScript>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Rigidbody _rigid = GetComponent<Rigidbody>();
        _rigid.freezeRotation = true;
    }

    private void Update()
    {
        RotationInputCheck();
    }

    private void MovementInput(string command)
    {
        if(command == "Forward")
        {
            characterMovementScript.MoveForward();
        }
        else if(command == "Backward")
        {
            characterMovementScript.MoveBackward();
        }
        if(command == "Left")
        {
            characterMovementScript.MoveLeft();
        }
        if(command == "Right")
        {
            characterMovementScript.MoveRight();
        }
        else if(command == "")
        {
            characterMovementScript.MoveNone();
        }
    }

    private void RotationInputCheck()
    {
        if(Input.GetAxis("Mouse X") != 0f)
        {
            characterControllerScript.HorizontalRotation(Input.GetAxis("Mouse X"));
        }
        if(Input.GetAxis("Mouse Y") != 0f)
        {
            characterControllerScript.VerticalRotation(Input.GetAxis("Mouse Y"));
        }
    }
}
