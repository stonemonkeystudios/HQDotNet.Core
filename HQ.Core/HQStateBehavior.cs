using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using HQ.Contracts;

// Add references to Soap and Binary formatters.
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;



namespace HQ {
    /// <summary>
    /// This is for automatic serialization of classes to subtypes.
    /// </summary>
    public abstract class HQStateBehavior<TState> where TState : BaseStateModel, new(){
        public TState State { get; protected set; }

        public HQStateBehavior() {
            State = new TState();
            State.Phase = HQPhase.Initialized;
        }

        public HQStateBehavior(TState state) {
            State = state;
        }

        public virtual bool Shutdown() {
            State.Phase = HQPhase.Shutdown;
            State = null;
            return true;
        }

        public virtual bool Startup() {
            State.Phase = HQPhase.Started;
            return true;
        }

        public virtual void Update() {
        }
    }
}