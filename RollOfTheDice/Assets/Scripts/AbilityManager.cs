using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<AbilityType> abilitiesAvailable;

    public GameObject jumpUIPrefab;
    public GameObject dashUIPrefab;
    public GameObject attackUIPrefab;

    List<IAbility> abilities = new List<IAbility>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (AbilityType type in abilitiesAvailable)
        {
            if (type == AbilityType.Jump)
            {
                abilities.Add(new JumpAbility(abilities.Count));
            }
            else if (type == AbilityType.Dash)
            {
                abilities.Add(new DashAbility(abilities.Count));
            }
            else if (type == AbilityType.Attack)
            {
                abilities.Add(new AttackAbility(abilities.Count));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            foreach (IAbility ability in abilities)
            {
                if (ability.type == AbilityType.Jump)
                {
                    ability.DoAction();
                    break;
                }
            }
        }

        if (Input.GetButtonDown("Dash"))
        {
            foreach (IAbility ability in abilities)
            {
                if (ability.type == AbilityType.Dash)
                {
                    ability.DoAction();
                    break;
                }
            }
        }

        if (Input.GetButtonDown("Attack"))
        {
            foreach (IAbility ability in abilities)
            {
                if (ability.type == AbilityType.Attack)
                {
                    ability.DoAction();
                    break;
                }
            }
        }
    }
}
