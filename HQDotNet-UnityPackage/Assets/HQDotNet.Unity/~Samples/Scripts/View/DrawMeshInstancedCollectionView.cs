using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HQDotNet.Unity.Model;
using HQDotNet;

namespace HQDotNet.Unity {
    public class DrawMeshInstancedCollectionView : HQView, IModelListener<MeshViewModel> {

        [HQInject]
        private TransformCollectionService _transformService;

        private MeshViewModel _model;

        public override void LateUpdate() {
            if (_model == null)
                return;


            int i = 0;
            while (i < _transformService.LocalCollection.Count) {
                int end = i + 1024;
                if(_transformService.LocalCollection.Count < end) {
                    end = _transformService.LocalCollection.Count;
                }
                Graphics.DrawMeshInstanced(_model.mesh, 0, _model.material, _transformService.LocalCollection.GetRange(i, end - i - 1));
                i = end;
            }


            //Graphics.DrawMeshInstanced(_model.mesh, 0, _model.material, _transformService.LocalCollection.ToArray());

            base.LateUpdate();
        }

        void IModelListener<MeshViewModel>.OnModelUpdated(ref MeshViewModel model) {
            _model = model;
        }
    }
}