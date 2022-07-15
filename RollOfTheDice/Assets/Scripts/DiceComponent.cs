using System.Collections;
using UnityEngine;
using TMPro;

public class DiceComponent : MonoBehaviour
{
    public int value = 0;
    public int id = 0;

    [HideInInspector]
    public GameObject UI;

    DiceManager manager;

    void Awake()
    {
        GameObject UILayer = GameObject.FindGameObjectWithTag("UI");
        GameObject gameLogic = GameObject.FindGameObjectWithTag("GameController");
        manager = gameLogic.GetComponent<DiceManager>();

        UI = Instantiate(manager.diceUIPrefab, UILayer.transform);
        RectTransform UITransform = UI.GetComponent<RectTransform>();
        // #OverrideSorting?

        Vector2 UIPosition = new Vector2(50 + ((float)id * (125.0f + 5.0f)), -55);
        UITransform.anchoredPosition = UIPosition;

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
    }

    IEnumerator RollDice()
    {
        yield return new WaitForSecondsRealtime(manager.rollDiceTimeSeconds);

        value = UnityEngine.Random.Range(1, 7);
        UpdateUI();
    }

    void UpdateUI()
    {
        TMP_Text UIText = UI.GetComponentInChildren<TMP_Text>();
        UIText.text = value.ToString();
    }
}