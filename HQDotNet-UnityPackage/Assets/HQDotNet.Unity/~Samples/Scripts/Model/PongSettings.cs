using UnityEngine;
using UnityEditor;

namespace HQDotNet.Unity.Pong {
    [CreateAssetMenu(fileName = "PongSettings", menuName = "HQDotNet/Pong/Pong Settings", order = 51)]
    public class PongSettings : ScriptableObject {
        [SerializeField]
        private int winningScore;

        [SerializeField]
        private float paddleSpeed = 5f;

        [SerializeField]
        private float startingBallSpeed = 5f;

        [SerializeField]
        private float maxPaddleYPosition = 2f;

        public float PaddleSpeed {
            get { return paddleSpeed; }
            set { paddleSpeed = value; }
        }

        public float StartingBallSpeed {
            get { return startingBallSpeed; }
            set { startingBallSpeed = value; }
        }

        public int WinningScore {
            get { return winningScore; }
            set { winningScore = value; }
        }

        public float MaxPaddleYPosition {
            get { return maxPaddleYPosition; }
            set { maxPaddleYPosition = value; }
        }
    }
}