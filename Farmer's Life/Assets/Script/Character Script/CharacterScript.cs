using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterMovementScript))]
[RequireComponent(typeof(CharacterControllerScript))]
[RequireComponent(typeof(CharacterGroundScript))]
public class CharacterScript : MonoBehaviour
{
    private CharacterMovementScript characterMovementScript;
    private CharacterControllerScript characterControllerScript;
    private CharacterGroundScript CharacterGroundScript;

    [Header("Script")]
    [SerializeField] private KeyMapManagerScript keyMapManagerScript;

    private void Awake()
    {
        characterMovementScript = GetComponent<CharacterMovementScript>();
        characterControllerScript = GetComponent<CharacterControllerScript>();
        CharacterGroundScript = GetComponent<CharacterGroundScript>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Rigidbody _rigid = GetComponent<Rigidbody>();
        _rigid.freezeRotation = true;
    }

    private void Update()
    {
        List<string> value = keyMapManagerScript.GetAction("CharacterNonFixedMap");
        
        foreach(string command in value)
        {
            MoveCharacter(command);
        }

        value = keyMapManagerScript.GetAction("MouseMove");

        RotateLook(value);
    }

    private void FixedUpdate()
    {
        List<string> value = keyMapManagerScript.GetAction("CharacterFixedMap");
        
        foreach(string command in value)
        {
            MoveCharacterFixed(command);
        }
    }

    private void MoveCharacter(string command)
    {
        if(command == "Forward")
        {
            characterMovementScript.MoveForward();
        }
        else if(command == "Backward")
        {
            characterMovementScript.MoveBackward();
        }
        else if(command == "Left")
        {
            characterMovementScript.MoveLeft();
        }
        else if(command == "Right")
        {
            characterMovementScript.MoveRight();
        }
        else if(command == "Sprint On")
        {
            characterMovementScript.Sprint(true);
        }
        else if(command == "Sprint Off")
        {
            characterMovementScript.Sprint(false);
        }
        else if(command == "")
        {
            characterMovementScript.MoveNone();
        }
    }

    private void MoveCharacterFixed(string command)
    {
        if(command == "Jump")
        {
            if(CharacterGroundScript.CanJump() == true)
            {
                characterMovementScript.Jump();
            }
        }
    }

    private void RotateLook(List<string> mouseMove)
    {
        Vector2 result = new Vector2(float.Parse(mouseMove[0]), float.Parse(mouseMove[1]));
        characterControllerScript.Rotate(result);
    }

    private void ChangeMovement(Vector3 vector)
    {
        characterMovementScript.ChangeMovementSpeed(vector);
    }

    private void ChangeSprint(float value)
    {
        characterMovementScript.ChangeSprintSpeed(value);
    }

    private void ChangeJump(float value)
    {
        characterMovementScript.ChangeJumpSpeed(value);
    }
}
