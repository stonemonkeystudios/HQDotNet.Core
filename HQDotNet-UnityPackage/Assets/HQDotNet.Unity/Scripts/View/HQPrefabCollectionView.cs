using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace HQDotNet.Unity
{
    public class HQPrefabCollectionView : HQMonoView{
        public GameObject prefab;
        public GameObject localObject;
        protected Dictionary<object, HQMonoView> modelMonoViews = new Dictionary<object, HQMonoView>(); 

        public GameObject Spawn() {
            GameObject go = Instantiate(prefab);
            var monoView = go.GetComponent<HQMonoView>();
            if (monoView == null) {
                go.AddComponent<HQMonoView>();
            }
            return go;
        }

        public virtual void Dispose(object model) {
            if (!modelMonoViews.ContainsKey(model)) {
                Debug.LogError("Model to delete was not found.");
                return;
            }
            Destroy(modelMonoViews[model].gameObject);
            modelMonoViews.Remove(model);
        }

        private void Start() {
            Assert.IsNotNull(prefab);
        }
    }
}
