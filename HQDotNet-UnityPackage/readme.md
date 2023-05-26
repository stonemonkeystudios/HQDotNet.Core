# Unity Pong Game with HQDotNet

This repository demonstrates the use of the HQDotNet framework in Unity to create a simple Pong-style game. This README provides an overview of the key scripts and their relationships, focusing on illustrating how the HQDotNet framework works effectively within a Unity project.

## The HQDotNet Framework

HQDotNet is a lightweight framework that simplifies communication between game objects and components. It offers easy-to-use dependency injection, event dispatching, listeners, and controllers that help maintain a clean architecture in any Unity project.

### Registering Controllers

In `PongSession.cs`, various controllers are registered using the `_session.RegisterController<T>` method:

```csharp
_session.RegisterController<PongBallController>();
_session.RegisterController<PongStateController>();
//... other controllers
```

By registering controllers, we're able to manage and organize game functionalities efficiently.

### Model Listeners and Dispatching

HQDotNet uses listeners to observe changes in data models. You can implement the `IModelListener<T>` interface for any class that needs to respond to model updates:

```csharp
// Example from PongBallController.cs
public class PongBallController : HQController, IModelListener<PongSettings> {

    void IModelListener<PongSettings>.OnModelUpdated(PongSettings model) {
        //Handle updates to the PongSettings model
    }

}
```

To dispatch a change, you can use the `Session.Dispatcher.Dispatch<T>` method:

```csharp
// Example from PongSession.cs
_session.Dispatcher.Dispatch<IModelListener<PongSettings>>(listener => listener.OnModelUpdated(pongSettings));
```

### Dependency Injection

The framework provides a clean way to inject dependencies using the `[HQInject]` attribute and the `HQController` class:

```csharp
// Example from PongStateController.cs
[HQInject]
private PongBallController _ballController;
```

By using `[HQInject]`, we can easily access other controllers or services in our scripts without the need for messy references.

### Overview of Classes

- `PongSession`: Sets up the game environment, registers controllers & services and initializes `pongSettings`.
- `PongBallController`: Manages the ball's behavior and listens to various game state changes and button clicks.
- `PongStateController`: Manages the state of the game and coordinates transitions between different game states.
- `PongScoreController`: Manages players' scores, updates scores when a player scores a point, and checks if the game has reached its end.
- More classes I can't be bothered to add right now, including views and interfaces. Sorry. I made ChatGPT do this, so I'd have to give it more data or do it on my own. Meh

### IDispatchListener Interface

Listeners in the HQDotNet framework need to derive from the `IDispatchListener` interface. This ensures proper callback handling and communication between listeners and dispatchers. In the following example, the `IPlayerScoredController` interface derives from `IDispatchListener`:

```csharp
namespace HQDotNet.Unity.Pong {
    public interface IPlayerScoredController : IDispatchListener {
        void PlayerScored(int playerIndex, int currentScore);
    }
}
```

### HQMonoView

The `HQMonoView` class is a custom `MonoBehaviour` provided by the HQDotNet framework. It helps manage views by registering them to a session for updates and automatic dispatch registration. Here's an example of how `PongGameUIView` derives from `HQMonoView` and implements various listener interfaces:

```csharp
namespace HQDotNet.Unity.Pong {
    public class PongGameUIView : HQMonoView, IPongStateChangedListener, IPongCountdownUpdatedListener, IPlayerScoredController {
        //... other code
    }
}
```

In `HQMonoView`, the `SetSession` method registers a view for dispatches from the HQ session:

```csharp
public virtual void SetSession(HQSession session) {
    _session = session;
    _session.RegisterObjectOnlyForDispatch(this);
}
```

`OnDestroy` ensures the view is unregistered from the session when it's destroyed:

```csharp
private void OnDestroy() {
    if (_session != null) {
        _session.UnregisterNonHQBehaviorDispatch(this);
    }
}
```

### MainThreadSyncer

Some methods may be called from background threads, so we need to ensure synchronization with the main thread when updating Unity-specific objects. The `MainThreadSyncer` accomplishes this by allowing us to schedule actions to be executed on the main thread. Here's how the `PongGameUIView` makes use of `MainThreadSyncer`:

```csharp
void IPongCountdownUpdatedListener.OnCountdownUpdated(int secondsRemaining) {
    MainThreadSyncer.Instance.ExecuteOnMainThread(() => {
        countdown.text = secondsRemaining.ToString();
    });
}

void IPongStateChangedListener.StateChanged(PongStateController.PongState oldState, PongStateController.PongState newState) {
    MainThreadSyncer.Instance.ExecuteOnMainThread(() => {
        countdown.gameObject.SetActive(newState == PongStateController.PongState.WaitingToStart);
    });
}
```

By using `MainThreadSyncer.Instance.ExecuteOnMainThread()`, we can ensure that potentially thread-sensitive code is safely executed on the main thread, preventing any issues related to concurrency.