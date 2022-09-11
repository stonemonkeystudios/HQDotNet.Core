using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Samples {

    /// <summary>
    /// Transforms a lerped time into an actionable translation, then applies the transformation to this GameObject's Transform
    /// </summary>
    public class LerpExampleView : HQMonoView, ILerpListener {
        private Vector3 startingPosition;

        /// <summary>
        /// Set the starting position of this transform
        /// </summary>
        private void Awake() {
            startingPosition = transform.position;
            base.Awake();
        }

        /// <summary>
        /// Move the transform in the x direction according to our transformed lerp time
        /// This class is automatically registered for dispatch in Awake, so it receives this message from HQ
        /// </summary>
        /// <param name="lerpValue"></param>
        void ILerpListener.Lerp(float lerpValue) {
            Vector3 move = new Vector3(lerpValue, 0, 0);
            transform.position = startingPosition + move;
        }
    }

}