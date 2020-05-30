using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanelManager : MonoBehaviour
{
    public Text damageScore;
    public Text totalDeathsScore, deathsByBossScore, deathsByEnemiesScore, deathsByObstaclesScore, deathsByItemsScore;
    public Text totalKillsScore, normalKillsScore, bossKillsScore;
    public Text travelsScore;
    public Text timePlayedScore, recordTimeScore;
    public Text winsScore;
    public Text maxLevelScore;

    void Start()
    {
        StatsData stats = SaveManager.LoadStats();
       
        if (stats != null)
        {
            damageScore.text = stats.damageDone.ToString();
            totalDeathsScore.text = stats.totalDeaths.ToString();
            deathsByBossScore.text = stats.deathsByBoss.ToString();
            deathsByEnemiesScore.text = stats.deathsByNormalEnemies.ToString();
            deathsByObstaclesScore.text = stats.deathsByObstacles.ToString();
            deathsByItemsScore.text = stats.deathsByItems.ToString();
            totalKillsScore.text = stats.totalKills.ToString();
            normalKillsScore.text = stats.normalEnemiesKilled.ToString();
            bossKillsScore.text = stats.bossesKilled.ToString();
            winsScore.text = stats.wins.ToString();
            travelsScore.text = stats.travels.ToString();
            maxLevelScore.text = stats.maxLevelReached.ToString();
            
            timePlayedScore.text = ConvertTime(stats.timePlayed);
            recordTimeScore.text = ConvertTime(stats.recordTime);
        }
    }

    //Convierte el tiempo jugado a horas, minutos y segundos
    private string ConvertTime(float t)
    {
        float time = t;
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        return timeText;
    }
}
