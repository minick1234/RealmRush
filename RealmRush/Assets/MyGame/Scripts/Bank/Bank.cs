using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bank : MonoBehaviour
{
    [SerializeField] private int startingBalance = 150;
    [SerializeField] private int currentBalance = 0;

    // Start is called before the first frame update
    void Awake()
    {
        currentBalance = startingBalance;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int GetCurrentBalance()
    {
        return currentBalance;
    }

    public void IncreaseCurrentBalance(int depositAmount)
    {
        //The mathf.abs just makes sure that any number that was inputed and passed no matter positive or negative will always be positive.
        currentBalance += Mathf.Abs(depositAmount);
    }

    public void DecreaseCurrentBalance(int withdrawAmount)
    {
        //This does the same thing as deposit as well to make sure that we are always removing a positive amount of money.
        //This also has to check for that the current balance even has this amount if not we can do something like end game because we are broke.
        if (currentBalance > 0 && (currentBalance - withdrawAmount) >= 0)
        {
            currentBalance -= Mathf.Abs(withdrawAmount);
        }
        else
        {
            currentBalance = 0;
            EndGame();
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("As of right now you are broke and a loser.");
    }
}