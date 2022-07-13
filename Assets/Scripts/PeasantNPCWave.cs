using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class PeasantNPCWave : MonoBehaviour


{
    public ActionBasedController controller;
    public InputActionReference pressB;

    private Animator animator;
    public GameObject exclamation;
    public Canvas peasantCanvas;

    public GameObject button;

    private GameObject npcHeadAim;
    private MultiAimConstraint aim;

    private float targetweight = 0f;

    [SerializeField]
    private GameObject relic;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        peasantCanvas.enabled = true;
        animator.SetBool("IsBow", false);

        // Set NPCHeadAim Rig weight.
        npcHeadAim = GameObject.Find("NPCHeadAim");
        aim = npcHeadAim.GetComponent<MultiAimConstraint>();

        relic.SetActive(false);
        exclamation.SetActive(false);
        button.SetActive(false);
    }

    private void Start()
    {
        pressB.action.performed += Action_performed;
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        peasantCanvas.enabled = false;
        animator.SetBool("IsBow", true);
        StartCoroutine(SetFalse());
        StartCoroutine(SetTrue());
    }

    private void Update()
    {
        aim.weight = Mathf.Lerp(aim.weight, targetweight, Time.deltaTime * 10f);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            button.SetActive(true);
            exclamation.SetActive(true);
            animator.SetBool("IsWaving", true);
            StartCoroutine(SetFalse());

            //NPC Rig Multi-Constraint change to 1 to follow Player upon trigger.
            targetweight = 1f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("IsWaving", false);
        exclamation.SetActive(false);
        button.SetActive(false);
        targetweight = 0f;
    }

    IEnumerator SetFalse()
    {
        yield return new WaitForSeconds(2); // wait for 2 seconds.
        animator.SetBool("IsWaving", false);
        animator.SetBool("IsBow", false);

    }

    IEnumerator SetTrue()
    {
        yield return new WaitForSeconds(2.5f); // wait for 5 seconds.
        peasantCanvas.enabled = true;
        relic.SetActive(true);
    }
}
