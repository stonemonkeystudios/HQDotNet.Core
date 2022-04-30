using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity {
    public class SyncedSperesController : HQController {
        //Should be in a model, but it's a simple test;
        public float waitBetweenSpawn = .5f;
        public int numberToSpawn = 1;
        public float randomRadius = 10f;
        public float minScale = .5f;
        public float maxScale = 2f;

        private System.DateTime nextSpawnTime;

        [HQInject]
        private TransformCollectionService _transformCollectionService;

        public SyncedSperesController() {
            nextSpawnTime = System.DateTime.Now.AddSeconds(waitBetweenSpawn);
        }

        public override void Update() {

            if (System.DateTime.Now > nextSpawnTime) {
                var pos = Random.insideUnitSphere * randomRadius;
                float fScale = Random.Range(minScale, maxScale);
                var scale = new Vector3(fScale, fScale, fScale);


                Matrix4x4 newMatrix = Matrix4x4.identity;
                newMatrix.SetTRS(pos, Quaternion.identity, scale);
                _transformCollectionService.Add(newMatrix);

                nextSpawnTime = System.DateTime.Now.AddSeconds(waitBetweenSpawn);
            }


            base.Update();
        }
    }
}


