using System;
using HQ;
using HQ.Model;

/*
 * TODO: Set up callback system similar to the one in Data Sources.
 * TODO: Move ControllerDescriptor to its own file.
 * TODO: Should this be abstract? might currently be used by some tests, but it doesn't really do anything
 */

namespace HQ.Controller {

    /// <summary>
    /// Well, a controller controls something. Data processing and such.
    /// All Singleton controllers should be registered by HQ
    /// </summary>
    public class HQController<TControllerModel> : HQBehavior<TControllerModel> where TControllerModel : HQControllerModel, new(){

        public HQController() : base() { }
    }
}

