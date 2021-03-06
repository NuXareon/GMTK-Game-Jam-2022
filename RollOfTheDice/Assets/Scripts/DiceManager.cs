using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public float rollDiceTimeSeconds = 10.0f;
    public int numDice = 1;
    public GameObject diceUIPrefab1;
    public GameObject diceUIPrefab2;
    public GameObject diceUIPrefab3;
    public GameObject diceUIPrefab4;
    public GameObject diceUIPrefab5;
    public GameObject diceUIPrefab6;

    public List<GameObject> dice = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numDice; i++)
        {
            GameObject newDice = CreateDice(i);
            dice.Add(newDice);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    GameObject CreateDice(int id)
    {
        GameObject newDice = new GameObject();
        newDice.SetActive(false);
        newDice.transform.parent = transform;
        newDice.name = "Dice" + id;
        DiceComponent newDiceComponent = newDice.AddComponent<DiceComponent>();
        newDiceComponent.id = id;
        newDice.SetActive(true);

        return newDice;
    }

    // Returns dice value and removed dice from queue
    // 0 means no dice available
    public int ConsumeDice()
    {
        if (dice.Count == 0)
        {
            return 0;
        }

        GameObject currentDice = dice[0];
        DiceComponent diceComp = currentDice.GetComponent<DiceComponent>();
        int value = diceComp.value;

        if (value != 0)
        {
            dice.RemoveAt(0);
            Destroy(currentDice);
            FillDiceQueue();
        }

        return value;
    }

    void FillDiceQueue()
    {
        int i = 0;
        for (; i < dice.Count; i++)
        {
            dice[i].name = "Dice" + i;
            DiceComponent newDiceComponent = dice[i].GetComponent<DiceComponent>();
            newDiceComponent.id = i;
            newDiceComponent.UpdateUI();
        }
        for (; i < numDice; i++)
        {
            GameObject newDice = CreateDice(i);
            dice.Insert(i, newDice);
        }
    }

    public GameObject GetDicePrefab(int value)
    {
        switch(value)
        {
            case 1: return diceUIPrefab1;
            case 2: return diceUIPrefab2;
            case 3: return diceUIPrefab3;
            case 4: return diceUIPrefab4; 
            case 5: return diceUIPrefab5;
            case 6: return diceUIPrefab6;   
            default: return null;
        }
    }
}
