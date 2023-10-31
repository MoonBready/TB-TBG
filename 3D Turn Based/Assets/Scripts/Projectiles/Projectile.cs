using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public int heal;
    public Effect effectToApply;

    public float moveSpeed;

    private Character target;
    private Vector3 verticalOffset = new Vector3 (0, 0.5f, 0);

    public void Initialize (Character targetChar)
    {
        target = targetChar;
    }

     void Update ()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position + verticalOffset, moveSpeed * Time.deltaTime);
        }
    }

    void ImpactTarget()
    {
        if (damage > 0)
        {
            target.TakeDamage(damage);
        }
       
        if (heal > 0)
        {
            target.Heal(heal);
        }

        if(effectToApply != null)
        {
            target.GetComponent<CharacterEffects>().AddNewEffect(effectToApply);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target != null && other.gameObject == target.gameObject)
        {
            ImpactTarget();
            Destroy(gameObject);
        }
    }
}
