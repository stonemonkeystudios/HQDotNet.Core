using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HQDotNet.Unity.Model;

namespace HQDotNet.Unity {
    public class SyncedSperesController : HQController, IModelListener<MeshDemoSettings> {

        [HQInject]
        private TransformCollectionService _transformCollectionService;

        private System.DateTime nextSpawnTime = new System.DateTime();

        private MeshDemoSettings _settings;

        public override void Update() {
            if (_settings == null)
                return;

            if (System.DateTime.Now > nextSpawnTime) {
                var pos = Random.insideUnitSphere * _settings.randomRadius;
                float fScale = Random.Range(_settings.minScale, _settings.maxScale);
                var scale = new Vector3(fScale, fScale, fScale);


                Matrix4x4 newMatrix = Matrix4x4.identity;
                newMatrix.SetTRS(pos, Quaternion.identity, scale);
                _transformCollectionService.Add(newMatrix);

                nextSpawnTime = System.DateTime.Now.AddSeconds(_settings.waitBetweenSpawn);
            }

            base.Update();
        }

        void IModelListener<MeshDemoSettings>.OnModelUpdated(ref MeshDemoSettings model) {
            _settings = model;
        }
    }
}


