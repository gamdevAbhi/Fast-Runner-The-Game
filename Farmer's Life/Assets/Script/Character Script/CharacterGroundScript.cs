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

    private float delayTime;
    private bool canJump;

    private void Awake()
    {
        delayTime = delayOffset;
    }

    private void FixedUpdate()
    {
        isGround = CheckGround();

        if(isGround == true)
        {
            canJump = true;
        }
    }

    private void Update()
    {
        if(isGround == false)
        {
            delayTime = (delayTime - Time.deltaTime > 0f)? delayTime - Time.deltaTime : 0f;
        }
        else
        {
            delayTime = delayOffset;
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
            if(delayTime > 0f && canJump == true)
            {
                result = true;
            }
        }

        if(result == true)
        {
            canJump = false;
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

}
