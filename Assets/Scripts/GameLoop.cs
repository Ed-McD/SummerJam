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
        if (Input.GetButtonDown("Jump"))
        {
            playerOut = true;
        }
    }
    public void SetPlayerOut(bool _val)
    {
        playerOut = _val;
    }

    void SceneReset()
    {
        playerOut = false;
        AIManager.instance.Reset();        
        player.GetComponent<Tether>().Reset();
        player.GetComponent<PlayerMovement>().Reset();
        player.GetComponent<PlayerData>().playerScore = 0;
        player.transform.position = spawnPoint;
        player.GetComponentInChildren<ParticleSystem>().Clear();
        player.GetComponentInChildren<ParticleSystem>().Stop();
        player.GetComponentInChildren<ParticleSystem>().Play();
        CanvasManager.instance.SetScore(0);
        LevelGenerator.instance.ResetLevel();
        

    }

}
