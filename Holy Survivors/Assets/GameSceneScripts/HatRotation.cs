using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatRotation : MonoBehaviour
{
    public GameObject camObj;
    private float x;
    private float y;
    void Update()
    {
        x = camObj.transform.localEulerAngles.x - 90;
        x = Mathf.Clamp(x, -90, 360);

        y = transform.parent.eulerAngles.y;

        if(x >= -90 && x < 0 || x == 0)
        {
            // Do nothing, be happy
        }
        else
        {
            transform.eulerAngles = new Vector3(x, y, 0);
        }
    }
}
