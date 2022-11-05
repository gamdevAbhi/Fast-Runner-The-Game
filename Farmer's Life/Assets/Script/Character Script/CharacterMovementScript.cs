using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class CharacterMovementScript : MonoBehaviour
{
    [Header("Movement Speed")]
    [SerializeField] private float forwardMovement = 2;
    [SerializeField] private float sideMovement = 1.6f;
    [SerializeField] private float backwardMovement = 1.3f;
    [SerializeField] private float offsetSpeed = 0.2f;
    private float initialSpeed = 0.5f;
    private enum PreviousCommand {Forward, Left, Right, Backward, None};
    private PreviousCommand command = PreviousCommand.None;

    private Transform _transform;

    private void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
    }

    protected internal void MoveForward()
    {
        CheckInitialSpeed(PreviousCommand.Forward);

        Vector3 movePos = _transform.forward * forwardMovement * initialSpeed * Time.deltaTime;
        _transform.position += movePos;
        
        initialSpeed = (initialSpeed + (initialSpeed * offsetSpeed) > 1f)? 1f : initialSpeed + (initialSpeed * offsetSpeed);

        command = PreviousCommand.Forward;
    }

    protected internal void MoveBackward()
    {
        CheckInitialSpeed(PreviousCommand.Backward);

        Vector3 movePos = -_transform.forward * backwardMovement * initialSpeed * Time.deltaTime;
        _transform.position += movePos;
        
        initialSpeed = (initialSpeed + (initialSpeed * offsetSpeed) > 1f)? 1f : initialSpeed + (initialSpeed * offsetSpeed);

        command = PreviousCommand.Backward;
    }

    protected internal void MoveLeft()
    {
        CheckInitialSpeed(PreviousCommand.Left);

        Vector3 movePos = -_transform.right * sideMovement * initialSpeed * Time.deltaTime;
        _transform.position += movePos;
        
        initialSpeed = (initialSpeed + (initialSpeed * offsetSpeed) > 1f)? 1f : initialSpeed + (initialSpeed * offsetSpeed);

        command = PreviousCommand.Left;
    }

    protected internal void MoveRight()
    {
        CheckInitialSpeed(PreviousCommand.Right);

        Vector3 movePos = _transform.right * sideMovement * initialSpeed * Time.deltaTime;
        _transform.position += movePos;
        
        initialSpeed = (initialSpeed + (initialSpeed * offsetSpeed) > 1f)? 1f : initialSpeed + (initialSpeed * offsetSpeed);

        command = PreviousCommand.Right;
    }

    protected internal void MoveNone()
    {
        command = PreviousCommand.None;
    }

    private void CheckInitialSpeed(PreviousCommand currentCommand)
    {
        if(command == PreviousCommand.None || command != currentCommand)
        {
            initialSpeed = 0.5f;
        }
    }
}
