using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<AbilityType> abilitiesAvailable;
    public GameObject abilityUIPrefab;

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
    }
}
