using System;

[Serializable] //así se mostrará en el Inspector
public class IntRange
{
    public int minVal; //valor mínimo del rango
    public int maxVal; //valor máximo del rango

    //Constructor
    public IntRange (int min, int max)
    {
        minVal = min;
        maxVal = max;
    }

    public int Randomize
    {
        get { return UnityEngine.Random.Range(minVal, maxVal); }
    }
}
