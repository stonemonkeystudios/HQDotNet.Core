using System;
using HQDotNet.Model;

namespace HQDotNet {

    /// <summary>
    /// HQModelDispatch is used to monitor for updates to a model.
    /// If an update is triggered, or the model is marked dirty, automatically dispatch an update to the given listener type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HQModelDispatch : Attribute {

        public HQModelDispatch(Type dispatchListenerType) {

        }
    }
}