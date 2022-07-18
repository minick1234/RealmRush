using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int CashOnDeath = 25;
    [SerializeField] private int EndOfPathGoldAmount = 25;
    [SerializeField] private Bank _bank;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _bank = FindObjectOfType<Bank>();
    }

    public void GivePlayerMoneyForDeath()
    {
        _bank.IncreaseCurrentBalance(CashOnDeath);
    }

    public void StealPlayersMoney()
    {
        _bank.DecreaseCurrentBalance(EndOfPathGoldAmount);
    }
    
}