using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISetBackgroundComponent : MonoBehaviour
{
    public float diceTimeSeconds = 1f;
    public GameObject background;

    float timer = 0f; 

    public void SetAbilityDice(int value)
    {
        background.SetActive(true);

        TMP_Text UIText = background.GetComponentInChildren<TMP_Text>();
        UIText.text = value.ToString();

        timer = diceTimeSeconds;
    }

    void Awake()
    {

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
                background.SetActive(false);
            }
        }
    }
}
