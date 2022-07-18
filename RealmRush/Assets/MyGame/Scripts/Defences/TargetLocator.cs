using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    //Eventually this will have to be a dynamic system to grab the enemy closest to me but for right now i only have one enemy so i can easily make it just looks at the player.


    [SerializeField] private Transform Target;

    [SerializeField] private float RotationLength = 2f;

    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        //i do this just incase there is no enemies on the screen whcih may be possible for game over screen but for now this should never reach here.
        try
        {
            FindClosestEnemy();
            AimWeapon();
        }
        catch (NullReferenceException e)
        {
            //if we catch the nullreferenceexception we can just print this and thats it.
            Console.WriteLine("danng no enemies eh. thats weird.");
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

            if (targetDistance < maxDistanceTillNextEnemy)
            {
                closestTarget = enemy.transform;
                maxDistanceTillNextEnemy = targetDistance;
            }
        }

        Target = closestTarget;
    }

    private void AimWeapon()
    {
        if (Target != null)
        {
            float currentRotationPercent = 0f;
            Vector3 directionToOtherObject = Target.position - this.gameObject.transform.position;
            Debug.DrawRay(this.transform.position, directionToOtherObject, Color.blue);
            Quaternion targetRotation = Quaternion.LookRotation(directionToOtherObject);
            currentRotationPercent += Time.deltaTime;
            this.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, targetRotation,
                Time.deltaTime / RotationLength);
        }
    }
}