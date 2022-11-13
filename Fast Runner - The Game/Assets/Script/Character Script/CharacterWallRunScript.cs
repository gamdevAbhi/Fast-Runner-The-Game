using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWallRunScript : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Transform posOriginRightCenter;
    [SerializeField] private Transform posOriginLeftCenter;
    [SerializeField] private Transform _camera;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask[] wallLayer;

    [Header("Parameter")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rayDistance = 0.1f;
    [SerializeField] private bool isWallTouch = false;

    private Vector3 forwardDirection;
    private Rigidbody _rigidbody;
    private GameObject currentWall;

    private void Start( )
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        foreach(LayerMask layer in wallLayer)
        {
            if(layer == (layer | (1 << collider.gameObject.layer)))
            {
                isWallTouch = CanRun(layer, collider.transform.gameObject);
                break;
            }
        }
    }
    
    private void OnTriggerStay(Collider collider)
    {
        if(isWallTouch == false)
        {
            foreach(LayerMask layer in wallLayer)
            {
                if(layer == (layer | (1 << collider.gameObject.layer)))
                {
                    isWallTouch = CanRun(layer, collider.transform.gameObject);
                    break;
                }
            }
        }
        else
        {
            forwardDirection = (Vector3.Dot(collider.transform.forward, _camera.forward) >= 0f)? collider.transform.forward : -collider.transform.forward;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(currentWall == collider.transform.gameObject && isWallTouch == true)
        {
            currentWall = null;
            isWallTouch = false;
            forwardDirection = Vector3.zero;
        }
    }

    private bool CanRun(LayerMask layer, GameObject wall)
    {
        bool result = false;

        Ray rightRayCenter = new Ray(posOriginRightCenter.position, posOriginRightCenter.right);

        Ray leftRayCenter = new Ray(posOriginLeftCenter.position, -posOriginLeftCenter.right);

        RaycastHit hit;

        if(Physics.Raycast(rightRayCenter, out hit, rayDistance, layer) || Physics.Raycast(leftRayCenter, out hit, rayDistance, layer))
        {
            forwardDirection = (Vector3.Dot(hit.transform.forward, _camera.forward) >= 0f)? hit.transform.forward : -hit.transform.forward;
            currentWall = wall;
            result = true;
        }

        return result;
    }

    protected internal bool CheckWallRun()
    {
        return isWallTouch;
    }
    
    protected internal void WallRun()
    {
        _rigidbody.velocity = forwardDirection * speed;
    }
}
