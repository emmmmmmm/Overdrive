using UnityEngine;

public class Spring
{
    // Spring simulation constants

    public float M = 1f; // Mass
    public float K = 0.2f;// spring constant
    public float D = 0.17f; // damping
    public float R = 0f; // rest position
    // Spring simulation variables
    public float pos = 0f;    // Position
    private float vel = 0f;  // Velocity
    private float ac = 0f;    // Acceleration
    private float f = 0f;     // Force

    public float Update(float _pos)
    {
        R = _pos;
        f = -K * (pos - R);    // f=-ky
        ac = f / M;           // Set the acceleration, f=ma == a=f/m
        vel = (1f-D) * (vel + ac);   // Set the velocity
        pos = pos + vel;         // Updated position

        if (Mathf.Abs(vel) < 0.1)
        {
            vel = 0.0f;
        }
        return pos;
    }
}

public class Spring3 {
    public float M = 1f;
    public float K = 0.2f;
    public Vector3 D = new Vector3(0.95f, 0.95f, 0.95f);
    public Vector3 R = new Vector3();

    public Vector3 pos = new Vector3();
    private Vector3 vel = new Vector3();
    private Vector3 ac = new Vector3();
    private Vector3 f = new Vector3();

    public Vector3 Update(Vector3 _pos)
    {
        R = _pos;
        f = -K * (pos - R);    // f=-ky
        ac = f / M;           // Set the acceleration, f=ma == a=f/m
        vel = Vector3.Scale( (Vector3.one-D) ,(vel + ac));   // Set the velocity

        pos = pos + vel;         // Updated position

       
        return pos;
    }
}

