using System;;

namespace HQ.Model {
    public class HQException : Exception {
        public HQException() {

        }

        public HQException(string message) : base(message) {
        }
    }
}
