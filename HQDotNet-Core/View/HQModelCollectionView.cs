using System;
using System.Collections.Generic;
using HQDotNet.Model;

namespace HQDotNet.View {

    /// <summary>
    /// HQView binds a given view and view model together, representing all the data
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    /*public class HQModelCollectionView<TModel, TViewModel, TView> : 
        HQView<TModel, TViewModel>, IModelCollectionListener<TModel>
        where TModel : HQDataModel, new()
        where TViewModel : HQModelCollectionViewModel, new()
        where TView : HQView<TModel,TViewModel>, new()
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
    }*/
}
