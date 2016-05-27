using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour {

    private bool playerOut { get { return playerOut; } set { playerOut = value; } }
    
    [SerializeField]
    private GameObject player;
    [SerializeField]
    Vector3 spawnPoint;

    void Awake()
    {
        playerOut = false;
        spawnPoint = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerOut)
        {
            SceneReset();
        }     
    }
    void SceneReset()
    {
        playerOut = false;
    }

}
