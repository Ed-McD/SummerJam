using UnityEngine;
using System.Collections;

public class DataBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
            coll.GetComponent<PlayerData>().IncrementScore();

        LevelGenerator.instance.RemoveData(gameObject);
    }
}
