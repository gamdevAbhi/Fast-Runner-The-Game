using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeScript : MonoBehaviour
{
    [Header("Offset")]
    [SerializeField] private float maxYAxis = 1f;
    [SerializeField] private float maxXAxis = 1f;
    [SerializeField] private float runShakeMax = 1.5f;
    private float currentY;
    private float shakeImpactTime = 0f;
    private enum ShakeType {Stand, Walk, Run, Jump};

    [Header("Speed")]
    [SerializeField] private float walkShakeSpeed = 0.08f;
    [SerializeField] private float runShakeSpeed = 0.15f;
    [SerializeField] private float fallShakeSpeed = 0.08f;

    [Header("Falling Shake")]
    [SerializeField] private float limitStable = 5f;
    [SerializeField] private float ShakeImpact = 2f;
    [SerializeField] private float maxShakeImpactTime = 5f;

    private Transform _transform;
    private enum Direction {Up, Down};
    private enum FallDirection {Right, Left};

    [Header("Current Status")]
    [SerializeField] private bool shouldShake = true;
    [SerializeField] private ShakeType shakeType = ShakeType.Stand;
    [SerializeField] private Direction currentDirection = Direction.Up;
    [SerializeField] private FallDirection fallDirection = FallDirection.Right;

    [Header("Transform")]
    [SerializeField] private Transform cameraHolder;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody playerRigid;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if(shouldShake == true)
        {
            CalculateShake();
        }
    }

    private void CalculateShake()
    {
        currentY = cameraHolder.position.y;

        if(shakeType == ShakeType.Walk)
        {
            Shake(walkShakeSpeed, 1f);
            _transform.position = new Vector3(cameraHolder.position.x, _transform.position.y, _transform.position.z);
        }
        else if(shakeType == ShakeType.Run)
        {
            Shake(runShakeSpeed, runShakeMax);
            _transform.position = new Vector3(cameraHolder.position.x, _transform.position.y, _transform.position.z);
        }
        else if(shakeType == ShakeType.Jump)
        {
            if(playerRigid.velocity.y < -limitStable || playerRigid.velocity.y > limitStable)
            {
                shakeImpactTime = (shakeImpactTime + Time.deltaTime > maxShakeImpactTime)? maxShakeImpactTime : shakeImpactTime + Time.deltaTime;
                float currentShakeImpact = shakeImpactTime / maxShakeImpactTime * ShakeImpact;

                FallShake(fallShakeSpeed * Mathf.Abs(playerRigid.velocity.y), currentShakeImpact);
            }
            else
            {
                shakeImpactTime = 0f;
            }
        }
        else if(shakeType == ShakeType.Stand)
        {
            _transform.position = new Vector3(cameraHolder.position.x, cameraHolder.position.y, _transform.position.z);
        }

        if(shakeType != ShakeType.Jump)
        {
            shakeImpactTime = 0f;
        }
    }

    private void Shake(float speed, float shake)
    {
        float move = 0f;
        float finalMaxUp = maxYAxis * shake + currentY;
        float finalMaxDown =  currentY - maxYAxis * shake;

        if(currentDirection == Direction.Up)
        {
            move = (_transform.position.y + Time.deltaTime * speed > finalMaxUp)? finalMaxUp : _transform.position.y + Time.deltaTime * speed * 10f;
        }
        else if(currentDirection == Direction.Down)
        {
            move = (_transform.position.y - Time.deltaTime * speed < finalMaxDown)? finalMaxDown : _transform.position.y - Time.deltaTime * speed * 10f;
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

    private void FallShake(float speed, float currentShakeImpact)
    {
        float move = 0f;
        float finalMaxRight = maxXAxis * currentShakeImpact;
        float finalMaxLeft =  -maxXAxis * currentShakeImpact;

        if(fallDirection == FallDirection.Right)
        {
            move = (_transform.localPosition.x + Time.deltaTime * speed * 10f > finalMaxRight)? finalMaxRight : _transform.localPosition.x + Time.deltaTime * speed * 10f;
        }
        else if(fallDirection == FallDirection.Left)
        {
            move = (_transform.localPosition.x - Time.deltaTime * speed * 10f < finalMaxLeft)? finalMaxLeft : _transform.localPosition.x - Time.deltaTime * speed * 10f;
        }

        if(move == finalMaxRight)
        {
            fallDirection = FallDirection.Left;
        }
        else if(move == finalMaxLeft)
        {
            fallDirection = FallDirection.Right;
        }

        _transform.localPosition = new Vector3(move, _transform.localPosition.y, _transform.localPosition.z);
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
        else if(status == "Jump")
        {
            shakeType = ShakeType.Jump;
        }
    }

    protected internal void ChangeCameraShake(bool status)
    {
        shouldShake = status;
    }
}
