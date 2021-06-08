using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor_UI : MonoBehaviour
{

    /// <summary>
    /// Updating cursor icon on the hud
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Cursor_update(float x, float y)
    {
        float magnitude = Mathf.Sqrt(x * x + y * y) * 5;
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, magnitude, 1), Time.deltaTime);
        Debug.Log(magnitude);
        if (x > 0)
        {
            float rotation = Mathf.Atan2(y, x) / Mathf.PI * 180;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(rotation, new Vector3(0, 0, 1)), Time.deltaTime);
        }
        else
        {
            float rotation = Mathf.Atan2(y, x) / Mathf.PI * 180;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(rotation, new Vector3(0, 0, 1)), Time.deltaTime);
        }
        
    }
}
