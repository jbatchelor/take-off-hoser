using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] AudioClip mainEngine;
    [SerializeField] float forceY = 1f;
    [SerializeField] float MAX_THRUST = 100f;
    [SerializeField] float MAX_ROTATE = 20f;

    Rigidbody _rocketBody;
    AudioSource _thrusterAudio;

    // Start is called before the first frame update
    void Start()
    {
        _rocketBody = GetComponent<Rigidbody>();
        _thrusterAudio = GetComponent<AudioSource>();
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
            if (!_thrusterAudio.isPlaying) _thrusterAudio.PlayOneShot(mainEngine);
            float newThrustY = forceY * Time.deltaTime * MAX_THRUST;
            _rocketBody.AddRelativeForce(0, newThrustY, 0);
        }
        else
        {
            if (_thrusterAudio.isPlaying) _thrusterAudio.Stop();
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

    void ApplyRotation(float frameRotation)
    {
        // freeze rotation so we can manually rotate
        _rocketBody.freezeRotation = true;
        float newAngle = frameRotation * Time.deltaTime * MAX_ROTATE;
        transform.Rotate(0, 0, newAngle);
        _rocketBody.freezeRotation = false;
    }
}
