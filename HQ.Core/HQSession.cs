using System;
using System.Collections.Generic;
using HQ.Controllers;
using HQ.Contracts;

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

//TODO: HQ Needs to rely exclusively on states


namespace HQ {

    /// <summary>
    /// HQ is a State-Driven environment structured according to a version of MVCS architecture
    /// All serializable state information is housed within the State property of <see cref="HQStateBehavior"/>
    /// All behaviors inheriting from <see cref="HQStateBehavior"/> are housed in <see cref="SessionState.Behaviors"/>
    /// </summary>
    /// 
    public class HQSession : Controller<SessionState>{

        private static HQSession _current;
        private HQDispatcher _dispatcher;
        private HQInjector _injector;
        private HQBehaviorMap _behaviorMap;

        public static HQSession Current {
            get {
                return _current;
            }
        }

        public HQSession() {
            State = new SessionState();
            _dispatcher = new HQDispatcher();
            _injector = new HQInjector(this);
            _behaviorMap = new HQBehaviorMap();
        }

        public HQSession(SessionState session) {
            State = session;
        }

        public HQDispatcher Dispatcher {
            get {
                return _current._dispatcher;
            }
        }

        /// <summary>
        /// Initialize HQ and prepare for battle
        /// </summary>
        /// <returns></returns>
        public static void Init() {
            if(_current != null) {
                Current.Shutdown();
            }
            HQStateMachine.BuildKnownTypes();
            _current = new HQSession();
        }

        public static bool HasSession {
            get { return _current != null; }
        }

        public TListener Dispatch<TListener>() where TListener : IDispatchListener {
            return Dispatcher.Dispatch<TListener>();
        }

        public TBehavior Get<TBehavior>() where TBehavior : HQStateBehavior, new() {
            Type typeT = typeof(TBehavior);
            return State.Get<TBehavior>();
        }

        public TBehavior Register<TBehavior>() where TBehavior : HQStateBehavior, new(){

            if (State.Contains<TBehavior>())
                throw new ArgumentException("Behavior already registered.");

            TBehavior newBehavior = new TBehavior();

            State.Add<TBehavior>(newBehavior);
            _dispatcher.RegisterBehavior(newBehavior);
            _injector.InjectBehavior(newBehavior);

            return newBehavior;
        }

        public void Unregister<TBehavior>() where TBehavior : HQStateBehavior {
            if (State.Contains<TBehavior>()) {
                var behavior = State.Get<TBehavior>();

                if (behavior != null)
                    behavior.Shutdown();

                _dispatcher.UnregisterBehavior(behavior);
                _injector.UninjectBehavior(behavior);
                State.Remove<TBehavior>();
            }
        }

        #region HQBehavior Overrides

        public bool LoadSettings(HQSettings[] settings) {
            //TODO: Create an HQMasterSettings Class
            //This can contain a list of data source settings, a list of controller settings, etc
            throw new System.NotImplementedException("TODO");
        }

        /// <summary>
        /// Internally starts up all Registered DataSources, Services, and Controllers, in that order
        /// Moves phase to Started
        /// </summary>
        /// <returns></returns>
        public override bool Startup() {
            /*
             * TODO: The order in which these are started indicates some rules
             * -Data source Startup methods:
             *   -CAN rely on REGISTERED/INJECTED Controllers, Services, and Sources, even if registered this frame
             *   -CANNOT rely on Controllers, Services, or Sources being STARTED this frame
             * -Service startup methods:
             *   -CAN rely on REGISTERED/INJECTED Controllers, Services, and Sources, even if registered this frame
             *  -CAN rely on sources STARTED this frame or before
             *  -CANNOT rely on Controllers or Services being STARTED this frame
             * -Controller Startup Methods:
             *   -CAN rely on REGISTERED/INJECTED Controllers, Services, and Sources, even if registered this frame
             *   CAN rely on  Sources and Services STARTED this frame or before
             *  
             *  RULE: Services don't talk to each other (No injection of Services
             *  RULE: Services can talk to Data Sources
             *  RULE: Controllers can't talk to Controllers? (No injection of controllers)
             *  RULE: Controllers can talk to MULTIPLE Services
             *  RULE: Controllers can't talk to Data Sources (No injection of data sources)
             *  RULE: Controllers talk to Services, but NOT Data Sources
             *  RULE: Data Sources can talk to Controllers
             *  RULE: Data Sources don't talk to Services (No injection of services)
             *  
             *  TODO: Draw a diagram and visualize how it should work
             * 
             * TODO: Something to think about:
             * Should we be able to explicitly define the startup order/script execution order?
             * Like unity does?
             */

            bool allStarted = true;

            /*foreach(var dataSource in State.DataSources.Values) {
                if(dataSource.State.Phase == HQPhase.Initialized)
                    allStarted &= dataSource.Startup();
            }*/

            foreach (var service in State.Services.Values) {
                if (service.State.Phase == HQPhase.Initialized)
                    allStarted &= service.Startup();
            }

            foreach (var controller in State.Controllers.Values) {
                if (controller.State.Phase == HQPhase.Initialized)
                    allStarted &= controller.Startup();
            }

            allStarted &= base.Startup();

            Dispatcher.Dispatch<ISessionListener>().PhaseUpdated(State.Phase);

            return allStarted;
        }

        /// <summary>
        /// Internally updates our Master Controllers
        /// </summary>
        public override void Update() {
            //Run startup
            Startup();

            switch (State.Phase) {
                case HQPhase.Started:

                    //This is the main reason we have categorized, so we can set the order which they are updated
                    /*foreach(var dataSource in State.DataSources.Values) {
                        dataSource.Update();
                    }*/

                    foreach (var service in State.Services.Values) {
                        service.Update();
                    }

                    foreach (var controller in State.Controllers.Values) {
                        controller.Update();
                    }

                    break;
            }
            base.Update();
        }

        public override bool Shutdown() {
            bool allShutDown = true;


            foreach(var controller in State.Controllers.Values){
                allShutDown &= controller.Shutdown();
            }

            foreach (var service in State.Services.Values) {
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

