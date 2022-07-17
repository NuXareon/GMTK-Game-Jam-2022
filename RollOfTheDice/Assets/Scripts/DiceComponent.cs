using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiceComponent : MonoBehaviour
{
    public int value = 0;
    public int id = 0;

    List<GameObject> UIDice = new List<GameObject>();

    DiceManager manager;

    float diceRollTime = 0f;

    void Awake()
    {
        GameObject UILayer = GameObject.FindGameObjectWithTag("UI");
        GameObject gameLogic = GameObject.FindGameObjectWithTag("GameController");
        manager = gameLogic.GetComponent<DiceManager>();

        GameObject dice1 = Instantiate(manager.diceUIPrefab1, UILayer.transform);
        dice1.SetActive(false);
        UIDice.Add(dice1);

        GameObject dice2 = Instantiate(manager.diceUIPrefab2, UILayer.transform);
        dice2.SetActive(false);
        UIDice.Add(dice2);

        GameObject dice3 = Instantiate(manager.diceUIPrefab3, UILayer.transform);
        dice3.SetActive(false);
        UIDice.Add(dice3);

        GameObject dice4 = Instantiate(manager.diceUIPrefab4, UILayer.transform);
        dice4.SetActive(false);
        UIDice.Add(dice4);

        GameObject dice5 = Instantiate(manager.diceUIPrefab5, UILayer.transform);
        dice5.SetActive(false);
        UIDice.Add(dice5);

        GameObject dice6 = Instantiate(manager.diceUIPrefab6, UILayer.transform);
        dice6.SetActive(false);
        UIDice.Add(dice6);

        UpdateUI();
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(RollDice());
    }

    // Update is called once per frame
    void Update()
    {
        if (value == 0)
        {
            diceRollTime += Time.deltaTime;
            UpdateUI();
        }
    }

    void OnDestroy()
    {
        foreach (GameObject dice in UIDice)
        {
            Destroy(dice);
        }
    }

    IEnumerator RollDice()
    {
        value = 0;
        diceRollTime = 0f;

        yield return new WaitForSecondsRealtime(manager.rollDiceTimeSeconds);

        value = UnityEngine.Random.Range(1, 7);
        UpdateUI();
    }

    public void UpdateUI()
    {
        // #Improve pos calculation
        Vector2 UIPosition = new Vector2(70 + ((float)id * (125.0f + 15.0f)), -65);

        if (value == 0)
        {
            // Rolling dice
            if (diceRollTime > 0.1f)
            {
                int tmpValue = UnityEngine.Random.Range(1, 7);

                for (int i = 0; i < UIDice.Count; i++)
                {
                    GameObject currentDice = UIDice[i];
                    RectTransform UITransform = currentDice.GetComponent<RectTransform>();
                    UITransform.anchoredPosition = UIPosition;

                    if (i == tmpValue - 1)
                    {
                        currentDice.SetActive(true);
                    }
                    else
                    {
                        currentDice.SetActive(false);
                    }
                }

                diceRollTime = 0f;
            }
            
            return;
        }

        for (int i = 0; i < UIDice.Count; i++)
        {
            GameObject currentDice = UIDice[i];
            RectTransform UITransform = currentDice.GetComponent<RectTransform>();
            UITransform.anchoredPosition = UIPosition;

            if (i == value - 1)
            {
                currentDice.SetActive(true);
            }
            else
            {
                currentDice.SetActive(false);
            }
        }
    }
}