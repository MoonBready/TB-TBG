using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Persistent Data", menuName = "New Player Persistent Data")]
public class PlayerPersistentData : ScriptableObject
{
    public PlayerPersistentCharacter[] characters;

#if UNITY_EDITOR

    private void OnValidate()
    {
        ResetCharacter();
    }

#endif

    public void ResetCharacter()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].health = characters[i].characterPrefab.GetComponent<Character>().maxHp;
            characters[i].isDead = false;
        }
    }
}
