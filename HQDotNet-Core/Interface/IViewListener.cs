using System;
using HQDotNet.Model;

namespace HQDotNet {
    public interface IViewListener<TViewType> : IDispatchListener 
        where TViewType : HQView{

        void OnViewUpdated(TViewType view);

    }
}