using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data 
{
    public int[] cardImageIndexes;
    public int nbrOfMatches, nbrOfTurns, nbrCombo;
   
    public Data(int[] imageIndexes,int _nbrOfmatches,int _nbrOfTurns,int _nbrCombo)
    {
        cardImageIndexes = imageIndexes;
        nbrOfMatches = _nbrOfmatches;
        nbrOfTurns = _nbrOfTurns;
        nbrCombo = _nbrCombo;
    }
}
