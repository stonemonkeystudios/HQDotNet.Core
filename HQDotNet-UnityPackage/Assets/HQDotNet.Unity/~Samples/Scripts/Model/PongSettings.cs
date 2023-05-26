using UnityEngine;
using UnityEditor;

namespace HQDotNet.Unity.Pong {
    [CreateAssetMenu(fileName = "PongSettings", menuName = "HQDotNet/Pong/Pong Settings", order = 51)]
    public class PongSettings : ScriptableObject {
        [SerializeField]
        private float paddleSpeed = 5f;

        [SerializeField]
        private float startingBallSpeed = 5f;

        public float PaddleSpeed {
            get { return paddleSpeed; }
            set { paddleSpeed = value; }
        }

        public float StartingBallSpeed {
            get { return startingBallSpeed; }
            set { startingBallSpeed = value; }
        }
    }
}