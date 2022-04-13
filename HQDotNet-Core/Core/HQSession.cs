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


        /// <summary>
        /// Register a singleton behavior for a type of controller 
        /// </summary>
        /// <param name="controller"></param>
        public void RegisterController<TBehavior>()
            where TBehavior : HQController<HQControllerModel>, new(){

            TBehavior controller = new TBehavior();
            _registry.RegisterController(controller);

            _dispatcher.RegisterListeners(controller);
            _injector.Inject( controller);
        }

        /// <summary>
        /// Register a singleton behavior for a type of service 
        /// </summary>
        /// <param name="controller"></param>
        public void RegisterService<TBehavior, TModel>()
            where TBehavior : HQService<HQServiceModel>, new()
            where TModel : HQServiceModel, new() {

            TBehavior service = new TBehavior();
            _registry.RegisterService<TBehavior, TModel>(service);

            _dispatcher.RegisterListeners(service);
            _injector.Inject(service);
        }

        public void RegisterView<TBehavior, TModel>()
            where TBehavior : HQView<HQDataModel, HQViewModel<HQDataModel>>, new()
            where TModel : HQViewModel<HQDataModel>, new() {

            TBehavior view = new TBehavior();
            _registry.RegisterView<TBehavior, TModel>(view);

            _dispatcher.RegisterListeners(view);
            _injector.Inject(view);
        }

        public void Unregister<TBehavior>() where TBehavior : HQCoreBehavior<HQCoreBehaviorModel>, new() {

            //_injector.UninjectBehavior()
            //_dispatcher.Unregister<>
            //_registry.UnbindBehavior();

            /*if (Model.Contains<TBehavior>()) {
                var behavior = Model.Get<TBehavior>();

                if (behavior != null)
                    behavior.Shutdown();

                _dispatcher.UnregisterBehavior(behavior);
                _injector.UninjectBehavior(behavior);
                Model.Remove<TBehavior>();
            }*/
        }

        #region HQBehavior Overrides

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override bool Startup() {

            //Set up our core functionality
            _registry = new HQRegistry();
            _injector = new HQInjector();
            _dispatcher = new HQDispatcher();

            bool allStarted = _dispatcher.Startup();

            //Use a method rather than injection for these
            //Since only this class should be mediating
            _injector.SetRegistry(_registry);
            _dispatcher.SetRegistry(_registry);

            //Servies
            foreach(var service in _registry.Services.Values) {
                if(service.Model.Phase == HQPhase.Initialized) {
                    allStarted &= service.Startup();
                }
            }

            //Controllers
            foreach(var controller in _registry.Controllers.Values) {
                if(controller.Model.Phase == HQPhase.Initialized) {
                    allStarted &= controller.Startup();
                }
            }

            //Views
            foreach(var viewList in _registry.Views.Values) {
                foreach(var view in viewList) {
                    if(view.Model.Phase == HQPhase.Initialized) {
                        allStarted &= view.Startup();
                    }
                }
            }

            allStarted &= base.Startup();

            _dispatcher.Dispatch<ISessionListener>().PhaseUpdated(Model.Phase);

            return allStarted;
        }

        /// <summary>
        /// Internally updates our Master Controllers
        /// </summary>
        public override void Update() {
            //Run startup
            Startup();

            if(Model.Phase == HQPhase.Started) {
                //Servies
                foreach (var service in _registry.Services.Values) {
                    service.Update();
                }

                //Controllers
                foreach (var controller in _registry.Controllers.Values) {
                    controller.Update();
                }

                //Views
                foreach (var viewList in _registry.Views.Values) {
                    foreach (var view in viewList) {
                        view.Update();
                    }
                }
            }

            base.Update();
        }

        public override bool Shutdown() {
            bool allShutDown = true;

            //Servies
            foreach (var service in _registry.Services.Values) {
                allShutDown &= service.Shutdown();
            }

            //Controllers
            foreach (var controller in _registry.Controllers.Values) {
                allShutDown &= controller.Shutdown();
            }

            //Views
            foreach (var viewList in _registry.Views.Values) {
                foreach (var view in viewList) {
                    allShutDown &= view.Shutdown();
                }
            }

            _injector.Shutdown();
            _injector = null;

            bool success = allShutDown && base.Shutdown();
            return success;
        }

        #endregion

    }
}

