using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotControl : MonoBehaviour {

    short velocity = 15;

    Transform bulletShot;
    // Use this for initialization
    void Start () {
     
        //float shootingDirection = Mathf.Atan2(-Input.GetAxis("xShoot"), Input.GetAxis("yShoot")) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(shootingDirection, Vector3.forward);

        Vector3 direction = transform.rotation * Vector3.up;

        GetComponent<Rigidbody>().velocity = direction * velocity;
        StartCoroutine(selfDestruct());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

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
