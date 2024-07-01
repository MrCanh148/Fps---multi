using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public Moving moving;
    public GameObject Camera;


    public void IsLocalPlayer()
    {
        moving.enabled = true;
        Camera.SetActive(true);
    }
}


