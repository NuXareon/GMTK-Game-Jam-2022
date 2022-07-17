using System.Collections;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int health = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Return true if HP reaches zero
    public bool TakeDamage(int damage)
    {
        health -= damage;
        return health <= 0;
    }
}