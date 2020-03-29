using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanelManager : MonoBehaviour
{
    public Text damageDoneText;

    // Start is called before the first frame update
    void Start()
    {
        StatsData stats = SaveManager.LoadStats();
       
        if (stats != null)
        {
            damageDoneText.text += ": " + stats.damageDone;
        }
        else
        {
            damageDoneText.text += ": 0";
        }
    }

}
