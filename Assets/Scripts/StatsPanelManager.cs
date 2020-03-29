using System;
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
            maxLevelScore.text = stats.maxLevelReached.ToString();

            //Convertimos el tiempo jugado a horas, minutos y segundos
            float timePlayed = stats.timePlayed;

            TimeSpan timeSpan = TimeSpan.FromSeconds(timePlayed);
            string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            timePlayedScore.text = timeText;
        }
    }

}
