using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalAnimation : MonoBehaviour
{

    private Animator animator;

    //******************************************************************************
    // Declare all strings used within the script
    private string relic = "Relic";
    private string isTriggered = "IsTriggered";
    private string crystalAnimation = "CrystalAnimation";
    private string cutoffHeight = "_Cutoff_Height";
    //******************************************************************************

    private int relicCount = 0;

    // Objects to Destroy after triggered crystal Animation
    [SerializeField]
    private GameObject rayLight;
    [SerializeField]
    private GameObject relicInWell;

    [SerializeField]
    private Renderer whiteGroundMaterial;
    [SerializeField]
    private Renderer texturedGroundMaterial;

    private float currentWhiteDissolveValue = 0.8f;
    private float targetWhiteDissolveValue = -3f;
    
    private float currentTexturedDissolveValue = -0.8f;
    private float targetTexturedDissolveValue = 3.0f;

    private AudioSource audioSource;
    private AudioClip whoosh;

    private float time = 5f;

    private bool isPlaying = false;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        whoosh = audioSource.clip;

        whiteGroundMaterial.material.SetFloat(cutoffHeight, currentWhiteDissolveValue);
        texturedGroundMaterial.material.SetFloat(cutoffHeight, currentTexturedDissolveValue);
    }

    private void Start()
    {
        
    }

    void Update()
    {
        if (relicCount == 4)
        {
            rayLight.SetActive(true);
            relicInWell.SetActive(true);
            animator.SetBool(isTriggered, true);
            relicCount = 0;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(crystalAnimation) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
        {
            //dissolve = true;
            animator.SetBool(isTriggered, false);
            rayLight.SetActive(false);
            relicInWell.SetActive(false);
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName(crystalAnimation) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f)
        {
            Dissolve();
            if(!isPlaying)
            {
                audioSource.PlayOneShot(whoosh, 2.5f);
            }
            isPlaying = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(relic))
        {
            relicCount++;
            other.enabled = false;
        }
    }

    void Dissolve()
    {
        _= time - Time.deltaTime;
        while(time > 0)
        {
            currentWhiteDissolveValue = Mathf.Lerp(currentWhiteDissolveValue, targetWhiteDissolveValue, 0.2f * Time.smoothDeltaTime);
            whiteGroundMaterial.material.SetFloat(cutoffHeight, currentWhiteDissolveValue);

            currentTexturedDissolveValue = Mathf.Lerp(currentTexturedDissolveValue, targetTexturedDissolveValue, 0.2f * Time.smoothDeltaTime);
            texturedGroundMaterial.material.SetFloat(cutoffHeight, currentTexturedDissolveValue);
            break;
        }

    }
}
