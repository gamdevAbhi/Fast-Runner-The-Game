using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private GameObject instantiater;
    private Transform target;
    private Vector3 startPos;
    private Vector3 direction;    
    private float maxDistance;
    private float speed;
    private float followTime;

    protected internal float _speed
    {
        set
        {
            this.speed = value;
        }
    }

    protected internal float _followTime
    {
        set
        {
            this.followTime = value;
        }
    }

    protected internal float _maxDistance
    {
        set
        {
            this.maxDistance = value;
        }
    }

    protected internal Transform _target
    {
        set
        {
            this.target = value;
        }
    }

    protected internal Vector3 _startPos
    {
        set
        {
            this.startPos = value;
        }
    }

    protected internal Vector3 _direction
    {
        set
        {
            this.direction = value;
        }
    }

    protected internal GameObject _instantiater
    {
        set
        {
            this.instantiater = value;
        }
    }

    private void Update()
    {
        if(followTime > 0f)
        {
            followTime -= Time.deltaTime;
            direction = (target.position - transform.position).normalized;
        }

        Vector3 distance = (startPos - this.transform.position);
        float magnitude = Mathf.Sqrt(distance.x * distance.x + distance.y * distance.y + distance.z * distance.z);

        if(magnitude <= maxDistance)
            transform.position += direction * speed * Time.deltaTime;
        //else
            // BulletDestroy(null);
    }

    private void OnCollisionEnter(Collision collider)
    {
        BulletDestroy(collider.gameObject);
    }

    private void BulletDestroy(GameObject target)
    {
        if(target != instantiater)
        {
            Destroy(this.gameObject);
        }
    }
}
