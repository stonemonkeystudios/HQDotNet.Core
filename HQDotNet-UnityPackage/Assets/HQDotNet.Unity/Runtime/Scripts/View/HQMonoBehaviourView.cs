using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HQDotNet;

namespace HQDotNet.Unity
{
    public class HQMonoBehaviourView : MonoBehaviour, IViewListener<HQView>
    {
        protected HQView view;
        protected bool isDirty = false;

        public void Render() {

        }

        public void SetView<TView>(TView view) where TView : HQView, new(){
            this.view = view;
        }

        public virtual void OnViewUpdated(HQView view) {
        }

        void LateUpdate() {
            if (isDirty) {
                Render();
                isDirty = false;
            }
        }
    }
}
