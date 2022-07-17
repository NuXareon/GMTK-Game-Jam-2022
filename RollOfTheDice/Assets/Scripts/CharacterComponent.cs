using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToonBoom.Harmony;

public class CharacterComponent : MonoBehaviour
{
    public float maxSidewaysSpeed = 10f;
    public float sidewaysAcceleration = 2.0f;
    public float dashStrength = 10f;
    public float dashDurationSeconds = 0.2f;
    public float jumpStrength = 10f;
    public float invulnerabilityTimeSeconds = 1f;

    Rigidbody rigidBody;
    GameFlow gameFlow;
    Animator animator;
    float originalLocalScaleX;

    float sidewaysInput = 0.0f;
    Vector3 orientation = new Vector3(1f, 0f, 0f);

    // Fake a dice roll for the dice. 0 = no override.
    public int debugDiceRoll = 0;
    int jumpDiceRoll = 0;
    int dashDiceRoll = 0;
    int attackDiceRoll = 0;

    bool isDashing = false;
    float dashTime = 0.0f;

    float invulnerability = 0.0f;
    float invulnerabilityInvisibility = 0.0f;

    bool isDead = false;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        GameObject gameLogic = GameObject.FindGameObjectWithTag("GameController");
        gameFlow = gameLogic.GetComponent<GameFlow>();
    }

    // Start is called before the first frame update
    void Start()
    {
        originalLocalScaleX = animator.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            sidewaysInput = 0.0f;
            CancelDash();
            UpdateAnimator(false);
            return;
        }

        if (isDashing)
        {
            dashTime += Time.deltaTime;
        }

        if (invulnerability > 0.0f)
        {
            invulnerability -= Time.deltaTime;

            if (invulnerability <= 0.0f)
            {
                invulnerability = 0.0f;

                GetComponentInChildren<HarmonyRenderer>().enabled = true;
            }
            else
            {
                invulnerabilityInvisibility += Time.deltaTime;
                if (invulnerabilityInvisibility > 0.15f)
                {
                    invulnerabilityInvisibility = 0.0f;
                    GetComponentInChildren<HarmonyRenderer>().enabled = !GetComponentInChildren<HarmonyRenderer>().enabled;
                }
            }
        }

        sidewaysInput = -Input.GetAxis("Horizontal");

        if (sidewaysInput != 0.0f)
        {
            Vector3 groundDirection = Physics.gravity.normalized;
            Vector3 groundDirectionPositive = new Vector3(Mathf.Abs(groundDirection.x), Mathf.Abs(groundDirection.y), 0.0f);
            Vector3 right = Vector3.Cross(Vector3.forward, groundDirectionPositive);
            orientation = (right * sidewaysInput).normalized;
        }

        bool isAttacking = false;
        if (attackDiceRoll > 0)
        {
            // #Scale with dice
            float attackDistance = 1.5f + attackDiceRoll*0.15f;
            RaycastHit[] hits = Physics.BoxCastAll(transform.position, transform.localScale / 2, orientation, Quaternion.identity, attackDistance);

            foreach (RaycastHit hit in hits)
            {
                EnemyComponent enemy = hit.transform.gameObject.GetComponent<EnemyComponent>();
                HealthComponent hp = hit.transform.gameObject.GetComponent<HealthComponent>();
                if (enemy && hp)
                {
                    bool isDead = hp.TakeDamage(attackDiceRoll);
                    if (isDead)
                    {
                        enemy.Kill();
                    }
                }

                Debug.Log("Hit: " + hit.transform.gameObject.name);
            }

            isAttacking = true;
            attackDiceRoll = 0;
        }

        if (orientation.x < 0f)
        {
            animator.transform.localScale = new Vector3(-originalLocalScaleX, animator.transform.localScale.y, animator.transform.localScale.z);
        }
        else
        {
            animator.transform.localScale = new Vector3(originalLocalScaleX, animator.transform.localScale.y, animator.transform.localScale.z);
        }

        UpdateAnimator(isAttacking);
    }

    void FixedUpdate()
    {
        Vector3 groundDirection = Physics.gravity.normalized;
        Vector3 groundDirectionPositive = new Vector3(Mathf.Abs(groundDirection.x), Mathf.Abs(groundDirection.y), 0.0f);
        Vector3 right = Vector3.Cross(Vector3.forward, groundDirectionPositive);
        float currentSidewaysSpeed = rigidBody.velocity.x * right.x + rigidBody.velocity.y * right.y;

        // Jump
        if (jumpDiceRoll > 0)
        {
            if (isDashing)
            {
                CancelDash();
            }

            Vector3 newVelocity = rigidBody.velocity;

            Vector3 jumpSpeed = (-groundDirection);
            if (debugDiceRoll > 0)
            {
                jumpSpeed *= Mathf.Sqrt(2 * Physics.gravity.magnitude * debugDiceRoll * jumpStrength);
            }
            else
            {
                jumpSpeed *= Mathf.Sqrt(2 * Physics.gravity.magnitude * jumpDiceRoll * jumpStrength);
            }

            if (jumpSpeed.x != 0.0f)
            {
                newVelocity.x = jumpSpeed.x;
            }
            if (jumpSpeed.y != 0.0f)
            {
                newVelocity.y = jumpSpeed.y;
            }

            rigidBody.velocity = newVelocity;
            jumpDiceRoll = 0;
        }

        // Dash
        if (dashDiceRoll > 0)
        {
            Vector3 newVelocity = rigidBody.velocity;

            newVelocity.y = 0.0f; // Cancel vertical movement

            Vector3 dashSpeed = orientation * dashStrength;
            if (debugDiceRoll > 0)
            {
                dashSpeed *= debugDiceRoll;
            }
            else
            {
                dashSpeed *= dashDiceRoll;
            }

            newVelocity.x = dashSpeed.x;
            rigidBody.velocity = newVelocity;
            rigidBody.useGravity = false;

            dashDiceRoll = 0;
            isDashing = true;
            dashTime = 0;
        }

        if (isDashing)
        {
            if (dashTime >= dashDurationSeconds)
            {
                CancelDash();
            }
            else
            {
                // Stop processing physics 
                return;
            }
        }

        // Horizontal movement
        if (sidewaysInput != 0.0f)
        {
            int layerMask = 1 << 3;
            RaycastHit hitInfo;
            bool sidewaysHit = Physics.Raycast(transform.position, (right * sidewaysInput).normalized, out hitInfo, transform.localScale.x + 0.01f, layerMask);
            if (!sidewaysHit)
            {
                // Snap speed if we are turning
                if (sidewaysInput < 0.0f && currentSidewaysSpeed > 0.0f
                    || sidewaysInput > 0.0f && currentSidewaysSpeed < 0.0f)
                {
                    Vector3 acceleration;
                    if (sidewaysInput > 0.0f)
                    {
                        acceleration = right * Mathf.Abs(currentSidewaysSpeed)*0.8f;
                    }
                    else
                    {
                        acceleration = -right * Mathf.Abs(currentSidewaysSpeed)*0.8f;
                    }
                    rigidBody.velocity += acceleration;
                }

                // Don't accelerate if we are already past max speed
                if (sidewaysInput < 0.0f && currentSidewaysSpeed > -maxSidewaysSpeed
                 || sidewaysInput > 0.0f && currentSidewaysSpeed < maxSidewaysSpeed)
                {
                    Vector3 acceleration = right * sidewaysInput * sidewaysAcceleration;
                    Vector3 newVelocity = rigidBody.velocity + acceleration;
                    float newVelocityValue = newVelocity.x * right.x + newVelocity.y * right.y;
                    if (Mathf.Abs(newVelocityValue) > maxSidewaysSpeed)
                    {
                        float diffSpeed = maxSidewaysSpeed - Mathf.Abs(currentSidewaysSpeed);
                        if (sidewaysInput > 0.0f)
                        {
                            acceleration = right * diffSpeed;
                        }
                        else
                        {
                            acceleration = -right * diffSpeed;
                        }
                    }
                    rigidBody.velocity += acceleration;
                }
            }
        }
        else
        {
            // Stop when nothing is pressed
            if (Mathf.Abs(currentSidewaysSpeed) != 0.0f)
            {
                rigidBody.velocity -= right * currentSidewaysSpeed;
            }
        }
        
        // Cap horizontal speed
        if (Mathf.Abs(currentSidewaysSpeed) > maxSidewaysSpeed)
        {
            float speedDiff = Mathf.Abs(currentSidewaysSpeed) - maxSidewaysSpeed;
            if (currentSidewaysSpeed < 0.0f)
            {
                speedDiff = -speedDiff;
            }
            rigidBody.velocity -= right * speedDiff;
        }
        

        //# cap vertical speed
        /*
                float currentVerticalSpeed = mRigidBody.velocity.x * -groundDirection.x + mRigidBody.velocity.y * -groundDirection.y;
                if (Mathf.Abs(currentVerticalSpeed) > maxVerticalSpeed)
                {
                    mRigidBody.velocity -= groundDirection * (Mathf.Abs(currentVerticalSpeed) - maxVerticalSpeed);
                }
        */
    }

    void OnCollisionEnter(Collision collision)
    {
        EnemyComponent enemy = collision.gameObject.GetComponent<EnemyComponent>();
        ContactPoint[] contacts = new ContactPoint[1];
        int count = collision.GetContacts(contacts);
        Vector3 collisionNormal = Vector3.zero;
        if (count > 0)
        {
            collisionNormal = collision.contacts[0].normal;
        }
        DoDamage(enemy, collisionNormal);
    }

    public void DoDamage(EnemyComponent enemy, Vector3 collisionNormal)
    {
        if (invulnerability > 0f)
        {
            return;
        }

        if (isDead)
        {
            return;
        }

        if (enemy != null)
        {
            bool dead = GetComponent<HealthComponent>().TakeDamage(enemy.damage);
            invulnerability = invulnerabilityTimeSeconds;
            invulnerabilityInvisibility = 0.0f;

            if (dead)
            {
                isDead = true;
                gameFlow.OnPlayerDeath();
            }
            else
            {
                if (!collisionNormal.Equals(Vector3.zero))
                {
                    if (isDashing)
                    {
                        CancelDash();
                    }

                    rigidBody.velocity = collisionNormal * 10f;
                }
            }
        }
    }

    void CancelDash()
    {
        isDashing = false;
        dashTime = 0f;
        rigidBody.useGravity = true;
    }


    public void DoJump(int diceValue)
    {
        if (isDead)
        {
            return;
        }

        jumpDiceRoll = diceValue;
    }

    public void DoDash(int diceValue)
    {
        if (isDead)
        {
            return;
        }

        dashDiceRoll = diceValue;
    }

    public void DoAttack(int diceValue)
    {
        if (isDead)
        {
            return;
        }

        attackDiceRoll = diceValue;
    }    

    void UpdateAnimator(bool isAttacking)
    {
        Vector3 groundDirection = Physics.gravity.normalized;
        Vector3 groundDirectionPositive = new Vector3(Mathf.Abs(groundDirection.x), Mathf.Abs(groundDirection.y), 0.0f);
        Vector3 right = Vector3.Cross(Vector3.forward, groundDirectionPositive);
        float currentSidewaysSpeed = rigidBody.velocity.x * right.x + rigidBody.velocity.y * right.y;

        int layerMask = 1 << 3;
        bool isGrounded = Physics.Raycast(transform.position, groundDirection, transform.localScale.x + 0.1f, layerMask);

        animator.SetBool("IsDashing", isDashing);
        animator.SetBool("IsTakingDamage", isDead ? false : invulnerability > 0.0f);
        animator.SetBool("IsIdle", currentSidewaysSpeed == 0.0f && sidewaysInput == 0);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsAttacking", isAttacking);
        animator.SetBool("IsDead", isDead);
    }
}
