using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BlobbyNav
{
    public class BlobbyNavMesh : MonoBehaviour
    {

        [SerializeField]
        private Transform movePositionTransform;
        public NavMeshAgent navMeshAgent;

        [SerializeField]
        private GameObject player;

        [SerializeField]
        private GameObject floaty;



        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = floaty.transform.rotation;
            navMeshAgent.SetDestination(movePositionTransform.position);
        }

        //void RotateTowards(GameObject target)
        //{
        //    var lookPos = target.transform.position - transform.position;
        //    lookPos.y = 0;
        //    var targetRotation = Quaternion.LookRotation(lookPos);
        //    transform.rotation = targetRotation;
        //}

    }
}


