using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyCombatManager : MonoBehaviour
{
    [Header("AI")]
    public float minWaitTime = 0.2f;
    public float maxWaitTIme = 1.0f;

    [Header("Attacking")]
    public float attackWeakestChance = 0.7f;

    [Header("Chance Curves")]
    public AnimationCurve healChanceCurve;


    private Character curEnemy;

    private void OnEnable()
    {
        TurnManager.instance.OnNewTurn += OnNewTurn;
    }

    private void OnDisable()
    {
        TurnManager.instance.OnNewTurn -= OnNewTurn;
    }

    void OnNewTurn()
    {
        if(TurnManager.instance.GetCurrentTurnCharacter().team == Character.Team.Enemy)
        {
            curEnemy = TurnManager.instance.GetCurrentTurnCharacter();
            Invoke(nameof(DecideCombatAction), Random.Range(minWaitTime, maxWaitTIme));
        }
    }

    void DecideCombatAction()
    {
        if(HasCombatActionOfType(typeof(HealCombatAction)))
        {
            Character weakestEnemy = GetWeakestCharacter(Character.Team.Enemy);

            if(Random.value < healChanceCurve.Evaluate(GetHealthPercentage(weakestEnemy)))
            {
                CastCombatAction(GetHealCombatAction(), weakestEnemy);
                return;
            }
        }
 
        Character playerToDamage;

        if(Random.value < attackWeakestChance)
        {
            playerToDamage = GetWeakestCharacter(Character.Team.Player);
        }
        else
        {
            playerToDamage = GetRandomCharacter(Character.Team.Player);
        }

        if(playerToDamage != null)
        {
            if(HasCombatActionOfType(typeof(MeleeCombatAction)) || HasCombatActionOfType(typeof(RangedCombatAction)))
            {
                CastCombatAction(GetDamageCombatAction(), playerToDamage);
                return;
            }
        }

        Invoke(nameof(EndTurn), Random.Range(minWaitTime, maxWaitTIme));
    }

    void CastCombatAction(CombatAction combatAction, Character target)
    {
        if(curEnemy == null)
        {
            EndTurn();
            return;
        }

        curEnemy.CastCombatAction(combatAction, target);
        Invoke(nameof(EndTurn), Random.Range(minWaitTime, maxWaitTIme));
    }

    void EndTurn()
    {
        TurnManager.instance.EndTurn();
    }

    float GetHealthPercentage(Character character)
    {
        return (float)character.curHp / (float)character.maxHp;
    }

    bool HasCombatActionOfType(System.Type type)
    {
        foreach(CombatAction ca in curEnemy.combatActions)
        {
            if(ca.GetType() == type)
            {
                return true;
            }
        }

        return false;
    }

    CombatAction GetDamageCombatAction()
    {
        CombatAction[] ca = curEnemy.combatActions.Where(x => x.GetType() == typeof(MeleeCombatAction) || x.GetType() == typeof(RangedCombatAction)).ToArray();

        if(ca == null || ca.Length == 0)
        {
            return null;
        }

        return ca[Random.Range(0, ca.Length)];
    }

    CombatAction GetHealCombatAction()
    {
        CombatAction[] ca = curEnemy.combatActions.Where(x => x.GetType() == typeof(HealCombatAction)).ToArray();

        if (ca == null || ca.Length == 0)
        {
            return null;
        }

        return ca[Random.Range(0, ca.Length)];
    }

    CombatAction GetEffectCombatAction()
    {
        CombatAction[] ca = curEnemy.combatActions.Where(x => x.GetType() == typeof(EffectCombatAction)).ToArray();

        if (ca == null || ca.Length == 0)
        {
            return null;
        }

        return ca[Random.Range(0, ca.Length)];
    }

    Character GetWeakestCharacter (Character.Team team)
    {
        int weakestHp = 999;
        int weakestIndex = 0;

        Character[] characters = team == Character.Team.Player ? GameManager.instance.playerTeam.ToArray() : GameManager.instance.enemyTeam;

        for(int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == null)
            {
                continue;
            }

            if (characters[i].curHp < weakestHp)
            {
                weakestHp = characters[i].curHp;
                weakestIndex = i;
            }
        }

        return characters[weakestIndex];
    }

    Character GetRandomCharacter (Character.Team team)
    {
        Character[] characters = null;

        if (team == Character.Team.Player)
        {
            characters = GameManager.instance.playerTeam.Where(x => x != null).ToArray();
        }

        if (team == Character.Team.Enemy)
        {
            characters = GameManager.instance.enemyTeam.Where(x => x != null).ToArray();
        }

        return characters[Random.Range(0, characters.Length)];
    }
}
