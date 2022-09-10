# HQDotNet
HQDotNet is a loose MVCS architectural framework. It's primary functionalities are Dependency Injection and Dispatch management.

## Architecture
### HQSession
HQSession is the primary entry point for interaction with the HQ ecosystem.

Any project utilizing HQ must create at least one HQSession to manage the state of the application.

When used with Unity, typically a session will be managed in a MonoBehaviour and will utilize the built in methods such as Start and Update to regulate the Session's HQPhase

The most basic HQSession implementation within Unity would look something like this.

```
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
First is the HQPhase. All behaviors maintain a phase, which is managed by HQSession.

The phases are:
1. **Initialized**: This is the default state of all behaviors

2. **Started**: When a behavior is registered with HQSession, the session will first execute the Startup method, and it will also execute Startup at the beginning of each Update. Any registered behaviors that are in the *initialized* state will be moved to *Started*

3. **Shutdown**: When the Shutdown method of a given behavior is called, it moves into the *Shutdown* state.

### 
