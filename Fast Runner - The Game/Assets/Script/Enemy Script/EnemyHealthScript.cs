using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] [Range(0f, 100f)] private float currentHealth;

    [Header("Toughness")]
    [SerializeField] private float toughness = 1f;

    [Header("Regenarate")]
    [SerializeField] private float regenarateRate = 0.5f;
    [SerializeField] private float regenaratePower = 3.3f;

    private float regenarateTime = 0f;

    private void Update()
    {
        if(regenarateTime < regenarateRate)
        {
            regenarateTime += Time.deltaTime;
        }
    }

    public float Health()
    {
        return currentHealth;
    }

    protected internal void Regenarate()
    {
        if(regenarateTime >= regenarateRate)
        {
            currentHealth += (currentHealth + regenaratePower > 100f)? 100f : currentHealth + regenaratePower;
            regenarateTime = 0f;
        }
    }

    protected internal bool DamageTake(float damage)
    {
        currentHealth -= damage / toughness;

        bool result = (currentHealth <= 0f)? true : false;

        return result;
    }
}
