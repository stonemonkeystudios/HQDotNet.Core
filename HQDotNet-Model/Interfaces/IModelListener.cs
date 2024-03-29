using System;
using HQDotNet.Model;

namespace HQDotNet {
    public interface IModelListener<TModelType> : IDispatchListener{

        void OnModelUpdated(TModelType model);

    }
}