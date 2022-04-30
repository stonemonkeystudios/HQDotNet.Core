using UnityEngine;
using UnityEngine.Assertions;

namespace HQDotNet.Unity
{
    public class HQPrefabCollectionView : HQMonoBehaviourCollectionView{
        public GameObject prefab;

        public override void OnModelAdded(object model) {
            Assert.IsNotNull(model);

            GameObject go = Instantiate(prefab);
            var monoView = go.GetComponent<HQMonoBehaviourView>();
            if (monoView == null) {
                go.AddComponent<HQMonoBehaviourView>();
            }
            _modelViewBindings.Add(model, monoView);
            Debug.Log("Added a new view for model named " + go.name);
        }

        private void Start() {
            Assert.IsNotNull(prefab);
        }
    }
}
