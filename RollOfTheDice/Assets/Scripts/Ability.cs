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
        Vector2 UIPosition = new Vector2(70 + ((float)id * (125.0f + 5.0f)), 75);
        UITransform.anchoredPosition = UIPosition;
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
    UISetBackgroundComponent UIBackground;
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
        UI = Object.Instantiate(abilityManager.jumpUIPrefab, UILayer.transform);
        UIBackground = UI.GetComponent<UISetBackgroundComponent>();
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
        UIBackground.SetAbilityDice(lastDiceValue); 

        return true;
    }
}

public class DashAbility : IAbility
{
    CharacterComponent characterComponent;
    AbilityManager abilityManager;
    DiceManager diceManager;
    UISetBackgroundComponent UIBackground;
    GameObject UI;
    int id;
    int lastDiceValue = 0;

    public DashAbility(int newId)
    {
        GameObject UILayer = GameObject.FindGameObjectWithTag("UI");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        characterComponent = player.GetComponent<CharacterComponent>();
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        abilityManager = controller.GetComponent<AbilityManager>();
        diceManager = controller.GetComponent<DiceManager>();

        id = newId;
        UI = Object.Instantiate(abilityManager.dashUIPrefab, UILayer.transform);
        UIBackground = UI.GetComponent<UISetBackgroundComponent>();
        AbilityUtils.UpdateUI(UI, id, type);
    }

    public AbilityType type
    {
        get { return AbilityType.Dash; }
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

        // Do action
        characterComponent.DoDash(lastDiceValue);
        UIBackground.SetAbilityDice(lastDiceValue);

        return true;
    }
}

public class AttackAbility : IAbility
{
    CharacterComponent characterComponent;
    AbilityManager abilityManager;
    DiceManager diceManager;
    UISetBackgroundComponent UIBackground;
    GameObject UI;
    int id;
    int lastDiceValue = 0;

    public AttackAbility(int newId)
    {
        GameObject UILayer = GameObject.FindGameObjectWithTag("UI");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        characterComponent = player.GetComponent<CharacterComponent>();
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        abilityManager = controller.GetComponent<AbilityManager>();
        diceManager = controller.GetComponent<DiceManager>();

        id = newId;
        UI = Object.Instantiate(abilityManager.attackUIPrefab, UILayer.transform);
        UIBackground = UI.GetComponent<UISetBackgroundComponent>();
        AbilityUtils.UpdateUI(UI, id, type);
    }

    public AbilityType type
    {
        get { return AbilityType.Attack; }
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

        // Do action
        characterComponent.DoAttack(lastDiceValue);
        UIBackground.SetAbilityDice(lastDiceValue);

        return true;
    }
}