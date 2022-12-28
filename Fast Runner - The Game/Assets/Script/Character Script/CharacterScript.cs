using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterMovementScript))]
[RequireComponent(typeof(CharacterControllerScript))]
[RequireComponent(typeof(CharacterGroundScript))]
[RequireComponent(typeof(CharacterGroundImpactScript))]
[RequireComponent(typeof(CharacterAttackScript))]
[RequireComponent(typeof(CharacterWallRunScript))]
public class CharacterScript : MonoBehaviour
{
    private CharacterMovementScript characterMovementScript;
    private CharacterControllerScript characterControllerScript;
    private CharacterGroundScript characterGroundScript;
    private CharacterGroundImpactScript characterGroundImpactScript;
    private CharacterAttackScript characterAttackScript;
    private CharacterWallRunScript characterWallRunScript;

    [Header("Script")]
    [SerializeField] private KeyMapManagerScript keyMapManagerScript;
    [SerializeField] private CameraShakeScript cameraShakeScript;

    private void Awake()
    {
        characterMovementScript = GetComponent<CharacterMovementScript>();
        characterControllerScript = GetComponent<CharacterControllerScript>();
        characterGroundScript = GetComponent<CharacterGroundScript>();
        characterGroundImpactScript = GetComponent<CharacterGroundImpactScript>();
        characterAttackScript = GetComponent<CharacterAttackScript>();
        characterWallRunScript = GetComponent<CharacterWallRunScript>();
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

        if(value.Count == 0)
        {
            value.Add("Stand");
        }
        
        MoveCharacter(value);

        value = keyMapManagerScript.GetAction("MouseMove");

        RotateLook(value);

        if(characterGroundScript.GetGroundCheck() == true && characterGroundImpactScript.IsImpact() == false)
        {
            cameraShakeScript.ChangeStatus(characterMovementScript.CurrentState());
            characterMovementScript.ResetDash();
        }
        else if(characterGroundScript.GetGroundCheck() == true && characterGroundImpactScript.IsImpact() == true)
        {
            cameraShakeScript.ChangeStatus("Impact");
            cameraShakeScript.SetImpactVelocity(characterGroundImpactScript.Impact());
        }
        else
        {
            cameraShakeScript.ChangeStatus("Fall");
        }

        if(characterGroundScript.GetGroundCheck() == true)
        {
            characterAttackScript.Status(false);
        }
    }

    private void MoveCharacter(List<string> value)
    {
        bool isSprint = false;
        bool isCrouch = false;
        bool isWallRunning = false;

        foreach(string command in value)
        {
            if(cameraShakeScript.IsImpacted() == false)
            {
                if(command == "Jump" && characterGroundScript.CanJump() == true)
                {
                    characterMovementScript.Jump(false);
                }
                else if(command == "Jump" && characterGroundScript.CanJump() == false && characterWallRunScript.CheckWallRun())
                {
                    characterWallRunScript.WallJump(true);
                    characterMovementScript.Jump(true);
                }
                else if(command == "Sprint" && characterGroundScript.GetGroundCheck())
                {
                    isSprint = true;
                }
                else if(command == "Sprint" && characterGroundScript.GetGroundCheck() == false && characterWallRunScript.CheckWallRun())
                {
                    isWallRunning = true;
                    characterWallRunScript.WallRun();
                    characterMovementScript.ResetDash();
                }
                else if(command == "Dash" && characterGroundScript.GetGroundCheck() == false && characterMovementScript.CanDash() && isWallRunning == false)
                {
                    characterMovementScript.Dash();
                    characterAttackScript.Status(true);
                }
                else if(command == "Crouch" && isWallRunning == false)
                {
                    isCrouch = true;

                    if(value.Count == 1)
                    {
                        characterMovementScript.MoveNone();
                    }
                }
                else if(command == "Forward" && isWallRunning == false)
                {
                    characterMovementScript.MoveForward();
                }
                else if(command == "Backward" && isWallRunning == false)
                {
                    characterMovementScript.MoveBackward();
                }
                else if(command == "Left" && isWallRunning == false)
                {
                    characterMovementScript.MoveLeft();
                }
                else if(command == "Right" && isWallRunning == false)
                {
                    characterMovementScript.MoveRight();
                }
                else if(command == "Stand")
                {
                    characterMovementScript.MoveNone();
                }
            }
        }

        characterMovementScript.Crouch(isCrouch, characterGroundScript.GetGroundCheck());
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

    private void ChangeOffsetSpeed(float value)
    {
        characterMovementScript.ChangeOffsetSpeed(value);
    }

    private void ChangeWallRunSpeed(float value)
    {
        characterWallRunScript.ChangeWallRunSpeed(value);
    }

    private void ChangeDashDamage(float value)
    {
        characterAttackScript.ChangeDashDamage(value);
    }
}
