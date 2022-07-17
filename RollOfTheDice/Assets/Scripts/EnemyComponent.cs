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

    bool flipObject = false;
    float storedXScale;

    GameObject target;
    CharacterController characterController;
    Animator animator;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Use this for initialization
    void Start()
    {
        storedXScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        bool moving = false;
        if (behaviour == EnemyBehaviour.Patrol)
        {
            DoPatrol();
            moving = true;
        }
        else if (behaviour == EnemyBehaviour.Follow)
        {
            moving = DoFollow();
        }

        if (flipObject)
        {
            transform.localScale = new Vector3(-storedXScale, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(storedXScale, transform.localScale.y, transform.localScale.z);
        }

        UpdateAnimation(moving);
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

        flipObject = patrolFlip;
    }

    bool DoFollow()
    {
        Vector3 targetVector = target.transform.position - transform.position;
        float magnitudeSqr = targetVector.sqrMagnitude;
        if (magnitudeSqr < followDistance*followDistance)
        {
            Vector3 targetVectorNormilized = targetVector.normalized;
            characterController.Move(targetVectorNormilized * moveSpeed * Time.deltaTime);

            flipObject = targetVectorNormilized.x > 0;

            return true;
        }

        return false;
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

    void UpdateAnimation(bool moving)
    {
        if (animator)
        {
            animator.SetBool("Moving", moving);
            Vector3 targetVector = target.transform.position - transform.position;
            float magnitudeSqr = targetVector.sqrMagnitude;
            if (magnitudeSqr < 4.0f)
            {
                animator.SetBool("Bite", true);
            }
            else
            {
                animator.SetBool("Bite", false);
            }
        }
    }
}