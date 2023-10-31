using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatActionsButton : MonoBehaviour
{
    public TextMeshProUGUI nametext;

    private CombatAction combatAction;
    private CombatActionsUI ui;

    private void Awake()
    {
        ui = FindAnyObjectByType<CombatActionsUI>();
    }

    public void SetCombatAction (CombatAction ca)
    {
        combatAction = ca;
        nametext.text = ca.displayName;
    }

    public void OnClick()
    {
        PlayerCombatManager.instance.SetCurrentCombatAction(combatAction);
    }

    public void OnHoverEnter()
    {
        ui.SetCombatActionDescription(combatAction);
    }

    public void OnHoverExit()
    {
        ui.DisableCombatActionDescription();
    }
}
