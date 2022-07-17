using System.Collections;
using UnityEngine;

public enum EnemyBehaviour
{
    Static,
    Patrol,
    Follow
}

public class EnemyComponent : MonoBehaviour
{
    public int damage = 1;
    public EnemyBehaviour behaviour = EnemyBehaviour.Static;
    public float followDistance = 10.0f;
    public float moveSpeed = 2.0f;
    public bool patrolFlip = false;

    GameObject target;
    CharacterController characterController;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        characterController = GetComponent<CharacterController>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (behaviour == EnemyBehaviour.Patrol)
        {
            DoPatrol();
        }
        else if (behaviour == EnemyBehaviour.Follow)
        {
            DoFollow();
        }
    }

    void DoPatrol()
    {
        Vector3 direction = Vector3.right;
        if (patrolFlip)
        {
            direction = -direction;
        }

        int layerMask = 1 << 3;
        bool hitWall = Physics.Raycast(transform.position, direction, transform.localScale.x + 0.01f, layerMask);
        if (hitWall)
        {
            patrolFlip = !patrolFlip;
        }
        else
        {
            characterController.Move(direction * moveSpeed * Time.deltaTime);
        }
    }

    void DoFollow()
    {
        Vector3 targetVector = target.transform.position - transform.position;
        float magnitudeSqr = targetVector.sqrMagnitude;
        if (magnitudeSqr < followDistance*followDistance)
        {
            Vector3 targetVectorNormilized = targetVector.normalized;
            characterController.Move(targetVectorNormilized * moveSpeed * Time.deltaTime);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CharacterComponent player = hit.gameObject.GetComponent<CharacterComponent>();
        if (player)
        {
            player.DoDamage(this, -hit.normal);
        }
    }

    public void Kill()
    {
        // #Do dead animation
        // wait for anim
        Destroy(gameObject);
    }
}