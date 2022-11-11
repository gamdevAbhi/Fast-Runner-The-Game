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
    [SerializeField] private int totalDashTime = 1;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 2f;

    [Header("Camera")]
    [SerializeField] private Transform _camera;

    private Rigidbody  _rigidbody;
    private bool isSprint = false;
    private float initialSpeed = 0.5f;
    private int maximumDash = 0;
    private enum PreviousCommand {Forward, Left, Right, Backward, Stand};
    private PreviousCommand command = PreviousCommand.Stand;

    private Transform _transform;

    private void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        maximumDash = totalDashTime;
    }

    protected internal void MoveForward()
    {
        CheckInitialSpeed(PreviousCommand.Forward);

        Vector3 movePos = _transform.forward * forwardMovement * initialSpeed * Time.deltaTime;

        if(isSprint == true)
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

        if(isSprint == true)
        {
            movePos *= sprint;
        }

        _transform.position += movePos;
        
        initialSpeed = (initialSpeed + (initialSpeed * offsetSpeed * Time.deltaTime) > 1f)? 1f : initialSpeed + (initialSpeed * offsetSpeed * Time.deltaTime);

        command = PreviousCommand.Backward;
    }

    protected internal void MoveLeft()
    {
        CheckInitialSpeed(PreviousCommand.Left);

        Vector3 movePos = -_transform.right * sideMovement * initialSpeed * Time.deltaTime;

        if(isSprint == true)
        {
            movePos *= sprint;
        }

        _transform.position += movePos;
        
        initialSpeed = (initialSpeed + (initialSpeed * offsetSpeed * Time.deltaTime) > 1f)? 1f : initialSpeed + (initialSpeed * offsetSpeed * Time.deltaTime);

        command = PreviousCommand.Left;
    }

    protected internal void MoveRight()
    {
        CheckInitialSpeed(PreviousCommand.Right);

        Vector3 movePos = _transform.right * sideMovement * initialSpeed * Time.deltaTime;

        if(isSprint == true)
        {
            movePos *= sprint;
        }

        _transform.position += movePos;
        
        initialSpeed = (initialSpeed + (initialSpeed * offsetSpeed * Time.deltaTime) > 1f)? 1f : initialSpeed + (initialSpeed * offsetSpeed * Time.deltaTime);

        command = PreviousCommand.Right;
    }

    protected internal void Jump()
    {
        if(command == PreviousCommand.Forward && isSprint == true)
        {
            _rigidbody.velocity = _transform.up * jumpForce * 1.25f + _transform.forward * initialSpeed * sprint * forwardMovement;
        }
        else
        {
            _rigidbody.velocity = Vector3.up * jumpForce;
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
            if(isSprint == true)
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
