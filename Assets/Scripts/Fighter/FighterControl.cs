using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]

public class FighterControl : MonoBehaviour
{

    private Rigidbody rig;

    public float speed;
    public float wind_angle;
    private float thrust;
    // thrust_min is the maximum backward thrust, should be less than 0 since backward
    public float thrust_max, thrust_min;
    public float thrust_friction;

    public float roll_power;
    public float roll_friction;

    public float pitch_power;
    public float pitch_friction;

    public float upward_friction;

    private float mouse_input_x, mouse_input_y;
    private float mouse_force_x, mouse_force_y;
    public float mouse_force_max;
    public float mouse_force_multiplier;

    public ParticleSystem cloud_left_wing;
    public ParticleSystem cloud_right_wing;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rig = GetComponent<Rigidbody>();
        rig.velocity = transform.forward * speed;

        cloud_left_wing.emissionRate = 0;
        cloud_right_wing.emissionRate = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ThrustControl();

        speed = rig.velocity.magnitude;

        wind_angle = CalculateWindAngle();
        if (wind_angle < 0.87)
        {
            cloud_left_wing.emissionRate = (1 - wind_angle) * speed;
            cloud_right_wing.emissionRate = (1 - wind_angle) * speed;
        } else
        {
            cloud_left_wing.emissionRate = 0;
            cloud_right_wing.emissionRate = 0;
        }
    }

    private void FixedUpdate()
    {
        Thrust();
        RollControl();
        PitchControl();
        Friction();
        AimControl();

        LiftForce();
    }

    float CalculateWindAngle()
    {
        // return cos(x)
        return 1 - Mathf.Abs(Vector3.Dot(transform.up, rig.velocity.normalized));
    }

    void LiftForce()
    {
        float lift = Mathf.Pow(speed, 2);
        //rig.AddForce(transform.up * lift);
        if (lift < rig.mass * Physics.gravity.magnitude)
        {
            rig.AddForce(transform.up * lift);
        }
        else
        {
            rig.AddForce(transform.up * rig.mass * Physics.gravity.magnitude);
        }
    }

    void AimControl()
    {
        // mouse
        mouse_input_x = Input.GetAxis("Mouse X") * mouse_force_multiplier * Time.deltaTime;
        if (mouse_input_x == 0)
        {
            if (Mathf.Abs(mouse_force_x) < 0.3f)
            {
                mouse_force_x = 0;
            }
        }
        else
        {
            mouse_force_x += mouse_input_x;
        }


        mouse_input_y = Input.GetAxis("Mouse Y") * mouse_force_multiplier * Time.deltaTime;
        if (mouse_input_y == 0)
        {
            if (Mathf.Abs(mouse_force_y) < 0.3f)
            {
                mouse_force_y = 0;
            }
        }
        else
        {
            // inverse y is more comfortable for me
            mouse_force_y -= mouse_input_y;
        }

        if (Mathf.Abs(mouse_force_x) > 0)
        {
            if (mouse_force_x > mouse_force_max)
            {
                mouse_force_x = mouse_force_max;
            }
            else if (mouse_force_x < -mouse_force_max)
            {
                mouse_force_x = -mouse_force_max;
            }

        }

        if (Mathf.Abs(mouse_force_y) > 0)
        {
            if (mouse_force_y > mouse_force_max)
            {
                mouse_force_y = mouse_force_max;
            }
            else if (mouse_force_y < -mouse_force_max)
            {
                mouse_force_y = -mouse_force_max;
            }

        }

        // verticle rotate
        rig.AddForceAtPosition(-transform.up * mouse_force_y * mouse_force_y * Mathf.Sign(mouse_force_y), transform.position + transform.forward * 20);
        rig.AddForceAtPosition(transform.up * mouse_force_y * mouse_force_y * Mathf.Sign(mouse_force_y), transform.position - transform.forward * 20);
        // horizontal rotate
        rig.AddForceAtPosition(transform.right * mouse_force_x * mouse_force_x * Mathf.Sign(mouse_force_x), transform.position + transform.forward * 10);
        rig.AddForceAtPosition(-transform.right * mouse_force_x * mouse_force_x * Mathf.Sign(mouse_force_x), transform.position - transform.forward * 10);

    }

    void ThrustControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            thrust = thrust_max;
        } else if (Input.GetKey(KeyCode.S))
        {
            thrust = thrust_min;
        } else
        {
            thrust = 0;
        }
    }

    void RollControl()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rig.AddTorque(-transform.forward * roll_power);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rig.AddTorque(transform.forward * roll_power);
        }
    }

    void PitchControl()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            rig.AddTorque(-transform.right * pitch_power);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            rig.AddTorque(transform.right * pitch_power);
        }
    }

    void Thrust()
    {
        // engine thrust
        rig.AddForce(transform.forward * thrust);
    }

    void Friction()
    {
        // Movement forward
        rig.AddForce(-0.01f * rig.velocity * Vector3.Dot(rig.velocity, transform.forward) * thrust_friction);

        // Movement upward
        rig.AddForce(-transform.up * Vector3.Dot(rig.velocity, transform.up) * upward_friction);

        // roll and pitch
        rig.AddTorque(-rig.angularVelocity * roll_friction);

    }

}
