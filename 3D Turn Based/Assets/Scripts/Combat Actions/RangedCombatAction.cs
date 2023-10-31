using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Combat Action", menuName = "Combat Actions/Ranged Combat Action")]
public class RangedCombatAction : CombatAction
{
    public GameObject projectilePrefab;
    private Vector3 verticalOffset = new Vector3 (0, 0.5f, 0);

    public override void Cast(Character caster, Character target)
    {
        if(caster == null)
        {
            return;
        }
        GameObject proj = Instantiate(projectilePrefab, caster.transform.position + verticalOffset, Quaternion.identity);
        proj.GetComponent<Projectile>().Initialize(target);
        
    }
}
