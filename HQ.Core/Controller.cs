using System;
using HQ;
using HQ.Contracts;

/*
 * TODO: Set up callback system similar to the one in Data Sources.
 * TODO: Move ControllerDescriptor to its own file.
 * TODO: Should this be abstract? might currently be used by some tests, but it doesn't really do anything
 */

namespace HQ.Controllers {

    /// <summary>
    /// Well, a controller controls something. Data processing and such.
    /// All Singleton controllers should be registered by HQ
    /// </summary>
    public class Controller<TControllerState> : HQStateBehavior<ControllerState> where TControllerState : ControllerState{

        public Controller() : base() { }
    }
}

