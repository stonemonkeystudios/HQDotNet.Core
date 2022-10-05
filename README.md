
# HQDotNet
HQDotNet is a loose MVCS architectural framework. It's primary functionalities are Dependency Injection and Dispatch management.

## Architecture
### HQSession
HQSession is the primary entry point for interaction with the HQ ecosystem.

Any project utilizing HQ must create at least one HQSession to manage the state of the application.

When used with Unity, typically a session will be managed in a MonoBehaviour and will utilize the built in methods such as Start and Update to regulate the Session's HQPhase

The most basic HQSession implementation within Unity would look something like this.

```csharp
using UnityEngine;
using HQDotNet;

public class DemoClass : MonoBehaviour{
    private HQSession _session;

    void Awake(){
        _session = new HQSession();

        //Register behaviors here
    }

    void Start(){
        _session.Startup();
    }

    void Update(){
        _session.Update();
    }

    void LateUpdate(){
        _session.LateUpdate();
    }

    void OnDestroy(){
        _session.Shutdown();
    }
}
```

### HQBehavior
HQBehavior is the base class for all fully integrated HQ components.

Behaviors are rarely used directly as there are no public Session methods that allow for direct registration of Behaviors. Typically, one will utilize an HQController, HQService, or an HQView.

All HQBehaviors are state behaviors that implement a number of features to interact with the state.

### HQPhase
All behaviors maintain a phase, which is managed by HQSession. This determines the state they are in and how functionality applies to them in said state.

The phases are:
1. **Initialized**: This is the default state of all behaviors

2. **Started**:When a registered behavior is in the *initialized* state, calling startup will cause the behavior to be moved to *Started* state.

3. **Shutdown**: When the Shutdown method of a given behavior is called, it moves into the *Shutdown* state.

### HQBehavior Methods
The base HQBehavior class implements a number of virtual methods that may be utilized depending on the given state of the behavior. Any setup for a behavior should be done in a constructor. Keep in mind, dependencies will not be injected yet, so setup only.
The base methods are:
1. **Startup()** - This is called only once per behavior. Dependencies may or may not be injected at this point, that is largely dictated by the order you register your behaviors. I would recommend to not rely on dependencies here unless you are careful with registration order.
2. **Update()** - After a registered behavior has moved into the *Started* state, it will begin to receive update messages any time HQSession.Update() is called. Typically in a Unity project, a session will be updated in a MonoBehaviour Update method.
3. **LateUpdate()** - This is the same as Update, but will be called after all behaviors have been Updated for a frame. So all behaviors receive Update, and then all behaviors receive LateUpdate
4. **Shutdown()** - This method allows you to execute any cleanup code before the behavior moves into a Shutdown state, where it no longer receives messages. This will be automatically called if the behavior type is unregistered from the session, or the session shuts down.

## Registering an HQBehavior

All behaviors you wish to incorporate with the HQ Environment must be registered with an HQSession. Within that HQSession, all registered behaviors will have one or both of Dispatcher Registration and Dependency Injection (Property-Based).

A simple example of registering a number of components with an HQSession looks something like this. We'll take a look at the specific components involved later.

```csharp
using UnityEngine;
using HQDotNet;

public class DemoClass : MonoBehaviour{
    //public MathMonoView mathMonoView;
    public MathSettings _mathSettings;

    private HQSession _session;

    void Awake(){
        _session = new HQSession();

        //Register behaviors here
        _session.RegisterController<MathController>();
        _session.RegisterService<MathService>();
        _session.RegisterView<MathView>();

        //For a monobehavior view, we might use something like this.
        //_session.RegisterObjectForDispatchOnly(mathMonoView);
        
        //Best practice for a Monobehaviour-based view is to inherit from HQDotNet.Unity.HQMonoView
        //An HQMonoView will register itself automatically on awake with the HQViewMediator (which is created by the most recently created session. At the moment, for this reason, multiple sessions may not behave correctly.
        
        DispatchModelUpdates();
    }

    //Here we can dispatch initial model files to the HQ ecosystem. Anyone listening for this specific model to update will receive a dispatch indicating that new data has been updated.
    //Frequently used for initial settings such as setting host and port for a web service.
    void DispatchModelUpdates(){
        _session.Dispatcher.Dispatch<IModelListener<MathSettings>>((listener)=>{
            listener.OnModelUpdated(_mathSettings);
        });
    }

    void Start()
    ...
}
```


## HQ Behavior Types

### HQController

### HQService

### HQView

## TODO
There area number of things to cover here yet, and a number of things to cover in the codebase as well.
**Current Priorities**
1. Update Unity Package with refactored HQMonoView and other Unity utilities that have developed elsewhere
2. Update Unity Package with a number of demo scenes to demonstrate usage
3. Run code coverage analysis and continue iterating on unit testing
4. Review of the codebase to clean up
5. Once demo scripts have been written, add more detailed examples to the readme
