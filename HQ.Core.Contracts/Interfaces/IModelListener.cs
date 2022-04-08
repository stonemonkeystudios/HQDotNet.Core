using System;
using HQDotNet.Model;

namespace HQDotNet {
    public interface IModelListener<TModelType> : IDispatchListener where TModelType : HQDataModel {

        void OnModelUpdated(TModelType model);

    }
}