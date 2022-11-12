using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackScript : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] private float dashPower = 20f;
    [SerializeField] private float reactionForce = 2f;
    [SerializeField] private float minDashVelocity = 3f;
    [SerializeField] private float relativeMass = 4f;
    [SerializeField] private bool isDash = false;
    [SerializeField] private string[] dashAllowLayer;

    [Header("Camera")]
    [SerializeField] private Transform _camera;

    private Rigidbody _rigidbody;
    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision collider)
    {

        LayerMask colliderLayer = collider.gameObject.layer;
        string colliderLayerName = LayerMask.LayerToName(colliderLayer);

        foreach(string layer in dashAllowLayer)
        {
            if(colliderLayerName == layer)
            {
                CalculateDash(collider);
            }
        }
    }

    private void CalculateDash(Collision collider)
    {
        if(_transform.InverseTransformDirection(_rigidbody.velocity).z >= minDashVelocity && isDash == true)
        {
            Rigidbody _colliderRigid = collider.gameObject.GetComponent<Rigidbody>();
            float mass = (_colliderRigid.mass > relativeMass)? _colliderRigid.mass / relativeMass : 1f;
            
            _colliderRigid.velocity = _camera.forward * _camera.InverseTransformDirection(_rigidbody.velocity).z * dashPower / mass;

            _rigidbody.velocity = -_camera.forward * reactionForce;
        }
    }

    protected internal void Status(bool _case)
    {
        isDash = _case;
    }
}
