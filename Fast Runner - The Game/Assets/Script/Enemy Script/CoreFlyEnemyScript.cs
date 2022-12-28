using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyFireScript))]
[RequireComponent(typeof(EnemyHealthScript))]
public class CoreFlyEnemyScript : MonoBehaviour
{
    [Header("Varibles")]
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float rotatingSpeed = 8f;
    [SerializeField] private float lookingSpeed = 1f;
    [SerializeField] private LayerMask layerView;
    [SerializeField] private bool isAlive = true;

    [Header("Target Behavior")]
    [SerializeField] private float maxDistance = 7f;
    [SerializeField] private float limitDistnace = 4f;
    [SerializeField] private float minDistance = 1.5f;
    [SerializeField] private float stableVelocity = 2f;
    [SerializeField] private float stablizePower = 50f;
    [SerializeField] private float maxFollowDistance = 40f;

    [Header("Patrol Behavior")]
    [SerializeField] [Range(0, 360)] private float FOV = 90f;
    [SerializeField] private float patrolDistance = 10f;
    [SerializeField] private float patrolMinDistance = 0.5f;
    [SerializeField] private float targetDistance = 12f;

    [Header("Search")]
    [SerializeField] [Range(0, 1)] private float searchProbality = 0.4f;
    [SerializeField] private float searchTime = 5f;

    [Header("Color")]
    [SerializeField] private Color patrolColor;
    [SerializeField] private Color searchColor;
    [SerializeField] private Color targetColor;
    [SerializeField] private Color disableColor;
    [SerializeField] private float intensity = 1.3f;

    [Header("Particle")]
    [SerializeField] private GameObject destroyParticle;
    [SerializeField] private GameObject destroyParticleSide;
 
    private enum OrderState {Follow, Back, Rotate, Damage};
    private enum BehaviorState {Target, PatrolFront, PatrolBack, Search, Disable};
    private enum PatrolState {Up, Down};
    private float cooldown = 0f;
    private float searchCurrentTime = 0f;
    private bool hasSearch = false;
    
    private EnemyFireScript enemyFireScript;
    private EnemyHealthScript enemyHealthScript;

    [Header("Settings")]
    [SerializeField] private bool drawDirection = true;
    [SerializeField] private OrderState currentState = OrderState.Follow;
    [SerializeField] private BehaviorState currentBehavior = BehaviorState.PatrolFront;
    [SerializeField] private PatrolState currentPatrol = PatrolState.Up;

    [Header("Transform")]
    [SerializeField] private Transform targetTransform;
    [SerializeField] private List<Transform> patrolPoint;
    [SerializeField] private int patrolIndex = 0;
    [SerializeField] private Transform coreTransfrom;
    [SerializeField] private Transform worldBulletParent;
    [SerializeField] private Transform coreBody;

    private void Start()
    {
        if(enemyFireScript == null)
            enemyFireScript = gameObject.GetComponent<EnemyFireScript>();

        if(enemyHealthScript == null)
            enemyHealthScript = gameObject.GetComponent<EnemyHealthScript>();
    }

    private void Update()
    {

        if(StablityCheck(stableVelocity))
        {
            currentState = OrderState.Damage;
        }
        else
        {
            currentState = OrderState.Follow;

            if(isAlive == false)
            {
                BlastParticle();
                Destroy(this.gameObject);
            }
        }

        if(cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }

        if(currentBehavior == BehaviorState.Target && currentState != OrderState.Damage)
        {
            if(cooldown <= 0f)
                TargetCommand();
        }
        else if(currentBehavior != BehaviorState.Disable && currentState != OrderState.Damage)
        {
            NonTargetCommand();
        }

        ChangeColor();
    }

    private void BlastParticle() 
    {
        Instantiate(destroyParticle, transform.position, Quaternion.identity, worldBulletParent);
        Instantiate(destroyParticleSide, transform.position, Quaternion.identity, worldBulletParent);
    }

    private void ChangeColor()
    {
        Material matt = coreBody.GetComponent<Renderer>().material;

        if(currentBehavior == BehaviorState.Target)
        {
            matt.SetColor("_EmissionColor", targetColor * intensity);
        }
        else if(currentBehavior == BehaviorState.Search)
        {
            matt.SetColor("_EmissionColor", searchColor * intensity);
        }
        else if(currentBehavior == BehaviorState.PatrolFront || currentBehavior == BehaviorState.PatrolBack)
        {
            matt.SetColor("_EmissionColor", patrolColor * intensity);
        }
        else
        {
            matt.SetColor("_EmissionColor", disableColor * intensity);
        }
    }
    
