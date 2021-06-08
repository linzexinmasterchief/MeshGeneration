using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject target;
    public float follow_delta_time;
    public bool follow_pos;
    public Vector3 follow_delta_pos;
    private Vector3 mouse_control_delta_pos;
    public bool enable_mouse_control;
    private bool is_mouse_controling;
    private Vector3 delta_pos;
    private Quaternion delta_rot;
    private float x, y;
    // Start is called before the first frame update
    void Start()
    {
        delta_pos = target.transform.position - transform.position;
        // delta_pos = target.transform.forward * follow_delta_pos.z - target.transform.up * follow_delta_pos.y;
        mouse_control_delta_pos = delta_pos;
        delta_rot = target.transform.rotation * transform.rotation;
        if (enable_mouse_control)
        {
            Cursor.lockState = CursorLockMode.Locked;
            
        }
        is_mouse_controling = false;
    }

    private void FixedUpdate()
    {
        if (follow_pos)
        {
            if (enable_mouse_control)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    is_mouse_controling = !is_mouse_controling;
                }
                if (is_mouse_controling) {
                    x += Input.GetAxis("Mouse X") * 50 * Time.deltaTime;
                    // inverse y is more comfortable for me
                    y -= Input.GetAxis("Mouse Y") * 100 * Time.deltaTime;
                    Quaternion q = Quaternion.Euler(y, x, 0);
                    Vector3 direction = q * mouse_control_delta_pos;
                    this.transform.position = target.transform.position + target.transform.forward * 10 - direction;
                } else
                {
                    //相机的位置
                    Vector3 targetPos = target.transform.position + Vector3.up * delta_pos.y - target.transform.forward * delta_pos.z;
                    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * follow_delta_time);
                    x = 0;
                    y = 0;
                }
            }
            else
            {
                //相机的位置
                Vector3 targetPos = target.transform.position - Vector3.up * delta_pos.y - target.transform.forward * delta_pos.z;
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * follow_delta_time);
            }
        }
        //相机的角度
        Quaternion targetRot = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * follow_delta_time);
        //transform.LookAt(target.transform.position + target.transform.forward * follow_delta_time + target.transform.up * 2f);

        float fov = 60 + target.GetComponent<Rigidbody>().velocity.magnitude / 10f;
        if (fov > 100)
        {
            fov = 100;
        }
        GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, fov, 0.1f);
    }
    // Update is called once per frame
    void Update()
    {
        //transform.position = target.transform.position - delta_pos;
        //transform.RotateAround(target.transform.position + new Vector3(0, 0, 1), target.transform.rotation.ToEuler(), 1);
        
    }
}
