using System;
using System.Collections.Generic;
using System.Text;

namespace HQDotNet.Test {
    internal interface IDummyListener : IDispatchListener {
        void UpdateText(string text);
    }
}
