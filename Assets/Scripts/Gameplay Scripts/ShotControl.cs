using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotControl : MonoBehaviour {

    //Determines bullet speed
    short velocity = 15;

    // Use this for initialization
    void Start () {

        Vector3 direction = transform.rotation * Vector3.up;

        GetComponent<Rigidbody>().velocity = direction * velocity;
        StartCoroutine(selfDestruct());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Gets rid of the bullet after 1.5 seconds
    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "EnemyPentagon(Clone)" || other.name == "EnemyRhombus(Clone)")
        {
            Destroy(gameObject);
        }
    }

}
