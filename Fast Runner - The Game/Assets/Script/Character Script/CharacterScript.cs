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
    private CharacterGroundScript characterGroundScript;

    [Header("Script")]
    [SerializeField] private KeyMapManagerScript keyMapManagerScript;
    [SerializeField] private CameraShakeScript cameraShakeScript;

    private void Awake()
    {
        characterMovementScript = GetComponent<CharacterMovementScript>();
        characterControllerScript = GetComponent<CharacterControllerScript>();
        characterGroundScript = GetComponent<CharacterGroundScript>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Rigidbody _rigid = GetComponent<Rigidbody>();
        _rigid.freezeRotation = true;
    }

    private void Update()
    {
        List<string> value = keyMapManagerScript.GetAction("CharacterdMap");
        
        MoveCharacter(value);

        if(value.Count == 0)
        {
            value.Add("Stand");
            MoveCharacter(value);
        }

        value = keyMapManagerScript.GetAction("MouseMove");

        RotateLook(value);

        if(characterGroundScript.GetGroundCheck() == true)
        {
            cameraShakeScript.ChangeStatus(characterMovementScript.CurrentState());
            characterMovementScript.ResetDash();
        }
        else
        {
            cameraShakeScript.ChangeStatus("Jump");
        }
    }

    private void MoveCharacter(List<string> value)
    {
        bool isSprint = false;

        foreach(string command in value)
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
            else if(command == "Jump" && characterGroundScript.CanJump() == true)
            {
                characterMovementScript.Jump();
            }
            else if(command == "Sprint" && characterGroundScript.GetGroundCheck())
            {
                isSprint = true;
            }
            else if(command == "Dash" && characterGroundScript.GetGroundCheck() == false)
            {
                characterMovementScript.Dash();
            }
            else if(command == "Stand")
            {
                characterMovementScript.MoveNone();
            }
        }

        characterMovementScript.Sprint(isSprint);
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

    private void ChangeMaxSpeed(float value)
    {
        characterMovementScript.ChangeMaxSpeed(value);
    }

    private void ChangeDashSpeed(float value)
    {
        characterMovementScript.ChangeDashSpeed(value);
    }

    private void ChangeMaxDash(int value)
    {
        characterMovementScript.ChangeMaxDash(value);
    }
}
