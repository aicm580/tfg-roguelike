using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatsData
{
    public int damageDone;
    public int totalDeaths;
    public int deathsByBoss;
    public int deathsByNormalEnemies;
    public int totalKills;
    public int normalEnemiesKilled;
    public int bossesKilled;
    public int wins;
    public int travels;
    public int timePlayed;
    public int maxLevelReached;


    public StatsData(GameManager game)
    {
        damageDone = game.damageDone;
        totalDeaths = game.totalDeaths;
        deathsByBoss = game.deathsByBoss;
        deathsByNormalEnemies = game.deathsByNormalEnemies;
        totalKills = game.totalKills;
        normalEnemiesKilled = game.normalEnemiesKilled;
        bossesKilled = game.bossesKilled;
        wins = game.wins;
        travels = game.travels;
        timePlayed = game.timePlayed;
        maxLevelReached = game.maxLevelReached;
    }
}
