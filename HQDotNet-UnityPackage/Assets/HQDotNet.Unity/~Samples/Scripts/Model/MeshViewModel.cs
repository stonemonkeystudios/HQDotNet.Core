using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Model {

    [CreateAssetMenu(fileName ="MeshViewModel", menuName = "HQDotNet/Model/Mesh View Model")]
    public class MeshViewModel : ScriptableObject {
        public Mesh mesh;
        public Material material;
    }
}