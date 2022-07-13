using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDoorOpen : MonoBehaviour
{
    private readonly string player = "Player";
    private readonly string isOpen = "IsOpen";

    private Animator animator;

    [SerializeField]
    private AudioSource audiosource;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool(isOpen, false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(player))
        {
            animator.SetBool(isOpen, true);
            audiosource.PlayOneShot(audiosource.clip, 0.8f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool(isOpen, false);
        audiosource.PlayOneShot(audiosource.clip, 0.8f);
    }
}
