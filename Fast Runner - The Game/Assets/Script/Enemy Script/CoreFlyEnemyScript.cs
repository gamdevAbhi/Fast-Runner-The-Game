using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyFireScript))]
public class CoreFlyEnemyScript : MonoBehaviour
{
    [Header("Varibles")]
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float rotatingSpeed = 8f;
    [SerializeField] private float lookingSpeed = 1f;
    [SerializeField] private float maxDistance = 7f;
    [SerializeField] private float limitDistnace = 4f;
    [SerializeField] private float minDistance = 1.5f;
    [SerializeField] private float stableVelocity = 2f;
    [SerializeField] private float stablizePower = 50f;

    private enum OrderState {Follow, Back, Rotate, Damage};
    private enum BehaviorState {Target, Search};
    private float cooldown = 0f;
    
    private EnemyFireScript enemyFireScript;

    [Header("Settings")]
    [SerializeField] private bool drawDirection = true;
    [SerializeField] private bool isActive = false;
    [SerializeField] private OrderState currentState = OrderState.Follow;
    [SerializeField] private BehaviorState currentBehavior = BehaviorState.Target;

    [Header("Transform")]
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform coreTransfrom;
    [SerializeField] private Transform worldBulletParent;

    private void Start()
    {
        if(enemyFireScript == null)
            enemyFireScript = gameObject.GetComponent<EnemyFireScript>();
    }

    private void Update()
    {
        if(cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            Command();
        }

        if(isActive == true)
            LookBehavior(currentBehavior);
    }
    private void Command()
    {
        bool isReady = false;

        Vector3 distance = (targetTransform.position - this.transform.position);
        float magnitude = Mathf.Sqrt(distance.x * distance.x + distance.y * distance.y + distance.z * distance.z);

        if(isActive == true)
        {            
            if(StablityCheck(stableVelocity))
            {
                currentState = OrderState.Damage;
            }
            else
            {
                if(magnitude > limitDistnace && currentState == OrderState.Follow)
                {
                    currentState = OrderState.Follow;

                    if(magnitude <= maxDistance)
                        isReady = true;
                }
                else if(magnitude > maxDistance && currentState == OrderState.Rotate)
                {
                    currentState = OrderState.Follow;
                }
                else if(magnitude > minDistance)
                {
                    currentState = OrderState.Rotate;
                    isReady = true;
                }
                else if(magnitude <= minDistance && currentState == OrderState.Rotate)
                {
                    currentState = OrderState.Back;
                    isReady = true;
                }
            }

            Order(currentState, magnitude);

            if(isReady && currentBehavior != BehaviorState.Search)
                cooldown = enemyFireScript.Fire(targetTransform, coreTransfrom.position, worldBulletParent);
        }
    }
    private bool StablityCheck(float minVelocity)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        Vector3 localVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.velocity);

        bool result = true;

        if(CheckVelocity(localVelocity.x, minVelocity) || CheckVelocity(localVelocity.y, minVelocity) || CheckVelocity(localVelocity.z, minVelocity))
        {
            localVelocity -= localVelocity / (stablizePower * Time.deltaTime * 10f);

            rigidbody.velocity = rigidbody.transform.TransformDirection(localVelocity);
        }
        else
        {
            result = false;

            rigidbody.velocity = new Vector3(0f, 0f, 0f);
        }

        return result;
    }

    private bool CheckVelocity(float velocityAxis, float range)
    {
        bool result = (velocityAxis < -range && velocityAxis != 0)? true : false;
        
        return result;
    }

    private void Order(OrderState currentState, float magnitude)
    {
        if(currentState == OrderState.Follow)
        {
            FollowTarget(transform.forward, followSpeed);
        }
        else if(currentState == OrderState.Rotate)
        {
           RotateAroundTarget(targetTransform.position, rotatingSpeed, magnitude);
        }
        else if(currentState == OrderState.Back)
        {
            FollowTarget(-transform.forward, followSpeed);
        }
    }

    private void LookBehavior(BehaviorState currentBehavior)
    {
        LookAtTarget(targetTransform.position, lookingSpeed, drawDirection);
    }

    private void FollowTarget(Vector3 direction, float speed)
    {
        this.transform.position += direction * speed * Time.deltaTime;
    }

    private void LookAtTarget(Vector3 targetPos, float speed, bool drawDirection)
    {
        Vector3 targetDirection = targetPos - transform.position;
        Vector3 currentLookDirection = transform.forward;
        Vector3 SpeedDirection = targetDirection.normalized * speed * Time.deltaTime + currentLookDirection;

        if(drawDirection)
        {
            Debug.DrawRay(transform.position, currentLookDirection * 10, Color.green);
            Debug.DrawRay(transform.position, targetDirection.normalized * 10, Color.red);
            Debug.DrawRay(transform.position, SpeedDirection.normalized * 10, Color.blue);
        }
        
        transform.rotation = Quaternion.LookRotation(SpeedDirection, Vector3.up);
    }

    private void RotateAroundTarget(Vector3 targetPos, float speed, float radius)
    {
        this.transform.RotateAround(targetPos, transform.up, speed / radius * Time.deltaTime * 40f);
    }
}
