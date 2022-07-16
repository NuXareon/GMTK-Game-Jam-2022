using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    public float maxSidewaysSpeed = 10f;
    public float sidewaysAcceleration = 2.0f;
    public float jumpStrength = 10f;

    // Fake a dice roll for the dice. 0 = no override.
    public int debugDiceRoll = 0;

    Rigidbody rigidBody;

    float sidewaysInput = 0.0f;
    bool jump = false;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sidewaysInput = -Input.GetAxis("Horizontal");

        // #TEMP
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        // Jump
        Vector3 groundDirection = Physics.gravity.normalized;
        if (jump)
        {
            Vector3 newVelocity = rigidBody.velocity;

            // #Apply dice properly
            Vector3 jumpSpeed = (-groundDirection);

            if (debugDiceRoll > 0)
            {
                jumpSpeed *= Mathf.Sqrt(2 * Physics.gravity.magnitude * debugDiceRoll * jumpStrength);
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
            jump = false;
        }

        // Horizontal movement
        Vector3 groundDirectionPositive = new Vector3(Mathf.Abs(groundDirection.x), Mathf.Abs(groundDirection.y), 0.0f);
        Vector3 right = Vector3.Cross(Vector3.forward, groundDirectionPositive);
        float currentSidewaysSpeed = rigidBody.velocity.x * right.x + rigidBody.velocity.y * right.y;
        Debug.Log(currentSidewaysSpeed);
        Debug.Log(sidewaysInput);
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
            if (Mathf.Abs(currentSidewaysSpeed) != 0.0f)
            {
                rigidBody.velocity -= right * Mathf.Min(maxSidewaysSpeed, currentSidewaysSpeed);
            }
        }
/*
        float currentVerticalSpeed = mRigidBody.velocity.x * -groundDirection.x + mRigidBody.velocity.y * -groundDirection.y;
        if (Mathf.Abs(currentVerticalSpeed) > maxVerticalSpeed)
        {
            mRigidBody.velocity -= groundDirection * (Mathf.Abs(currentVerticalSpeed) - maxVerticalSpeed);
        }
*/
    }
}
