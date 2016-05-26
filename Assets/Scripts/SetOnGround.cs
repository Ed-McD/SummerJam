using UnityEngine;
using System.Collections;

public class SetOnGround : MonoBehaviour
{
    PlayerMovement player;

    // Use this for initialization
    void Awake()
    {
        player = GetComponentInParent<PlayerMovement>();
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag != "Player")
        {
            player.SetGrounded(true);
        }
    }

}
