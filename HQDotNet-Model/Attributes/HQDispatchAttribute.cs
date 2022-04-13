using System;
using HQDotNet.Model;

namespace HQDotNet {

    [AttributeUsage(AttributeTargets.Class)]
    public class HQDispatch : Attribute {

        public HQDispatch(Type dispatchListenerType) {

        }
    }
}