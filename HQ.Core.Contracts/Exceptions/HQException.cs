using System;;

namespace HQ.Contracts {
    public class HQException : Exception {
        public HQException() {

        }

        public HQException(string message) : base(message) {
        }
    }
}
