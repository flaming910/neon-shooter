using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour {

    public Text level;
    public Text xP;
    public Text points;
    public Text damage;
    public Text speed;
    public Text health;

    public float damageStat;
    public float speedStat;
    public float healthStat;

    static float playerLevel;
    static float statPoints;
    public static float playerXP;
    static float playerLevelUpRequirement;

	// Use this for initialization
	void Start () {
        playerLevelUpRequirement = 500;
        if(PlayerPrefs.GetFloat("Level Up Requirement") > playerLevelUpRequirement)
        {
            playerLevelUpRequirement = PlayerPrefs.GetFloat("Level Up Requirement");
        }
        playerXP = PlayerPrefs.GetFloat("Player XP");
        playerLevel = PlayerPrefs.GetFloat("Player Level");
        statPoints = PlayerPrefs.GetFloat("Stat Points");
        damageStat = PlayerPrefs.GetFloat("Damage");
        speedStat = PlayerPrefs.GetFloat("Speed");
        healthStat = PlayerPrefs.GetFloat("Health");
	}
	
	// Update is called once per frame
	void Update () {
        level.text = "Level: " + playerLevel;
		damage.text = "Damage: " + (1 + damageStat);
        speed.text = "Speed: " + (6 + speedStat);
        health.text = "Health: " + (5 + healthStat);
        xP.text = "XP: " + playerXP;
        points.text = "Points Available: " + statPoints;
	}

    public void damageUp()
    {
        if(statPoints >= 1)
        {
            statPoints -= 1;
            damageStat += 1;
            PlayerPrefs.SetFloat("Stat Points", statPoints);
            PlayerPrefs.SetFloat("Damage", damageStat);
        }
    }

    public void speedUp()
    {
        if (statPoints >= 1)
        {
            statPoints -= 1;
            speedStat += 1;
            PlayerPrefs.SetFloat("Stat Points", statPoints);
            PlayerPrefs.SetFloat("Speed", speedStat);
        }
    }

    public void healthUp()
    {
        if (statPoints >= 1)
        {
            statPoints -= 1;
            healthStat += 1;
            PlayerPrefs.SetFloat("Stat Points", statPoints);
            PlayerPrefs.SetFloat("Health", healthStat);
        }
    }

    public static void levelUp()
    {
        if(playerXP > playerLevelUpRequirement)
        {
            playerLevel += 1;
            statPoints += 2;
            playerLevelUpRequirement *= 2;
            PlayerPrefs.SetFloat("Player Level", playerLevel);
            PlayerPrefs.SetFloat("Stat Points", statPoints);
            PlayerPrefs.SetFloat("Level Up Requirement", playerLevelUpRequirement);
        }
        
    }


}
