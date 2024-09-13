using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data 
{
    public int[] cardImageIndexes;
   
    public Data(int[] imageIndexes)
    {
        cardImageIndexes = imageIndexes;
    }
}
