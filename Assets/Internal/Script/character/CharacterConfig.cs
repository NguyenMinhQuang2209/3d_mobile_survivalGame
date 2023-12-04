using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterConfig : MonoBehaviour
{
    [SerializeField] private Transform handHolder;

    PlayerAttacking playerAttacking;
    private void Start()
    {
        transform.parent.gameObject.TryGetComponent<PlayerAttacking>(out playerAttacking);
    }
    public void Attack()
    {
        if (playerAttacking != null)
        {
            playerAttacking.Attack();
        }
    }
}
