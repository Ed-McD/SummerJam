using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour {

    private bool playerOut = false;
    [SerializeField]
    private GameObject player;    
    private Vector3 spawnPoint;    

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
    public void SetPlayerOut(bool _val)
    {
        playerOut = _val;
    }

    void SceneReset()
    {
        playerOut = false;
        player.GetComponent<Tether>().Reset();
        player.GetComponent<PlayerMovement>().Reset();
        player.transform.position = spawnPoint;
        LevelGenerator.instance.RemoveLevel();
    }

}
