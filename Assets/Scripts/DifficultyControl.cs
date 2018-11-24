using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyControl : MonoBehaviour {

    public static float healthMultiplier;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void normalDiff()
    {
        healthMultiplier = 1;
    }

    public void hardDiff()
    {
        healthMultiplier = 2;
    }

    public void masterDiff()
    {
        healthMultiplier = 4;
    }

    public void veteranDiff()
    {
        healthMultiplier = 8;
    }
}
