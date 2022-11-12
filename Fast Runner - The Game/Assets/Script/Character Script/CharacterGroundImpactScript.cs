using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterGroundImpactScript : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] private bool isImpact = false;
    [SerializeField] private float limitStable = 8f;
    private Rigidbody _rigidbody;
    private float previousVelocityY = 0f;
    private float impactVelocty = 0f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(_rigidbody.velocity.y == 0f && previousVelocityY <= -limitStable)
        {
            isImpact = true;
            impactVelocty = previousVelocityY;
            previousVelocityY = 0f;
        }
        else
        {
            previousVelocityY = _rigidbody.velocity.y;
        }
    }

    protected internal bool IsImpact()
    {
        if(isImpact)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected internal float Impact()
    {
        isImpact = false;
        return Mathf.Abs(impactVelocty);
    }
}
