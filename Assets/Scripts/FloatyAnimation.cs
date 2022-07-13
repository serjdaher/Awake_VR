using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;


namespace FloatyMovement
{
    public class FloatyAnimation : MonoBehaviour
    {
        public ActionBasedController controller;

        private int count = 0;

        [SerializeField]
        private Animator blobbyAnimator;
        private Animator floatyAnimator;

        [SerializeField]
        private GameObject target;
        [SerializeField]
        private GameObject target2;
        private GameObject headAim;

        [SerializeField]
        private GameObject mainPlayer;

        private NavMeshAgent navMeshFloaty;

        private MultiAimConstraint headAimWeight;

        [SerializeField]
        private GameObject button;
        [SerializeField]
        private GameObject speechBubble1;
        [SerializeField]
        private GameObject speechBubble2;

        // Distance between NavMeshTarget and Floaty.
        private float distanceBetweenTarget = 0f;

        // Distance between Second NavMeshTarget and Floaty.
        private float distanceBetweenTarget2 = 0f;

        private float targetweight = 0f;

        // ******************************************************
        // Declare all strings here to avoid calling strings.
        private string isWalking = "IsWalking";
        private string isWave = "IsWave";
        private string headAiming = "HeadAim";
        private string playerTag = "Player";
        private string isSwing = "IsSwing";
        private string isTalking = "IsTalking";
        // ******************************************************

        public InputActionReference pressA;

        private bool isRotating = false;
        private bool hasTriggered = false;

        private AudioSource play;
        public AudioClip hey;
        // Start is called before the first frame update
        void Start()
        {
            floatyAnimator = GetComponent<Animator>();

            headAim = GameObject.Find(headAiming);
            headAimWeight = headAim.GetComponent<MultiAimConstraint>();

            floatyAnimator.SetBool(isWalking, false);
            floatyAnimator.SetBool(isTalking, false);

            navMeshFloaty = GetComponent<NavMeshAgent>();
            pressA.action.performed += Action_performed;

            button.SetActive(false);
            speechBubble1.SetActive(false);
            speechBubble2.SetActive(false);

            play = GetComponent<AudioSource>();

        }

        private void Action_performed(InputAction.CallbackContext obj)
        {
            if(hasTriggered)
            {
                count++;
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            var lookPos = mainPlayer.transform.position - transform.position;
            lookPos.y = 0;
            var targetRotation = Quaternion.LookRotation(lookPos);
            Vector3 targetDirection = (mainPlayer.transform.position - transform.position).normalized;

            headAimWeight.weight = Mathf.Lerp(headAimWeight.weight, targetweight, Time.deltaTime * 10f);

            distanceBetweenTarget = Vector3.Distance(target.transform.position, transform.position);
            distanceBetweenTarget2 = Vector3.Distance(target2.transform.position, transform.position);

            if (count == 1 && hasTriggered)
            {
                button.SetActive(false);
                speechBubble1.SetActive(true);
            }
            
            if(speechBubble1.activeSelf == true || speechBubble2.activeSelf == true)
            {
                floatyAnimator.SetBool(isTalking, true);
            }
            else if(speechBubble1.activeSelf == false || speechBubble2.activeSelf == false)
            {
                floatyAnimator.SetBool(isTalking, false);
            }
            if(count == 2 && hasTriggered)
            {
                speechBubble1.SetActive(false);
                navMeshFloaty.SetDestination(target.transform.position);
                floatyAnimator.SetBool(isWalking, true);
            }

            if (Mathf.Round(transform.position.x) == Mathf.Round(target.transform.position.x) && Mathf.Round(transform.position.z) == Mathf.Round(target.transform.position.z) || distanceBetweenTarget < 0.8f)
            {
                RotateToPlayer(targetRotation, targetDirection);
                target.SetActive(false);
                floatyAnimator.SetBool(isWalking, false);
                navMeshFloaty.ResetPath();
                if (hasTriggered)
                {
                    speechBubble2.SetActive(true);
                }
            }

            if (floatyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                floatyAnimator.SetBool(isWave, false);
            }

            if(count == 3 && hasTriggered)
            {
                speechBubble2.SetActive(false);
                navMeshFloaty.SetDestination(target2.transform.position);
                floatyAnimator.SetBool(isWalking, true);
            }

            if (Mathf.Round(transform.position.x) == Mathf.Round(target2.transform.position.x) && Mathf.Round(transform.position.z) == Mathf.Round(target2.transform.position.z) || distanceBetweenTarget2 < 0.8f)
            {
                RotateToPlayer(targetRotation, targetDirection);
                navMeshFloaty.ResetPath();
                floatyAnimator.SetBool(isWalking, false);
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag(playerTag))
            {
                hasTriggered = true;
                blobbyAnimator.SetTrigger(isSwing);
                targetweight = 1.0f;
            }

            if(count == 0 && hasTriggered)
            {
                button.SetActive(true);
                play.PlayOneShot(hey, 0.8f);
            }
            else if(count >=1)
            {
                button.SetActive(false);
            }  
            
            if(count == 0 || count >= 3)
            {
                floatyAnimator.SetTrigger(isWave);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            hasTriggered = false;
            targetweight = 0.0f;
            button.SetActive(false);
            speechBubble1.SetActive(false);
            speechBubble2.SetActive(false);

            if(count <=1)
            {
                count = 0;
            }
        }

        void RotateToPlayer(Quaternion targetRotation, Vector3 targetDirection)
        {
            isRotating = true;
            do
            {
                //targetRotation = Quaternion.LookRotation(targetDirection);
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime).normalized;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
            } while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f && isRotating == true);

            if (Vector3.Dot(transform.TransformDirection(Vector3.forward), targetDirection) > 0.8)
            {
                isRotating = false;
            }
        }
    }
}

