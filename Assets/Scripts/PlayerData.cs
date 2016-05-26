using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour
{
    public int playerScore = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void IncrementScore()
    {
        ++playerScore;
    }
}
