using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private Renderer[] rs;

    private void Start()
    {
        rs = GetComponentsInChildren<Renderer>();
    }

    public void Flash()
    {
        StartCoroutine(FlashCoroutine());

        IEnumerator FlashCoroutine()
        {
            SetMREmission(Color.white);

            yield return new WaitForSeconds(0.05f);

            SetMREmission(Color.black);
        }
    }

    void SetMREmission (Color color)
    {
        for (int i = 0; i < rs.Length; i++)
        {
            rs[i].material.SetColor("_EmissionColor", color);
        }
    }
}
