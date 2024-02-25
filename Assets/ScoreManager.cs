using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    static public ScoreManager Instance;

    private int score;
    private Text scoreText;

    private int lightEnemyKillCount;
    private int heavyEnemyKillCount;
    private int superHeavyEnemyKillCount;
    private int factoryKillCount;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score;

        lightEnemyKillCount = 0;
        heavyEnemyKillCount = 0;
        superHeavyEnemyKillCount = 0;
        factoryKillCount = 0;
    }

    public void KillLightEnemy() 
    {
        lightEnemyKillCount++;
    }

    public void KillHeavyEnemy() 
    {
        heavyEnemyKillCount++;
    }

    public void KillSuperHeavyEnemy() 
    {
        superHeavyEnemyKillCount++;
    }

    public void KillFactory() {
        factoryKillCount++;
    }

    public void CalculateScore() 
    {
        score = lightEnemyKillCount * 50 + heavyEnemyKillCount * 100 + superHeavyEnemyKillCount * 150;
    }

    public int GetScore()
    {
        return score;
    }

    public UnityEngine.UI.Text GetScoreText()
    {
        return scoreText;
    }

    public int GetFactoryKillCount()
    {
        return factoryKillCount;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateScore();
        scoreText.text = "Score: " + score;
    }
}
