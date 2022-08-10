using UnityEngine;
using UnityEngine.Assertions;

namespace HQDotNet.Unity
{
    public class HQPrefabCollectionView : HQMonoBehaviourCollectionView{
        public GameObject prefab;

        public override void OnModelAdded(ref object model) {
            Assert.IsNotNull(model);

            GameObject go = Instantiate(prefab);
            var monoView = go.GetComponent<HQMonoBehaviourView>();
            if (monoView == null) {
                go.AddComponent<HQMonoBehaviourView>();
            }
            _modelViewBindings.Add(model, monoView);
            monoView.UpdateModel(ref model);
            Debug.Log("Added a new view for model named " + go.name);
            base.OnModelAdded(ref model);
        }

        public override void OnModelUpdated(ref object model) {
            base.OnModelUpdated(ref model);
            var monoView = _modelViewBindings[model];
            if (monoView == null) {
            }

            monoView.UpdateModel(ref model);

        }

        public override void OnModelDeleted(ref object model) {
            Assert.IsNotNull(model);
            Assert.IsTrue(_modelViewBindings.ContainsKey(model));

            var monoView = _modelViewBindings[model];
            Assert.IsNotNull(monoView);

            _modelViewBindings.Remove(model);
            Destroy(monoView);

            base.OnModelDeleted(ref model);
        }

        private void Start() {
            Assert.IsNotNull(prefab);
        }
    }
}
