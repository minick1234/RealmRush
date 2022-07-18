using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHitPoints = 3;
    [SerializeField] private int currentHitPoints = 0;

    [SerializeField] private TargetLocator tg;

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
        gameObject.SetActive(false);
    }
}