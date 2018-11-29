using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameMaster : MonoBehaviour {

    #region Player/Game Score Management
    public static float playerScore;
    public static int levelUpRequirement = 150;
    public static int level = 1;
    public static int healthPoints = 1;
    public static int isDead;
    private float highScore;
    #endregion

    #region Public Objects
    public Transform enemyPentagon;
    public Transform enemyRhombus;
    public Transform dmgUp;
    public Transform speedUp;
    public Transform lifeUp;
    public Text score;
    public Text health;
    public Text deathScore;
    public GameObject DeathScreen;
    public AudioSource MusicSource;
    public AudioClip[] MusicClips;
    #endregion

    #region Music
    private int whichSong;
    private int prevSong;
    #endregion

    #region Powerup Spawning
    float spawnPowerupLocX;
    float spawnPowerupLocY;
    float spawnPowerup;
    float spawnDelay;
    float baseDelay = 1.5f;
    float whatPowerup;
    #endregion

    #region Enemy Spawning
    float spawnLoc;
    float mobSpawned;
    int spawnCoord = 20;
    float x;
    float y;
    #endregion

	// Use this for initialization
	void Start () {
        highScore = PlayerPrefs.GetFloat("High Score");
        StartCoroutine(powerUpDelay());
        whichSong = Random.Range(0, MusicClips.Length);
        MusicSource.clip = MusicClips[whichSong];
        MusicSource.Play();
        prevSong = whichSong;
    }
	
	// Update is called once per frame
	void Update () {
        score.text = "Score: " + playerScore + "\n" + "High Score: " + highScore;
        health.text = "Life: " + PlayerControl.health + "/" + (PlayerPrefs.GetFloat("Health") + 5);
        spawnLoc = Random.Range(0, 361);
        spawnPowerupLocX = Random.Range(-11, 12);
        spawnPowerupLocY = Random.Range(-6, 7);
        whatPowerup = Random.Range(0, 3);
        

        if(!MusicSource.isPlaying)
        {
            whichSong = Random.Range(0, MusicClips.Length);
            while (whichSong == prevSong)
            {
                whichSong = Random.Range(0, MusicClips.Length);
            }
            MusicSource.clip = MusicClips[whichSong];
            MusicSource.Play();
        }

        if (spawnPowerup == 1)
        {
            spawnPowerUp();
            spawnPowerup = 0;
        }

        if (playerScore > highScore)
        {
            highScore = playerScore;
            PlayerPrefs.SetFloat("High Score", highScore);
        }

        if (spawnDelay == 0)
        {
            x = spawnCoord * (Mathf.Sin(Mathf.Deg2Rad * (90 - spawnLoc)));
            y = spawnCoord * (Mathf.Sin(Mathf.Deg2Rad * spawnLoc));
            spawnMob(x, y, -1);
            spawnDelay = baseDelay;
            StartCoroutine(delayRest());              
        }
        
        if (isDead == 1)
        {
            DeathScreen.gameObject.SetActive(true);
            deathScore.text = "Score: " + playerScore + "\n" + "High Score: " + highScore;
            
            
            if(Input.GetKeyDown("space"))
            {
                SceneManager.LoadScene("GameScene");
                isDead = 0;
                playerScore = 0;
                spawnDelay = 0;
                levelUpRequirement = 150;
                level = 1;
                healthPoints = 1;
            }
            if(Input.GetKeyDown(KeyCode.Joystick1Button0) )
            {
                SceneManager.LoadScene("GameScene");
                isDead = 0;
                playerScore = 0;
                spawnDelay = 0;
                levelUpRequirement = 150;
                level = 1;
                healthPoints = 1;
            }
        }


        if (playerScore >= levelUpRequirement)
        {
            healthPoints += level/2;
            level += 1;
            levelUpRequirement = levelUpRequirement * level;
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }


    }

    void spawnMob(float x,float y,float z)
    {
        mobSpawned = Random.Range(0, 2);
        if (mobSpawned == 1f)
        {
            Instantiate(enemyPentagon, new Vector3(x, y, z), enemyPentagon.rotation);
        }
        else if (mobSpawned == 0f)
        {   
            Instantiate(enemyRhombus, new Vector3(x, y, z), enemyRhombus.rotation);
        }
    }

    void spawnPowerUp()
    { 
        if (whatPowerup == 0)
        {
            Instantiate(dmgUp, new Vector3(spawnPowerupLocX, spawnPowerupLocY, -1), dmgUp.rotation);
            StartCoroutine(powerUpDelay());
        } else if (whatPowerup == 1)
        {
            Instantiate(speedUp, new Vector3(spawnPowerupLocX, spawnPowerupLocY, -1), speedUp.rotation);
            StartCoroutine(powerUpDelay());
        } else if (whatPowerup == 2)
        {
            Instantiate(lifeUp, new Vector3(spawnPowerupLocX, spawnPowerupLocY, -1), lifeUp.rotation);
            StartCoroutine(powerUpDelay());
        }

        
    }

    IEnumerator powerUpDelay()
    {
        yield return new WaitForSeconds(Random.Range(20, 40));
        spawnPowerup = 1;
    }

    IEnumerator delayRest()
    {
        yield return new WaitForSeconds(baseDelay/level*0.75f);
        spawnDelay = 0;
    }

}
