using HQDotNet.Model;

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
    public class HQSession : HQCoreBehavior{

        private static HQSession _current;
        private readonly HQDispatcher _dispatcher;
        private readonly HQInjector _injector;
        private readonly HQRegistry _registry;

        //To be in model
        private readonly System.DateTime _startDate;

        public static HQSession Current {
            get {
                return _current;
            }
        }

        public HQSession() {

            //Set up our core functionality
            _registry = new HQRegistry();
            _injector = new HQInjector();
            _dispatcher = new HQDispatcher();

            //Use a method rather than injection for these
            //Since only this class should be mediating
            _injector.SetRegistry(_registry);
            _dispatcher.SetRegistry(_registry);
            _startDate = System.DateTime.Now;
        }

        public System.DateTime StartDate {
            get { return _startDate; }
        }

        public System.TimeSpan TimeSinceStarted {
            get { return System.DateTime.Now - _startDate; }
        }

        public HQDispatcher Dispatcher { get { return _dispatcher; } }

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
        public TBehavior RegisterController<TBehavior>()
            where TBehavior : HQController, new(){
            
            TBehavior controller = new TBehavior();
            if (!_registry.RegisterController(controller)){
                return null;
            }

            _dispatcher.RegisterDispatchListenersForObject(controller);
            _injector.Inject( controller);
            controller.SetSession(this);
            return controller;
        }

        /// <summary>
        /// Register a singleton behavior for a type of service 
        /// </summary>
        /// <param name="controller"></param>
        public TBehavior RegisterService<TBehavior>()
            where TBehavior : HQService, new(){

            TBehavior service = new TBehavior();
            if (!_registry.RegisterService(service))
                return null;

            _dispatcher.RegisterDispatchListenersForObject(service);
            _injector.Inject(service);
            service.SetSession(this);
            return service;
        }

        public TBehavior RegisterView<TBehavior>()
            where TBehavior : HQView, new(){

            TBehavior view = new TBehavior();
            if (!_registry.RegisterView(view))
                return null;

            _dispatcher.RegisterDispatchListenersForObject(view);
            _injector.Inject(view);
            view.SetSession(this);
            return view;
        }

        public void RegisterObjectOnlyForDispatch(object obj) {
            _dispatcher.RegisterDispatchListenersForObject(obj);
        }

        //TODO: Add an option to register or unregister only one type of interface. maybe in dispatcher itself

        public void UnregisterNonHQBehaviorDispatch(object obj) {
            _dispatcher.UnregisterDispatchListenersForObject(obj);
        }

        public void Unregister(HQCoreBehavior behavior) {
            _injector.UninjectBehavior(behavior);
            _dispatcher.UnregisterDispatchListenersForObject(behavior);
            _registry.Unregister(behavior);
        }

        #region HQBehavior Overrides

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override bool Startup() {
            bool allStarted =   _registry.Startup() &&
                                _injector.Startup() &&
                                _dispatcher.Startup();

            //Servies
            foreach (var service in _registry.Services.Values) {
                if(service.Phase == HQPhase.Initialized) {
                    allStarted &= service.Startup();
                }
            }

            //Controllers
            foreach(var controller in _registry.Controllers.Values) {
                if(controller.Phase == HQPhase.Initialized) {
                    allStarted &= controller.Startup();
                }
            }

            //Views
            foreach(var viewList in _registry.Views.Values) {
                foreach(var view in viewList) {
                    if(view.Phase == HQPhase.Initialized) {
                        allStarted &= view.Startup();
                    }
                }
            }

            allStarted &= base.Startup();

            DispatchPhaseUpdated();

            return allStarted;
        }

        private void DispatchPhaseUpdated() {
            _dispatcher.Dispatch<ISessionListener>((listener) => listener.PhaseUpdated(Phase));
        }

        /// <summary>
        /// Internally updates our Master Controllers
        /// </summary>
        public override void Update() {
            //Run startup
            Startup();

            _registry.Update();
            _injector.Update();
            _dispatcher.Update();

            if(Phase == HQPhase.Started) {
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

        /// <summary>
        /// Internally updates our Master Controllers
        /// </summary>
        public override void LateUpdate() {
            _registry.LateUpdate();
            _injector.LateUpdate();
            _dispatcher.LateUpdate();

            if (Phase == HQPhase.Started) {
                //Servies
                foreach (var service in _registry.Services.Values) {
                    service.LateUpdate();
                }

                //Controllers
                foreach (var controller in _registry.Controllers.Values) {
                    controller.LateUpdate();
                }

                //Views
                foreach (var viewList in _registry.Views.Values) {
                    foreach (var view in viewList) {
                        view.LateUpdate();
                    }
                }
            }

            base.LateUpdate();
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

            allShutDown &= _registry.Shutdown() &&
                           _injector.Shutdown() &&
                           _dispatcher.Shutdown();

            bool success = allShutDown && base.Shutdown();
            return success;
        }

        #endregion

    }
}

