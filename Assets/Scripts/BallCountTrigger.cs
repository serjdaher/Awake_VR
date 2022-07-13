using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCountTrigger : MonoBehaviour
{
    private Animator animator;
    private int ballCount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ballCount == 5)
        {

        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("IsEnabled", true);
            ballCount++;
        }
        
    }
}
