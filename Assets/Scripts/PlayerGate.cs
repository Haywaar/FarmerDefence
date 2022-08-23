using System;
using Units;
using UnityEngine;

public class PlayerGate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<AbstractEnemy>();
        if (enemy != null)
        {
            enemy.Attack();
        }
    }
}
