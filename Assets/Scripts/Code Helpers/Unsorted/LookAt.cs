using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace h1ddengames {
	public class LookAt : MonoBehaviour {
        #region Exposed Fields
        [SerializeField] private Transform target;
        [SerializeField] private bool isTargetPlayer = false;
		#endregion

		#region Private Fields
		#endregion

		#region Unity Methods
		void Start() {
			if(isTargetPlayer) {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
		}

		void Update() {
            Vector3 relativePos = target.position - transform.position;
            relativePos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }
        #endregion

        #region My Methods

        #endregion

        #region Helper Methods

        #endregion
    }
}