using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHitPoints = 3;
    [SerializeField] private int currentHitPoints = 0;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private TargetLocator tg;

    private void Awake()
    {
        _enemy = this.gameObject.GetComponent<Enemy>();
    }

    //Reset this objects health when it is enabled or disabled and reenabled.
    void OnEnable()
    {
        currentHitPoints = maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHitPoints <= 0)
        {
            KillEnemy();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Turret"))
        {
            tg = other.gameObject.transform.parent.GetComponent<TargetLocator>();
            Debug.Log(tg);
            if (currentHitPoints > 0)
            {
                //reduce the current hitpoints that the enemy has left.
                currentHitPoints--;
            }
        }
    }

    private void KillEnemy()
    {
        tg.Target = null;
        ObjectPool.DecreaseCurrentlyActiveEnemies();
        _enemy.GivePlayerMoneyForDeath();
        gameObject.SetActive(false);
        
    }
}