using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeScript : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] private float maxUpAxis = 1f;
    [SerializeField] private float maxDownAxis = -1f;
    private float currentY;
    private enum ShakeType {Stand, Walk, Run};
    [SerializeField] private float walkShakeSpeed = 0.08f;
    [SerializeField] private float runShakeSpeed = 0.15f;
    [SerializeField] private float runShakeMax = 1.5f;
    private Transform _transform;
    private enum Direction {Up, Down};

    [Header("Current Status")]
    [SerializeField] private ShakeType shakeType = ShakeType.Stand;
    [SerializeField] private Direction currentDirection = Direction.Up;

    [Header("Transform")]
    [SerializeField] private Transform cameraHolder;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        currentY = cameraHolder.position.y;
    }

    private void Update()
    {
        currentY = cameraHolder.position.y;

        if(shakeType == ShakeType.Walk)
        {
            Shake(walkShakeSpeed, 1f);
        }
        else if(shakeType == ShakeType.Run)
        {
            Shake(runShakeSpeed, runShakeMax);
        }
        else if(shakeType == ShakeType.Stand)
        {
            _transform.position = new Vector3(_transform.position.x, cameraHolder.position.y, _transform.position.z);
        }
    }

    private void Shake(float speed, float shake)
    {
        float move = 0f;
        float finalMaxUp = maxUpAxis * shake + currentY;
        float finalMaxDown =  currentY - maxDownAxis * shake;

        if(currentDirection == Direction.Up)
        {
            move = (_transform.position.y + Time.deltaTime * speed * 10f > finalMaxUp)? finalMaxUp : _transform.position.y + Time.deltaTime * speed * 10f;
        }
        else if(currentDirection == Direction.Down)
        {
            move = (_transform.position.y - Time.deltaTime * speed * 10f < finalMaxDown)? finalMaxDown : _transform.position.y - Time.deltaTime * speed * 10f;
        }

        if(move == finalMaxUp)
        {
            currentDirection = Direction.Down;
        }
        else if(move == finalMaxDown)
        {
            currentDirection = Direction.Up;
        }

        _transform.position = new Vector3(_transform.position.x, move, _transform.position.z);
    }

    protected internal void ChangeStatus(string status)
    {
        if(status == "Walk")
        {
            shakeType = ShakeType.Walk;
        }
        else if(status == "Run")
        {
            shakeType = ShakeType.Run;
        }
        else if(status == "Stand")
        {
            shakeType = ShakeType.Stand;
        }
    }
}
