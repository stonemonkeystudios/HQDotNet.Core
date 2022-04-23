using HQDotNet.Model;
using System;

namespace HQDotNet {

    /// <summary>
    /// HQView binds a given view and view model together, representing all the data
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public class HQExternalView : HQView{
        public Action<HQExternalView> onViewUpdated;
        public bool IsDirty { get; set; }

        public override bool Render() {
            onViewUpdated?.Invoke(this);
            return true;
        }

        public override bool Startup() {
            return Render() && base.Startup();
        }
    }
}
