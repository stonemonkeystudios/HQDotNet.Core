using HQDotNet.Model;
using System;

namespace HQDotNet {

    /// <summary>
    /// HQView binds a given view and view model together, representing all the data
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public class HQView : HQCoreBehavior, IModelListener<HQViewModel>, IModelListener<HQDataModel>{
        protected bool IsDirty { get; set; }

        public virtual bool Render() {
            return true;
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

        void IModelListener<HQViewModel>.OnModelUpdated(HQViewModel model) {
            Render();
        }

        void IModelListener<HQDataModel>.OnModelUpdated(HQDataModel model) {
            Render();
        }
    }
}
