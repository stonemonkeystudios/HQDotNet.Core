using System;
using System.Collections.Generic;
using HQDotNet.Model;

namespace HQDotNet.View {
    public class HQModelCollectionView<TCollection, TModel, TView> : HQView, IModelCollectionListener<TCollection, TModel>
        where TCollection : ICollection<TModel>
        where TView : HQView, IModelListener<TModel>, new(){
        private Dictionary<TModel, TView> _modelViewBindings = new Dictionary<TModel, TView>();

        public void OnModelAdded(ref TModel model){
            if (_modelViewBindings.ContainsKey(model)){
                Console.WriteLine("View Collection already contains a view for this model.");
                return;
            }
            else{
                TView view = new TView();
                _modelViewBindings.Add(model, view);
            }
        }

        public void OnModelDeleted(ref TModel model){
            if (!_modelViewBindings.ContainsKey(model)) {
                Console.WriteLine("No view is registered for this model.");
                return;
            }
            else {
                _modelViewBindings.Remove(model);
            }
        }

        public void OnModelUpdated(ref TModel model){
            //Update the specific view associated with this model
            if (!_modelViewBindings.ContainsKey(model)) {
                Console.WriteLine("No view is registered for this model.");
                return;
            }
            else {
                _modelViewBindings[model].OnModelUpdated(ref model);
            }
        }
    }
}
