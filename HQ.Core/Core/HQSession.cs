using System;
using System.Collections.Generic;
using HQDotNet.Model;
using HQDotNet.View;
using HQDotNet.Controller;
using HQDotNet.Service;

/* Master TODO List
 * 
 * Tasks:
 * TODO: Create View and HQViews - Classes that manage rendering data
 * 
 * Controllers and HQ should have callbacks for the various phases
 *  Things like OnRegister, OnStartup, OnShutdown,
 * 
 * Controller should be desroyed in shutdown phase, including Generic, so other references to it are also destroyed
 * 
 */


//TODO: Threading
// -All HQBehaviors should run on their own thread.
// -Dispatcher should dispatch to the context of the behavior
// -Ideally services would be entirely based on ThreadPoolWorkers
// -This means that every api needs to be its own IThreadPoolWorkerClass
// 


namespace HQDotNet {

    /// <summary>
    /// HQ is a State-Driven environment structured according to a version of MVCS architecture
    /// All serializable state information is housed within the State property of <see cref="HQStateBehavior"/>
    /// All behaviors inheriting from <see cref="HQStateBehavior"/> are housed in <see cref="HQSessionModel.Behaviors"/>
    /// </summary>
    /// 
    public sealed class HQSession : HQCoreBehavior<HQSessionModel>{

        private static HQSession _current;
        private HQDispatcher _dispatcher;
        private HQInjector _injector;
        private HQRegistry _registry;


        public static HQSession Current {
            get {
                return _current;
            }
        }

        public HQSession() {
            Model = new HQSessionModel();
        }

        public HQSession(HQSessionModel session) {
            Model = session;
        }

        public HQDispatcher Dispatcher {
            get {
                return _dispatcher;
            }
        }

        public HQRegistry Bindings {
            get {
                return _registry;
            }
        }

        /// <summary>
        /// Initialize HQ and prepare for battle
        /// </summary>
        /// <returns></returns>
        public static void Init() {
            if(_current != null) {
                throw new HQException("Current session is already running.");
            }

            _current = new HQSession();
        }

        public static bool HasSession {
            get { return _current != null; }
        }

        public TListener Dispatch<TListener>() where TListener : IDispatchListener {
            return Dispatcher.Dispatch<TListener>();
        }

        public TBehavior Get<TBehavior>() where TBehavior : HQController<HQBehaviorModel>, new() {
            Type typeT = typeof(TBehavior);
            return Model.Get<TBehavior>();
        }

        public TBehavior RegisterBehavior<TBehavior, TModel>() 
            where TBehavior : HQController<TModel>, new() 
            where TModel : HQBehaviorModel, new(){

            Type modelType = typeof(TModel);

/*            bool controllerRegistered = Model.ControllerModels.Exists((model) => { return model.GetType() == modelType; });
            if (controllerRegistered) {
                return Model.ControllerModels.Find((model) => { return model.GetType() == modelType; }) as TController;
            }*/

            TBehavior newBehavior = new TBehavior();
            //Model.

            //Model.ControllerModels.Add(newBehavior.Model);
            _registry.BindBehavior(newBehavior as HQController<HQBehaviorModel);
            _dispatcher.RegisterListeners(newBehavior);
            _injector.Inject(newBehavior);

            return newBehavior;
        }

        public void Unregister<TBehavior>() where TBehavior : HQController {
            if (Model.Contains<TBehavior>()) {
                var behavior = Model.Get<TBehavior>();

                if (behavior != null)
                    behavior.Shutdown();

                _dispatcher.UnregisterBehavior(behavior);
                _injector.UninjectBehavior(behavior);
                Model.Remove<TBehavior>();
            }
        }

        #region HQBehavior Overrides

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override bool Startup() {

            //Set up our core functionality
            _registry = new HQRegistry();
            _injector = new HQInjector();
            _dispatcher = RegisterBehavior<HQDispatcher, HQControllerModel>();

            bool allStarted = _dispatcher.Startup();

            //Use a method rather than injection for these
            //Since only this class should be mediating
            _injector.SetRegistry(_registry);
            _dispatcher.SetRegistry(_registry);

            var services = Model.

            foreach (var service in Model.Services.Values) {
                if (service.State.Phase == HQPhase.Initialized)
                    allStarted &= service.Startup();
            }

            foreach (var controller in Model.Controllers.Values) {
                if (controller.State.Phase == HQPhase.Initialized)
                    allStarted &= controller.Startup();
            }

            allStarted &= base.Startup();

            Dispatcher.Dispatch<ISessionListener>().PhaseUpdated(Model.Phase);

            return allStarted;
        }

        /// <summary>
        /// Internally updates our Master Controllers
        /// </summary>
        public override void Update() {
            //Run startup
            Startup();

            switch (Model.Phase) {
                case HQPhase.Started:





                    foreach (var service in Model.ServiceModels) {
                        _registry.
                        service.Update();
                    }

                    foreach (var controller in Model.ControllerModels) {
                        controller.Update();
                    }

                    break;
            }
            base.Update();
        }

        public override bool Shutdown() {
            bool allShutDown = true;


            foreach(var controller in Model.Controllers.Values){
                allShutDown &= controller.Shutdown();
            }

            foreach (var service in Model.Services.Values) {
                allShutDown &= service.Shutdown();
            }

            /*foreach(var dataSource in State.DataSources.Values) {
                allShutDown &= dataSource.Shutdown();
            }*/

            bool success = allShutDown && base.Shutdown();
            if(this == _current) {
                _current = null;
            }
            return success;
        }

        #endregion

    }
}

