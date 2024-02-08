using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody _rocketBody;
    [SerializeField] float forceX = 0f;
    [SerializeField] float forceY = 1f;
    [SerializeField] float forceZ = 0f;

    [SerializeField] float MAX_THRUST = 100f;
    [SerializeField] float MAX_ROTATE = 20f;

    // Start is called before the first frame update
    void Start()
    {
        _rocketBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float newThrustY = forceY * Time.deltaTime * MAX_THRUST;
            _rocketBody.AddRelativeForce(forceX, newThrustY, forceZ);
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(1f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-1f);
        }
    }

    private void ApplyRotation(float frameRotation)
    {
        // freeze rotation so we can manually rotate
        _rocketBody.freezeRotation = true;
        float newAngle = frameRotation * Time.deltaTime * MAX_ROTATE;
        transform.Rotate(0, 0, newAngle);
        _rocketBody.freezeRotation = false;
    }
}
