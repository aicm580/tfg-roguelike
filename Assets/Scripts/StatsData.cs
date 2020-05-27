using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatsData
{
    public int damageDone;
    public int totalDeaths;
    public int deathsByBoss, deathsByNormalEnemies, deathsByItems, deathsByObstacles;
    public int totalKills;
    public int normalEnemiesKilled, bossesKilled;
    public int wins;
    public int travels;
    public float timePlayed, recordTime;
    public int maxLevelReached;


    public StatsData(GameManager game)
    {
        damageDone = game.damageDone;
        totalDeaths = game.totalDeaths;
        deathsByBoss = game.deathsByBoss;
        deathsByNormalEnemies = game.deathsByNormalEnemies;
        deathsByItems = game.deathsByItems;
        deathsByObstacles = game.deathsByObstacles;
        totalKills = game.totalKills;
        normalEnemiesKilled = game.normalEnemiesKilled;
        bossesKilled = game.bossesKilled;
        wins = game.wins;
        travels = game.travels;
        timePlayed = game.timePlayed;
        recordTime = game.recordTime;
        maxLevelReached = game.maxLevelReached;
    }
}
