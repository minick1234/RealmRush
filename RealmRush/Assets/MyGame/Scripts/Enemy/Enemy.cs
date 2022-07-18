using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int CashOnDeath = 25;
    [SerializeField] private int EndOfPathGoldAmount = 25;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GivePlayerMoneyForDeath()
    {
        Bank.IncreaseCurrentBalance(CashOnDeath);
    }

    public void StealPlayersMoney()
    {
        Bank.DecreaseCurrentBalance(EndOfPathGoldAmount);
    }
    
}