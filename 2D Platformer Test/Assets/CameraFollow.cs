using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : Player
{

    // Update is called once per frame
    void Update()
    {
        if (this.isLocalPlayer)
        {
            transform.position = cameraPoint.position;
        }
    }
}
