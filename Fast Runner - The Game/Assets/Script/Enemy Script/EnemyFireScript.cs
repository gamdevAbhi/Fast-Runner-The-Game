using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireScript : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float fireRate = 0.25f;
    [SerializeField] private float fireSpeed = 2f;
    [SerializeField] private float fireFollowTime = 0.5f;
    [SerializeField] private float maxDistance = 30f;
    [SerializeField] private float cooldown = 0.5f;

    [Header("Others")]
    [SerializeField] private GameObject fireObject;
    [SerializeField] private ParticleSystem[] muzzleObject;

    private float rateTime;
    private bool canFire = false;

    private void Start()
    {
        rateTime = fireRate;
        canFire = false;
    }

    private void Update()
    {
        if(rateTime > 0f && canFire == false)
        {
            rateTime -= Time.deltaTime;
        }
        else if(rateTime <= 0f && canFire == false)
        {
            rateTime = fireRate;
            canFire = true;
        }
    }

    protected internal float Fire(Transform target, Vector3 location, Transform parent)
    {
        if(canFire == true)
        {
            Muzzle();
            
            GameObject fireObj = Instantiate(fireObject, location, Quaternion.identity, parent);

            BulletScript bulletScript = fireObj.GetComponent<BulletScript>();

            bulletScript._target = target;
            bulletScript._speed = fireSpeed;
            bulletScript._maxDistance = maxDistance;
            bulletScript._startPos = location;
            bulletScript._followTime = fireFollowTime;
            bulletScript._instantiater = this.gameObject;

            if(fireFollowTime == 0f)
            {
                bulletScript._direction = transform.forward;
            }

            canFire = false;

            return cooldown;
        }
        else
        {
            return 0f;
        }
    }

    private void Muzzle()
    {
        foreach(ParticleSystem muzzle in muzzleObject)
        {
            muzzle.Play();
        }
    }
}
