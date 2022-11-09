using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroundScript : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Transform raycastOriginTransform;

    [Header("Parameters")]
    [SerializeField] private float delayOffset;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask[] layerName;
    [SerializeField] private bool isGround;

    private float delayTimeFall;
    private float delayTimeJump;
    private bool canJump;

    private void Awake()
    {
        delayTimeFall = delayOffset;
    }

    private void FixedUpdate()
    {
        isGround = CheckGround();

        if(isGround == true && delayTimeJump == 0f)
        {
            canJump = true;
            delayTimeFall = delayOffset;
        }
        else if(isGround == true && delayTimeJump > 0f)
        {
            GetComponent<CharacterMovementScript>().Jump();
            delayTimeJump = 0f;
        }
        else
        {
            delayTimeFall = (delayTimeFall - Time.fixedDeltaTime > 0f)? delayTimeFall - Time.fixedDeltaTime : 0f;
            canJump = false;

            delayTimeJump = (delayTimeJump - Time.fixedDeltaTime > 0f)? delayTimeJump - Time.fixedDeltaTime : 0f;
        }
    }

    protected internal bool CanJump()
    {
        bool result = false;

        if(isGround == true)
        {
            result = true;
        }
        else
        {
            if(delayTimeFall > 0f && canJump == true)
            {
                result = true;
            }
            else if(canJump == false)
            {
                delayTimeJump = delayOffset;
            }
        }

        return result;
    }

    private bool CheckGround()
    {
        bool isHit = false;

        foreach(LayerMask layer in layerName)
        {
            isHit = Physics.Raycast(raycastOriginTransform.position, raycastOriginTransform.TransformDirection(Vector3.down), maxDistance, layer);

            if(isHit == true)
            {
                break;
            }
        }

        return isHit;
    }

    protected internal bool GetGroundCheck()
    {
        return isGround;
    }

}
