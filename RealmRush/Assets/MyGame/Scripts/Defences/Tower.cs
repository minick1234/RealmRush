using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int CostToPlace = 50;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool CreateTower(Tower tower, Vector3 PositionToSpawn)
    {
        //this way works but the way he does it in the video is just checks if the amount of the balance is greater then the cost
        //that basically is equivalent to this as it checks we have enough to spend before we spend, but this means we can spend to 0.
        if (Bank.GetCurrentBalance() - CostToPlace >= 0)
        {
            Bank.DecreaseCurrentBalance(CostToPlace);
            Instantiate(tower.gameObject, new Vector3(PositionToSpawn.x,
                PositionToSpawn.y, PositionToSpawn.z), Quaternion.identity);
            return true;
        }

        return false;
    }
}