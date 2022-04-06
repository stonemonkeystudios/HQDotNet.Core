using HQ.Model;
using System;

namespace HQ.View {

    /// <summary>
    /// HQView binds a given view and view model together, representing all the data
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public class HQView<TModel, TViewModel> : HQSingletonBehavior<HQBehaviorModel>, IViewModelListener<TViewModel>, IModelListener<TModel>
        where TModel : HQModel
        where TViewModel : HQViewModel {

        protected bool IsDirty { get; set; }

        public virtual bool Render() {
            return true;
        }

        void IModelListener<TModel>.OnModelUpdated(TModel model) {
            IsDirty = true;
        }

        void IViewModelListener<TViewModel>.ViewModelUpdated(TViewModel viewModel) {
            IsDirty = true;
        }

        public override bool Startup() {
            return Render() && base.Startup();
        }

        public override void Update() {
            if (IsDirty) {
                Render();
            }

            base.Update();
        }
    }
}
