using System.Collections;
using UnityEngine;
using TMPro;

public enum AbilityType
{
    Jump,
    Dash,
    Attack
}

class AbilityUtils
{
    public static void UpdateUI(GameObject UI, int id, AbilityType type)
    {
        // #Make this good and scalable
        RectTransform UITransform = UI.GetComponent<RectTransform>();
        Vector2 UIPosition = new Vector2(-700f + (float)(id+1) * 280f, 50);
        UITransform.anchoredPosition = UIPosition;

        TMP_Text UIText = UI.GetComponentInChildren<TMP_Text>();
        UIText.text = type.ToString();
    }
}

interface IAbility
{ 
    public AbilityType type
    {
        get;
    }
    public bool DoAction();
}

public class JumpAbility : IAbility
{
    CharacterComponent characterComponent;
    AbilityManager abilityManager;
    DiceManager diceManager;
    GameObject UI;
    int id;
    int lastDiceValue = 0;

    public JumpAbility(int newId)
    {
        GameObject UILayer = GameObject.FindGameObjectWithTag("UI");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        characterComponent = player.GetComponent<CharacterComponent>();
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        abilityManager = controller.GetComponent<AbilityManager>();
        diceManager = controller.GetComponent<DiceManager>();

        id = newId;
        UI = Object.Instantiate(abilityManager.abilityUIPrefab, UILayer.transform);

        AbilityUtils.UpdateUI(UI, id, type);
    }

    public AbilityType type
    {
        get { return AbilityType.Jump; }
    }

    bool IAbility.DoAction()
    {
        // Check ability cooldown
        // Consume dice
        int diceValue = diceManager.ConsumeDice();
        if (diceValue == 0)
        {
            // Not enough dice
            return false;
        }
        lastDiceValue = diceValue;

        // Set Player to jump
        characterComponent.DoJump(lastDiceValue);

        return true;
    }
}