    private void NonTargetCommand()
    {
        Vector3 targetDirection = (targetTransform.position - transform.position);
        float dotProduct = Vector3.Dot(transform.forward.normalized, targetDirection.normalized);
        float magnitude =  Mathf.Sqrt(targetDirection.x * targetDirection.x + targetDirection.y * targetDirection.y + targetDirection.z * targetDirection.z);

        float dotFOV = 1 - (FOV / 360f * 2);

        RaycastHit hit;
        Physics.Raycast(transform.position, targetDirection.normalized, out hit, targetDistance, layerView);
        // Debug.DrawLine(transform.position, hit.point, targetColor); // TODO: remove it

        if(dotFOV <= dotProduct && magnitude <= targetDistance && hit.collider.gameObject.layer == targetTransform.gameObject.layer)
        {
            currentBehavior = BehaviorState.Target;
        }
        else if(currentBehavior == BehaviorState.PatrolFront || currentBehavior == BehaviorState.PatrolBack)
        {
            Vector3 direction = patrolPoint[patrolIndex].position - transform.position;
            magnitude = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y + direction.z * direction.z);
            
            if(magnitude > patrolMinDistance && currentBehavior == BehaviorState.PatrolFront)
            {
                LookAtTarget(patrolPoint[patrolIndex].position, lookingSpeed, drawDirection);
            }
            else if(magnitude > patrolDistance && currentBehavior == BehaviorState.PatrolBack)
            {
                currentBehavior = BehaviorState.PatrolFront;
                hasSearch = false;
            }
            else if(magnitude <= patrolMinDistance)
            {
                float value = Random.Range(0f, 1f);

                if(value <= searchProbality && hasSearch == false)
                {
                    currentBehavior = BehaviorState.Search;
                    hasSearch = true;
                }
                else if(patrolPoint.Count > 1)
                {
                    if(currentPatrol == PatrolState.Up)
                    {
                        if(patrolIndex < patrolPoint.Count - 1) 
                            patrolIndex = patrolIndex + 1;
                        else
                            currentPatrol = PatrolState.Down;
                    }
                    else
                    {
                        if(patrolIndex > 0) 
                            patrolIndex = patrolIndex - 1;
                        else 
                            currentPatrol = PatrolState.Up;
                    }

                    currentBehavior = BehaviorState.PatrolFront;
                    hasSearch = false;
                }
            }

            FollowTarget(transform.forward, followSpeed);
        }
        else if(currentBehavior == BehaviorState.Search && searchCurrentTime < searchTime)
        {
            searchCurrentTime += Time.deltaTime;

            LookAtTarget(transform.right + transform.position, lookingSpeed, drawDirection);
        }
        else if(currentBehavior == BehaviorState.Search && searchCurrentTime >= searchTime)
        {
            searchCurrentTime = 0f;

            currentBehavior = BehaviorState.PatrolBack;
        }
    }

    private void TargetCommand()
    {
        bool isReady = false;

        Vector3 distance = (targetTransform.position - this.transform.position);
        float magnitude = Mathf.Sqrt(distance.x * distance.x + distance.y * distance.y + distance.z * distance.z);     

        if(magnitude >= maxFollowDistance)
        {
            transform.position = patrolPoint[patrolIndex].position;

            currentBehavior = BehaviorState.PatrolFront;
        }

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

        LookAtTarget(targetTransform.position, lookingSpeed, drawDirection);

        Order(currentState, magnitude);

        if(isReady && currentBehavior != BehaviorState.Search)
            cooldown = enemyFireScript.Fire(targetTransform, coreTransfrom.position, worldBulletParent);
    }

    private void GiveDamage(float damage)
    {
        isAlive =  !enemyHealthScript.DamageTake(damage);
        currentBehavior = BehaviorState.Target;
    }

    private bool StablityCheck(float minVelocity)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        Vector3 localVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.velocity);

        if(CheckVelocity(localVelocity.x, minVelocity) || CheckVelocity(localVelocity.y, minVelocity) || CheckVelocity(localVelocity.z, minVelocity))
        {
            localVelocity -= localVelocity / (stablizePower * Time.deltaTime * 10f);

            rigidbody.velocity = rigidbody.transform.TransformDirection(localVelocity);
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
        }

        bool result = (rigidbody.velocity == Vector3.zero)? false : true;

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

    protected internal void Initialized(Transform patrol, Transform target, Transform parentBullet)
    {
        foreach(Transform point in patrol)
        {
            patrolPoint.Add(point);
        }

        targetTransform = target;

        worldBulletParent = parentBullet;
    }
}
