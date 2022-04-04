using System;
using System.Collections.Generic;
using HQ.Model;

namespace HQ.View {

    /// <summary>
    /// HQView binds a given view and view model together, representing all the data
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public class HQModelCollectionView<TModel, TViewModel, TView> : 
        HQView<TModel, TViewModel>, IModelCollectionListener<TModel>
        where TModel : HQModel
        where TViewModel : HQModelCollectionViewModel
        where TView : HQView<TViewModel>
        {

        protected HQModelCollection<TModel> _modelCollection;

        Dictionary<TModel, List<TView>> _modelViewMap = new Dictionary<TModel, List<TView>>();

        public override bool Render() {
            return base.Render();
        }

        void IModelCollectionListener<TModel>.OnModelAdded(TModel model) {
            throw new NotImplementedException();
        }

        void IModelCollectionListener<TModel>.OnDataUpdated(TModel model) {
            throw new NotImplementedException();
        }
    }
}
