using HQDotNet.Model;

namespace HQDotNet {

    /// <summary>
    /// HQ is a State-Driven environment structured according to a version of MVCS architecture
    /// HQSession acts as the primary entry point of this stateful environment.
    /// It regulates the Phases of all registered instances of <see cref="HQCoreBehavior"/> and subclasses (<see cref="HQView"/>, <see cref="HQController"/>, <see cref="HQService"/>
    /// The three primary classes that are regulated here are <see cref="HQInjector"/>, <see cref="HQRegistry"/>, and <see cref="HQDispatcher"/>
    /// </summary>
    /// 
    public class HQSession : HQCoreBehavior{
        /// <summary>
        /// Dispatcher is responsible for observing registered classes' interface implementation and delivering messages accordingly
        /// </summary>
        private readonly HQDispatcher _dispatcher;

        /// <summary>
        /// Injector is responsible for analysing a registered behavior's properties and injecting any registered behaviors as appropriate
        /// </summary>
        private readonly HQInjector _injector;

        /// <summary>
        /// Registry is where information about the registered behaviors and their dispatch listeners are stored
        /// </summary>
        private readonly HQRegistry _registry;

        //To be in model
        private readonly System.DateTime _startDate;

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

        /// <summary>
        /// When was the given instance started?
        /// </summary>
        public System.DateTime StartDate {
            get { return _startDate; }
        }

        /// <summary>
        /// How much real time has elapsed since this session was created?
        /// </summary>
        public System.TimeSpan TimeSinceStarted {
            get { return System.DateTime.Now - _startDate; }
        }

        /// <summary>
        /// The current session instance's Dispatcher. Used for circulating messages throughout the app
        /// </summary>
        public HQDispatcher Dispatcher { get { return _dispatcher; } }

        /// <summary>
        /// Create, inject, and register a singleton behavior for a type of Controller 
        /// </summary>
        /// <param name="TBehavior">The type of Controller to register.</param>
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
        /// Create, inject, and register a singleton behavior for a type of Service 
        /// </summary>
        /// <param name="TBehavior">The type of Service to register.</param>
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

        /// <summary>
        /// Create, inject, and register a singleton behavior for a type of View 
        /// </summary>
        /// <param name="TBehavior">The type of View to register.</param>
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

        /// <summary>
        /// Registers an arbitrary object with the Dispatcher and adds it to any relevant dispatch listener lists.
        /// </summary>
        /// <param name="obj"></param>
        public void RegisterObjectOnlyForDispatch(object obj) {
            _dispatcher.RegisterDispatchListenersForObject(obj);
        }

        /// <summary>
        /// Unregister an arbitrary object from the dispatch system. 
        /// Meant to be utilized in conjunction with <see cref="RegisterObjectOnlyForDispatch(object)"/>
        /// </summary>
        /// <param name="obj"></param>
        public void UnregisterNonHQBehaviorDispatch(object obj) {
            _dispatcher.UnregisterDispatchListenersForObject(obj);
        }

        /// <summary>
        /// Unregister an HQBehavior from the HQSession environment
        /// This incldes dispatch registration and uninjection of the behavior from the whole ecosystem
        /// </summary>
        /// <param name="behavior"></param>
        public void Unregister(HQCoreBehavior behavior) {
            _injector.UninjectBehavior(behavior);
            _dispatcher.UnregisterDispatchListenersForObject(behavior);
            _registry.Unregister(behavior);
        }

        #region HQBehavior Overrides

        /// <summary>
        /// Runs the startup procedures for all of the given HQSession.
        /// This incldes internal classes such as <see cref="HQRegistry"/>, <see cref="HQInjector"/>, and <see cref="HQDispatcher"/>
        /// Then goes on to call startup on any previously registered behaviors. 
        /// It is recommended to call startup after any core behaviors have been registered
        /// </summary>
        /// <returns></returns>
        public override bool Startup() {

            //Start up our core functionality only if it has not been started previously
            bool allStarted =   (_registry.Phase != HQPhase.Initialized || _registry.Startup()) &&
                                (_injector.Phase != HQPhase.Initialized || _injector.Startup()) &&
                                (_dispatcher.Phase != HQPhase.Initialized || _dispatcher.Startup());

            //Startup all previously registered Services
            //Only if they have not already been initialized
            foreach (var service in _registry.Services.Values) {
                if(service.Phase == HQPhase.Initialized) {
                    allStarted &= service.Startup();
                }
            }

            //Startup all previously registered Controllers
            //Only if they have not already been initialized
            foreach (var controller in _registry.Controllers.Values) {
                if(controller.Phase == HQPhase.Initialized) {
                    allStarted &= controller.Startup();
                }
            }

            //Startup all previously registered Views
            //Only if they have not already been initialized
            foreach (var viewList in _registry.Views.Values) {
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

        /// <summary>
        /// Dispatch a notification that this session's HQPhase has been updated.
        /// </summary>
        private void DispatchPhaseUpdated() {
            _dispatcher.Dispatch<ISessionListener>((listener) => listener.PhaseUpdated(Phase));
        }

        /// <summary>
        /// Call the update method on all internal classes as well as all registered behaviors
        /// </summary>
        public override void Update() {
            //Run startup to start any new instances that have not been started previously
            Startup();

            //Update internal instances
            _registry.Update();
            _injector.Update();
            _dispatcher.Update();

            //For all registered behaviors, if they have already been started and not shut down, call the update method
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
        /// Secondary update call that takes place after the initial Update call.
        /// </summary>
        public override void LateUpdate() {

            //Update internal instances
            _registry.LateUpdate();
            _injector.LateUpdate();
            _dispatcher.LateUpdate();

            //Update all registered behaviors
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

        /// <summary>
        /// Shuts down all internal and registered behaviors, including the session itself.
        /// </summary>
        /// <returns></returns>
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

