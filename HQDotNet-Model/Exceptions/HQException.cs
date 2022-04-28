using System;

namespace HQDotNet.Model {
    public class HQException : Exception {
        public HQException(string message) : base(message) { }
    }

    public class HQInjectionException : HQException {
        public HQInjectionException(Type injecteeT, Type injectorT) : base("HQInjectionException: Class " + injecteeT + " has declared a field of type " + injectorT + ". This is disallowed to encourage HQDotNet's Separation of Concerns.") {  }
        public HQInjectionException(string message) : base(message) { }
    }
}
