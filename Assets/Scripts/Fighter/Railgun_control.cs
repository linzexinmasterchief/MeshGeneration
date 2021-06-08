using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun_control : MonoBehaviour
{
    public Transform emitter_pos;
    public GameObject bullet;
    public float damage;

    // 0: not ready, 1: ready
    private int fire_status;
    private float cooldown_time;
    private bool auto_fire;

    // Start is called before the first frame update
    void Start()
    {
        // ready
        fire_status = 1;
        // clear cooldown
        cooldown_time = 0;
        // switch off auto fire
        auto_fire = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (cooldown_time > 0)
        {
            cooldown_time -= Time.deltaTime;
        }
        else
        {
            cooldown_time = 0;
            fire_status = 1;
        }

        if (auto_fire)
        {
            if (fire_status == 1)
            {
                Fire();
            }
        }
    }

    /// <summary>
    /// Control game objects to finish one fire action
    /// </summary>
    private void Fire()
    {
        // change status to not ready (cooling)
        fire_status = 0;
        // initiate cooldown time
        cooldown_time = 1;
        GameObject b = Instantiate(bullet);
        b.transform.position = emitter_pos.position;
        b.transform.localScale = new Vector3(0.1f, 0.1f, 3f);
        b.transform.rotation = transform.rotation;
        b.GetComponent<Rigidbody>().AddForce(b.transform.forward * 1000, ForceMode.Impulse);
    }
    
    /// <summary>
    /// Call Fire() function once
    /// </summary>
    public void Fire_once()
    {
        Fire();
    }

    /// <summary>
    /// Switch to auto continue fire mode
    /// </summary>
    public void Auto_fire()
    {
        Fire();
        auto_fire = true;
    }

    public int Get_fire_status()
    {
        return fire_status;
    }

    public float Get_cooldown_time()
    {
        return cooldown_time;
    }
}
