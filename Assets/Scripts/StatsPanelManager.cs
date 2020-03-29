using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanelManager : MonoBehaviour
{
    public Text damageScore;
    public Text totalDeathsScore;
    public Text deathsByBossScore;
    public Text deathsByEnemiesScore;
    public Text totalKillsScore;
    public Text normalKillsScore;
    public Text bossKillsScore;
    public Text winsScore;
    public Text travelsScore;
    public Text timePlayedScore;
    public Text maxLevelScore;


    // Start is called before the first frame update
    void Start()
    {
        StatsData stats = SaveManager.LoadStats();
       
        if (stats != null)
        {
            damageScore.text = stats.damageDone.ToString();
            totalDeathsScore.text = stats.totalDeaths.ToString();
            deathsByBossScore.text = stats.deathsByBoss.ToString();
            deathsByEnemiesScore.text = stats.deathsByNormalEnemies.ToString();
            totalKillsScore.text = stats.totalKills.ToString();
            normalKillsScore.text = stats.normalEnemiesKilled.ToString();
            bossKillsScore.text = stats.bossesKilled.ToString();
            winsScore.text = stats.wins.ToString();
            travelsScore.text = stats.travels.ToString();
            timePlayedScore.text = stats.timePlayed.ToString();
            maxLevelScore.text = stats.maxLevelReached.ToString();
        }
    }

}
