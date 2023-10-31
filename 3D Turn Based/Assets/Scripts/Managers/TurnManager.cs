using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    private List<Character> turnOrder = new List<Character>();
    private int curTurnOrderIndex;
    private Character curTurnCharacter;

    [Header("Components")]
    public GameObject endTurnButton;

    public static TurnManager instance;

    public event UnityAction OnNewTurn;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void Begin()
    {
        GenerateTurnOrder(Character.Team.Player);
        NewTurn(turnOrder[0]);
    }

    void GenerateTurnOrder(Character.Team startingTeam)
    {
        if (startingTeam == Character.Team.Player)
        {
            turnOrder.AddRange(GameManager.instance.playerTeam);
            turnOrder.AddRange(GameManager.instance.enemyTeam);
        }
        else if (startingTeam == Character.Team.Enemy)
        {
            turnOrder.AddRange(GameManager.instance.enemyTeam);
            turnOrder.AddRange(GameManager.instance.playerTeam);
        }
    }

    void NewTurn(Character character)
    {
        curTurnCharacter = character;
        OnNewTurn?.Invoke();

        endTurnButton.SetActive(character.team == Character.Team.Player);
    }

    public void EndTurn()
    {
        curTurnOrderIndex++;
        if(curTurnOrderIndex == turnOrder.Count)
        {
            curTurnOrderIndex = 0;
        }

        while (turnOrder[curTurnOrderIndex] == null)
        {
            curTurnOrderIndex++;
            if (curTurnOrderIndex == turnOrder.Count)
            {
                curTurnOrderIndex = 0;
            }
        }

        NewTurn(turnOrder[curTurnOrderIndex]);
    }

    public Character GetCurrentTurnCharacter()
    {
        return curTurnCharacter;
    }

}
