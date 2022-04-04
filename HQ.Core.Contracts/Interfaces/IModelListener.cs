using System;
using HQ.Model;

namespace HQ {
    public interface IModelListener<TModelType> : IDispatchListener where TModelType : HQModel {

        void OnModelUpdated(TModelType model);

    }
}