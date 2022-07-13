using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateText : MonoBehaviour
{
    public float speed = 50f;
    public bool IsColliding = false;

    private void Start()
    {
        IsColliding = false;
    }
    void Update()
    {
        RotateCube();
    }

    void RotateCube()
    {
        if (IsColliding == false)
        {
            transform.Rotate(Vector3.back, speed * Time.deltaTime);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Cannonball"))
        {
            IsColliding = true;
        }
    }
}
