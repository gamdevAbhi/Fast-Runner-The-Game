using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class CharacterMovementScript : MonoBehaviour
{
    [field:Header("Movement Speed")]
    [field:SerializeField] private float forwardMovement = 2;
    [SerializeField] private float sideMovement = 1.6f;
    [SerializeField] private float backwardMovement = 1.3f;
    [SerializeField] private float offsetSpeed = 0.2f;
    [SerializeField] private float speedMax = 1.25f;

    [Header("Other")]
    [SerializeField] private float sprint = 2f;
    [SerializeField] private float dash = 3.5f;
    [SerializeField] private float crouchScale = 0.6f;
    [SerializeField] private int totalDashTime = 1;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 2f;

    [Header("Camera")]
    [SerializeField] private Transform _camera;
    
    [Header("Transform")]
    [SerializeField] private Transform pivotTransform;

    private Rigidbody  _rigidbody;
    private bool isSprint = false;
    private bool isCrouch = false;
    private float initialSpeed = 0.5f;
    private int maximumDash = 0;
    private float localCrouchScale = 0f;
    private float localStandScale = 0f;
    private enum PreviousCommand {Forward, Left, Right, Backward, Stand};
    private PreviousCommand command = PreviousCommand.Stand;

    private Transform _transform;

    private void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        maximumDash = totalDashTime;
        localStandScale = _transform.localScale.y;
        localCrouchScale = localStandScale * crouchScale;
    }

    protected internal void MoveForward()
    {
        CheckInitialSpeed(PreviousCommand.Forward);

        Vector3 movePos = _transform.forward * forwardMovement * initialSpeed * Time.deltaTime;

        if(isSprint == true && isCrouch == false)
        {
            movePos *= sprint;
        }

        _transform.position += movePos;
        
        initialSpeed = (initialSpeed + (initialSpeed * offsetSpeed * Time.deltaTime) > speedMax)? speedMax : initialSpeed + (initialSpeed * offsetSpeed * Time.deltaTime);

        command = PreviousCommand.Forward;
    }

    protected internal void MoveBackward()
    {
        CheckInitialSpeed(PreviousCommand.Backward);

        Vector3 movePos = -_transform.forward * backwardMovement * initialSpeed * Time.deltaTime;

        _transform.position += movePos;
        
        initialSpeed = (initialSpeed + (initialSpeed * offsetSpeed * Time.deltaTime) > 1f)? 1f : initialSpeed + (initialSpeed * offsetSpeed * Time.deltaTime);

        command = PreviousCommand.Backward;
    }

    protected internal void MoveLeft()
    {
        CheckInitialSpeed(PreviousCommand.Left);

        Vector3 movePos = -_transform.right * sideMovement * initialSpeed * Time.deltaTime;

        _transform.position += movePos;
        
        initialSpeed = (initialSpeed + (initialSpeed * offsetSpeed * Time.deltaTime) > 1f)? 1f : initialSpeed + (initialSpeed * offsetSpeed * Time.deltaTime);

        command = PreviousCommand.Left;
    }

    protected internal void MoveRight()
    {
        CheckInitialSpeed(PreviousCommand.Right);

        Vector3 movePos = _transform.right * sideMovement * initialSpeed * Time.deltaTime;

        _transform.position += movePos;
        
        initialSpeed = (initialSpeed + (initialSpeed * offsetSpeed * Time.deltaTime) > 1f)? 1f : initialSpeed + (initialSpeed * offsetSpeed * Time.deltaTime);

        command = PreviousCommand.Right;
    }

    protected internal void Jump(bool wallRun)
    {
        if(wallRun == true)
        {
            _rigidbody.velocity = _transform.up * jumpForce + _transform.forward * initialSpeed * sprint * forwardMovement;
        }
        else if(isCrouch == false && wallRun == false)
        {
            if(command == PreviousCommand.Forward && isSprint == true)
            {
                _rigidbody.velocity = _transform.up * jumpForce * 1.2f + _transform.forward * initialSpeed * sprint * forwardMovement / 2f;
            }
            else
            {
                _rigidbody.velocity = Vector3.up * jumpForce;
            }
        }
    }

    protected internal void Dash()
    {
        if(totalDashTime > 0)
        {
            _rigidbody.velocity = _camera.forward * dash * 10f;
            totalDashTime --;
        }
    }
    
    protected internal bool CanDash()
    {
        bool result = (totalDashTime > 0)? true : false;

        return result;
    }

    protected internal void MoveNone()
    {
        command = PreviousCommand.Stand;
    }

    private void CheckInitialSpeed(PreviousCommand currentCommand)
    {
        if(command == PreviousCommand.Stand || command != currentCommand)
        {
            initialSpeed = 0.5f;
        }
    }

    protected internal void Sprint(bool _case)
    {
        isSprint = _case;
    }

    protected internal void Crouch(bool _case, bool state)
    {
        if(_case == true)
        {
            if(isSprint == true && state == true && isCrouch == false && command != PreviousCommand.Stand)
            {
                _rigidbody.velocity = _camera.forward * dash * 10f;
            }

            Vector3 pos = _transform.localPosition;
            Vector3 pivotPoint = pivotTransform.position;

            Vector3 point = pos - pivotPoint;

            Vector3 scalePoint = new Vector3(1f, localCrouchScale, 1f);

            float multiplier = scalePoint.y / _transform.localScale.y;

            Vector3 result = pivotPoint + point * multiplier;

            _transform.localScale = scalePoint;
            _transform.localPosition = result;

            isCrouch = true;
        }
        else
        {
            Vector3 pos = _transform.localPosition;
            Vector3 pivotPoint = pivotTransform.position;

            Vector3 point = pos - pivotPoint;

            Vector3 scalePoint = new Vector3(1f, localStandScale, 1f);

            float multiplier = scalePoint.y / _transform.localScale.y;

            Vector3 result = pivotPoint + point * multiplier;

            _transform.localScale = scalePoint;
            _transform.localPosition = result;

            isCrouch = false;
        }
        
    }

    protected internal void ChangeMovementSpeed(Vector3 vector)
    {
        forwardMovement = vector.x;
        backwardMovement = vector.y;
        sideMovement = vector.z;
    }

    protected internal void ChangeSprintSpeed(float value)
    {
        sprint = value;
    }

    protected internal void ChangeJumpSpeed(float value)
    {
        jumpForce = value;
    }

    protected internal string CurrentState()
    {
        string state = "";

        if(command == PreviousCommand.Stand)
        {
            state = "Stand";
        }
        else
        {
            if(isSprint == true && isCrouch == false && command == PreviousCommand.Forward)
            {
                state = "Run";
            }
            else
            {
                state = "Walk";
            }
        }

        return state;
    }

    protected internal void ChangeMaxSpeed(float value)
    {
        speedMax = value;
    }

    protected internal void ResetDash()
    {
        totalDashTime = maximumDash;
    }

    protected internal void ChangeMaxDash(int value)
    {
        maximumDash = value;
    }

    protected internal void ChangeDashSpeed(float value)
    {
        dash = value;
    }

    protected internal void ChangeOffsetSpeed(float value)
    {
        offsetSpeed = value;
    }
}
