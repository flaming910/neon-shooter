using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public static Vector3 playerPos;
    public static float health; 

	float xVelAdj;
	float yVelAdj;
	public float xFire;
	public float yFire;
    public bool isDS4;
    public static float enemyDied;
    public static float damage;
    float speed;

    float leftBorder;
    float rightBorder;
    float bottomBorder;
    float topBorder;
    Vector3 playerSize;


    Vector3 lastKnownRotation;
    float powerUp;
    float speedUp;
    float lifeUp;
    public AudioClip bulletSound;
    public AudioClip gotHit;
    public AudioClip blewUp;
    public Transform bulletShot;
    public Transform bulletShotRed;
    float shotDelay;
    public static float baseDelay;

    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        damage = 1;
        health = 5;
        speed = 6;
        baseDelay = 0.08f;
        //Playstation 4 Controller seems to show up as Wireless Controller, so this checks for that since it has different inputs from an Xbox controller
        if (Input.GetJoystickNames()[Input.GetJoystickNames().Length - 1] == "Wireless Controller")
        {
            isDS4 = true;
        }
        else
        {
            isDS4 = false;
        }

        //Limits where the player can move
        playerSize = GetComponent<Renderer>().bounds.size;
        leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -1)).x + (playerSize.x / 2);
        rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, -1)).x - (playerSize.x / 2);
        bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -1)).y + (playerSize.y / 2);
        topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, -1)).y - (playerSize.y / 2);
        
    }
	
	// Update is called once per frame
	void Update () {

        //Used for enemy pathing
        playerPos = transform.position;

        //Used to make sure that the player can't leave the screen
        transform.position = (new Vector3(
             Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
             Mathf.Clamp(transform.position.y, bottomBorder, topBorder),
             transform.position.z)
         );

        xVelAdj = Input.GetAxis ("xMove");
		yVelAdj = Input.GetAxis ("yMove");        
        if(!isDS4)
        {
            xFire = Input.GetAxis("xShoot");
            yFire = Input.GetAxis("yShoot");
        } else
        {
            xFire = Input.GetAxis("xShootDS4");
            yFire = Input.GetAxis("yShootDS4");
        }

        //Plays audio on enemy death
        if (enemyDied == 1)
        {
            audioSource.clip = blewUp;
            audioSource.Play();
            enemyDied = 0;
        }

        #region Power Ups
        if (powerUp == 2)
        {
            damage = 2;
            StartCoroutine(powerUPTimer());
        }
        if (powerUp == 1)
        {
            damage = 1;
            powerUp = 0;
        }

        if (speedUp == 2)
        {
            speed = 9;
            StartCoroutine(speedUPTimer());
        }
        if (speedUp == 1)
        {
            speed = 6;
            speedUp = 0;
        }

        if (lifeUp == 1)
        {
            health += 1;
            lifeUp = 0;
        }
        #endregion

        #region Rotation Control
        float movingDirection = Mathf.Atan2(-Input.GetAxis("xMove"), -Input.GetAxis("yMove")) * Mathf.Rad2Deg;
        float shootingDirection = Mathf.Atan2(-xFire, yFire) * Mathf.Rad2Deg;
        if (xFire != 0 || yFire != 0)
        {            
            transform.rotation = Quaternion.AngleAxis(shootingDirection, Vector3.forward);
            lastKnownRotation = GetComponent<Transform>().eulerAngles;
        }
        else if (xVelAdj != 0 || yVelAdj != 0)
        {
            transform.rotation = Quaternion.AngleAxis(movingDirection, Vector3.forward);
            lastKnownRotation = GetComponent<Transform>().eulerAngles;
        } else
        {
            GetComponent<Transform>().eulerAngles = lastKnownRotation;
        }
        #endregion

        #region Shooting Control
        if ((xFire> 0.2 || xFire <-0.2) && (shotDelay == 0))
        {
            if (powerUp == 2)
            {
                Instantiate(bulletShotRed, transform.position, transform.rotation);
            } else { 
                Instantiate(bulletShot, transform.position, transform.rotation);
            }
            shotDelay = baseDelay;
            audioSource.clip = bulletSound;
            audioSource.Play();
            StartCoroutine(delayRest() );
        }

        if ((yFire > 0.2 || yFire < -0.2) && (shotDelay == 0))
        {
            shotDelay = baseDelay;
            if (powerUp == 2)
            {
                Instantiate(bulletShotRed, transform.position, transform.rotation);
            }
            else
            {
                Instantiate(bulletShot, transform.position, transform.rotation);
            }
            audioSource.clip = bulletSound;
            audioSource.Play();
            StartCoroutine(delayRest() );
        }
        #endregion

        GetComponent<Rigidbody>().velocity = new Vector3 (xVelAdj * speed, yVelAdj * -speed,0);
    }

    IEnumerator powerUPTimer()
    {
        yield return new WaitForSeconds(15);
        powerUp = 1;
    }
    IEnumerator speedUPTimer()
    {
        yield return new WaitForSeconds(15);
        speedUp = 1;
    }

    IEnumerator delayRest()
    {
        yield return new WaitForSeconds(baseDelay);
        shotDelay = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        #region Enemy Collision Detection
        if (other.name == "EnemyPentagon(Clone)" || other.name == "EnemyRhombus(Clone)")
        {
            audioSource.clip = gotHit;
            audioSource.Play();
            if (health == 1)
            {
                GameMaster.isDead = 1;
                Destroy(gameObject);
                health -= 1;

            }
            else
            {
                health -= 1;
            }
        }
        #endregion

        #region Collision With Powerups
        if (other.name == "DmgUp(Clone)")
        {
            powerUp = 2;
        }
        if (other.name == "SpeedUp(Clone)")
        {
            speedUp = 2;
        }
        if (other.name == "LifeUp(Clone)")
        {
            lifeUp = 1;
        }
        #endregion

    }
}
