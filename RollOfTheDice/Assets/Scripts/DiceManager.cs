using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public float rollDiceTimeSeconds = 10.0f;
    public int numDice = 1;
    public GameObject diceUIPrefab;

    public List<GameObject> dice = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numDice; i++)
        {
            GameObject newDice = new GameObject();
            newDice.SetActive(false);
            newDice.transform.parent = transform;
            newDice.name = "Dice" + i;
            DiceComponent newDiceComponent = newDice.AddComponent<DiceComponent>();
            newDiceComponent.id = i;
            newDice.SetActive(true);

            dice.Add(newDice);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
