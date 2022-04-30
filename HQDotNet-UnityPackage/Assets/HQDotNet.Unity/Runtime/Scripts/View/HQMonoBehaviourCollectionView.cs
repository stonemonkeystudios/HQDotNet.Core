using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HQDotNet.View;

namespace HQDotNet.Unity
{
    public class HQMonoBehaviourCollectionView : HQMonoBehaviourView, IModelCollectionListener<ICollection<object>,object>  {
        protected Dictionary<object, HQMonoBehaviourView> _modelViewBindings = new Dictionary<object, HQMonoBehaviourView>();

        new HQModelCollectionView<ICollection<object>, object, HQView> view;

        void Awake() {
            if (HQSession.HasSession) {
                view = HQSession.Current.RegisterView<HQModelCollectionView<ICollection<object>, object, HQView>>();
            }
        }

        public virtual void OnModelAdded(ref object model) {

        }

        public virtual void OnModelDeleted(ref object model) {
            if (_modelViewBindings.ContainsKey(model)) {
                Destroy(_modelViewBindings[model]);
                _modelViewBindings.Remove(model);
            }
        }

        public virtual void OnModelUpdated(ref object model) {
        }
    }
}
