using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISetBackgroundComponent : MonoBehaviour
{
    public float diceTimeSeconds = 1f;
    public GameObject background;

    float timer = 0f; 
    DiceManager diceManager;

    void Awake()
    {
        GameObject gameLogic = GameObject.FindGameObjectWithTag("GameController");
        diceManager = gameLogic.GetComponent<DiceManager>();
    }

    public void SetAbilityDice(int value)
    {
        if (background)
        {
            Destroy(background);
        }

        background = Instantiate(diceManager.GetDicePrefab(value), transform);
        RectTransform UITransform = background.GetComponent<RectTransform>();
        Vector2 UIPosition = new Vector2(55f, -45f);
        UITransform.anchoredPosition = UIPosition;
        background.transform.localScale *= 0.5f;

        timer = diceTimeSeconds;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                timer = 0f;
                Destroy(background);
            }
        }
    }
}
