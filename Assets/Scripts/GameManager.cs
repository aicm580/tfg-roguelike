using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MapGenerator mapGenerator;

    private int level = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();

        mapGenerator.SetupMap(level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
