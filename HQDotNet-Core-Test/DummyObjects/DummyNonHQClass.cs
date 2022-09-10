using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQDotNet.Test {
    public class DummyNonHQClass : IDummyListener {
        public string textToUpdate = "";
        void IDummyListener.UpdateText(string text) {
            textToUpdate = text;
        }
    }
}
