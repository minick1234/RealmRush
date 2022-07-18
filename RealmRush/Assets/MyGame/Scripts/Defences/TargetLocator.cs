using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    //Eventually this will have to be a dynamic system to grab the enemy closest to me but for right now i only have one enemy so i can easily make it just looks at the player.


    [SerializeField] public Transform Target;

    [SerializeField] private float RotationLength = 2f;
    [SerializeField] private float Range = 2.5f;

    [SerializeField] private ParticleSystem projectileParticles;

    [SerializeField] private float timeElapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        FindClosestEnemy();
        AimWeapon();

        if (Target == null)
        {
            var projectileParticlesEmission = projectileParticles.emission;
            projectileParticlesEmission.enabled = false;
        }
    }

    //This can be refactored to use a sphere check and then collect everything inside of that and check the distances between those objects and whichever is closer is the target.
    //this way i can also make a cool design or gizmo thing that will show the distance a turret can cover. - although this will suffice for now.
    private void FindClosestEnemy()
    {
        //Get all the enemies in the scene.
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        //The closest target - set to null because we havent found it yet.
        Transform closestTarget = null;

        //Set the original distance to be the longest possible. so its mathf.infinity because the first enemy we wanna find is this far away.
        float maxDistanceTillNextEnemy = Mathf.Infinity;
        foreach (var enemy in enemies)
        {
            float targetDistance = Vector3.Distance(this.gameObject.transform.position, enemy.transform.position);

            if ((targetDistance < maxDistanceTillNextEnemy))
            {
                closestTarget = enemy.transform;
                maxDistanceTillNextEnemy = targetDistance;
            }
        }

        if (closestTarget != null)
        {
            float distanceAgain =
                Vector3.Distance(this.gameObject.transform.position, closestTarget.transform.position);
            if (distanceAgain <= Range)
            {
                timeElapsed = 0f;
                Target = closestTarget;
            }
            else
            {
                Target = null;
                timeElapsed = 0f;
            }
        }
    }

    private void AimWeapon()
    {
        if (Target != null)
        {
            Vector3 directionToOtherObject = Target.position - this.gameObject.transform.position;
            Debug.DrawRay(new Vector3(this.transform.position.x, 2f, this.transform.position.z),
                new Vector3(directionToOtherObject.x, directionToOtherObject.y, directionToOtherObject.z),
                Color.red);

            float DistanceWithinRange = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToOtherObject);

            DistanceWithinRange = Vector3.Distance(this.gameObject.transform.position, Target.position);
            this.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation, targetRotation,
                Time.deltaTime * RotationLength);

            float dotProductReturned = Quaternion.Dot(targetRotation, this.gameObject.transform.rotation);
            Debug.Log(dotProductReturned);

            //The second part of this is using some new weird pattern thing lol idk never done it before. 
            //It essentially is the same thing as doing dotproductreturned >= 0.9 && dotproductreturned <= 1f -- but i guess its more readable and shorter so i dont call the variable twice or something.
            if ((DistanceWithinRange <= Range) &&
                ((dotProductReturned is >= 0.9f and <= 1f) || dotProductReturned is >= -1f and <= -0.9f))
            {
                Attack(true);
            }
            else
            {
                Attack(false);
            }
        }
    }

    void Attack(bool isActive)
    {
        var projectileParticlesEmission = projectileParticles.emission;
        projectileParticlesEmission.enabled = isActive;
    }
}