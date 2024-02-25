using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int difficulty;             // 1 = Easy, 2 = Medium, 3 = Hard
    private int requiredFactoryKills;   // 5 = Easy, 10 = Medium, 15 = Hard

    public void setDifficulty(int newDiff)
    {
        difficulty = newDiff;
        requiredFactoryKills = difficulty * 5;
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    /*
    // When the required number of factories are destroyed, the level ends
    public bool levelWin()
    {
        kills = GetFactoryKillCount() Function call from ScoreManager
        if (kills == requiredFactoryKills) 
        {
            return true;
        }

        return false;
    }
    */

    // Update is called once per frame
    void Update()
    {
        //levelWin()
    }
}
