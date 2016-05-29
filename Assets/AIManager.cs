using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AIManager : MonoBehaviour {

    static AIManager _instance;
    public static AIManager instance { get { return _instance; } }
    public GameObject bot;
    List<GameObject> bots = new List<GameObject>();

    // Use this for initialization
    void Awake()
    {
        _instance = this;
    }

    public void Populate(float _size, float _gap, float _pillarSize, float _blocksize)
    {
        AIController tempAI;
        List<Vector3> tempWaypoints = new List<Vector3>();
        for (int i = 0; i < _size; i++)
        {
            tempWaypoints.Clear();
            bots.Add(Instantiate(bot));
            tempAI = bots[i].GetComponent<AIController>();
            bots[i].transform.position = new Vector3( i * _gap - _gap / 2, Random.Range(-_pillarSize / 2, _pillarSize / 2), Random.Range(0, _size* _gap - _gap / 2));
            tempAI.Waypoints.Add(new Vector3(bots[i].transform.position.x, bots[i].transform.position.y, 0));
            Vector3 newWaypoint = tempAI.Waypoints[0];           
            newWaypoint.z = _size * _gap - _gap/ 2;      
            tempAI.Waypoints.Add(newWaypoint);
           
            //Reflect waypoints;
            foreach (Vector3 wp in tempAI.Waypoints)
            {
                tempWaypoints.Add(wp);
            }
            tempWaypoints.Reverse();
            tempWaypoints.RemoveAt(tempWaypoints.Count - 1);
            tempWaypoints.RemoveAt(0);
            for (int j = 0; j < tempWaypoints.Count; j++)
            {
                tempAI.Waypoints.Add(tempWaypoints[i]);
            }           

        }
    }

   public void Reset()
    {
        List<GameObject> toDestroy = new List<GameObject>();
        foreach (GameObject gO in bots)
        {
            toDestroy.Add(gO);
            Destroy(gO);
            
        }
        bots.Clear();
        //foreach (GameObject gO in toDestroy)
        //{
        //    Destroy(gO);
        //}

    }
    private bool RandomBool()
    {
        if (Random.value >= 0.5f)
            return true;
        return false;
    }
}
