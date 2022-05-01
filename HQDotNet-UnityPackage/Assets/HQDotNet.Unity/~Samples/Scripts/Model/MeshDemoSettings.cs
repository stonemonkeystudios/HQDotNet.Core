using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Model {

    [CreateAssetMenu(fileName ="SphereDemoSettings", menuName = "HQDotNet/Model/Sphere Demo Settings")]
    public class MeshDemoSettings : ScriptableObject {
        public float waitBetweenSpawn = .5f;
        public int numberToSpawn = 1;
        public float randomRadius = 10f;
        public float minScale = .5f;
        public float maxScale = 2f;
    }
}