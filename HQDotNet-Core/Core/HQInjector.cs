using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using HQDotNet.Model;

namespace HQDotNet
{
    public sealed class HQInjector : HQCoreBehavior{

        //TODO: Injector flags in an injectormodel
        const BindingFlags INJECT_BINDING_FLAGS = BindingFlags.NonPublic | BindingFlags.Instance;

        private HQRegistry _registry;

        public void SetRegistry(HQRegistry registry) {
            _registry = registry;
        }

        public bool InjectAll() {
            return false;
        }

        public bool Inject(HQCoreBehavior behavior){
            BehaviorCategory behaviorCategory = HQRegistry.GetBehaviorCategory(behavior.GetType());

            ValidateInjectionRules(behavior);

            foreach(Type existingControllerT in _registry.Controllers.Keys) {
                if(behavior.GetType() == existingControllerT) {
                    continue;
                }
                switch (behaviorCategory) {

                    //Controller <--> Controller
                    case BehaviorCategory.Controller:
                        Inject(behavior, _registry.Controllers[existingControllerT]);
                        Inject(_registry.Controllers[existingControllerT], behavior);
                        break;

                    //Controller <-!-> View
                    case BehaviorCategory.View: break;

                    //Controller <-- Service
                    case BehaviorCategory.Service:
                        //Testing, Controllers will not inject into services.
                        //Services should do data validation and dispatching
                        //Inject(behavior, _registry.Controllers[type]);
                        Inject(_registry.Controllers[existingControllerT], behavior);
                        break;
                }
            }

            //Service -> Controller, View
            foreach(Type existingServiceT in _registry.Services.Keys) {
                if (behavior.GetType() == existingServiceT) {
                    continue;
                }
                switch (behaviorCategory) {
                    case BehaviorCategory.Controller:
                        Inject(behavior, _registry.Services[existingServiceT]);
                        break;
                    case BehaviorCategory.View:
                        Inject(behavior, _registry.Services[existingServiceT]);
                        break;
                }
            }

            //View <--> View
            foreach (Type type in _registry.Views.Keys) {
                foreach (var existingView in _registry.Views[type]) {
                    if (behavior == existingView) {
                        continue;
                    }
                    switch (behaviorCategory) {
                        case BehaviorCategory.View:
                            Inject(existingView, behavior);
                            Inject(behavior, existingView);
                            break;
                    }
                }
            }

                return true;
        }

        public void UninjectBehavior(HQCoreBehavior behavior) {
            Type injectorType = behavior.GetType();

            //Iterate every known behavior

            //If there is a field with this behavior assigned, then set that behavior variable to null
            foreach (var controllerType in _registry.Controllers.Keys) {
                var controller = _registry.Controllers[controllerType];
                MemberFilter filter = new MemberFilter(InjectionMemberFilter);

                //Find all fields of the given type on injectee
                //Remove any
                MemberInfo[] injectableMembers = controller.GetType().FindMembers(MemberTypes.Field, INJECT_BINDING_FLAGS, filter, injectorType);
                foreach (MemberInfo member in injectableMembers) {
                    if(member.GetCustomAttribute<HQInject>() == null) {
                        continue;
                    }
                    (member as FieldInfo).SetValue(controller, null);
                }
            }

            foreach (var serviceType in _registry.Services.Keys) {
                var service = _registry.Services[serviceType];
                MemberFilter filter = new MemberFilter(InjectionMemberFilter);

                //Find all fields of the given type on injectee
                //Remove any
                MemberInfo[] injectableMembers = service.GetType().FindMembers(MemberTypes.Field, INJECT_BINDING_FLAGS, filter, injectorType);
                foreach (MemberInfo member in injectableMembers) {
                    if (member.GetCustomAttribute<HQInject>() == null) {
                        continue;
                    }
                    (member as FieldInfo).SetValue(service, null);
                }
            }

            foreach (var viewType in _registry.Views.Keys) {
                foreach (var view in _registry.Views[viewType]) {
                    MemberFilter filter = new MemberFilter(InjectionMemberFilter);

                    //Find all fields of the given type on injectee
                    //Remove any
                    MemberInfo[] injectableMembers = view.GetType().FindMembers(MemberTypes.Field, INJECT_BINDING_FLAGS, filter, injectorType);
                    foreach (MemberInfo member in injectableMembers) {
                        if (member.GetCustomAttribute<HQInject>() == null) {
                            continue;
                        }
                        (member as FieldInfo).SetValue(view, null);
                    }
                }
            }
        }

        public void ValidateInjectionRules(HQCoreBehavior behaviorToInject) {
            Type behaviorToInjectT = behaviorToInject.GetType();
            var behaviorToInjectCategory = HQRegistry.GetBehaviorCategory(behaviorToInjectT);

            var fields = behaviorToInjectT.GetFields(INJECT_BINDING_FLAGS | BindingFlags.DeclaredOnly);
            foreach (var injecteeField in fields) {
                var decaredFieldT = injecteeField.FieldType;


                switch (behaviorToInjectCategory) {
                    case BehaviorCategory.Controller:
                        //1. Thou shalt not inject a Singleton Controller unto itself.
                        if (decaredFieldT == behaviorToInjectT)
                            throw new HQInjectionException("A behavior may not be injected into itself.");

                        //2. Thou shalt not inject a View unto a Controller.
                        if (typeof(HQView).IsAssignableFrom(decaredFieldT))
                            throw new HQInjectionException(behaviorToInjectT, decaredFieldT);

                        break;

                    case BehaviorCategory.Service:

                        //3. Thou shalt pretect the sanctity of a Service: no behavior shall inject it.
                        throw new HQInjectionException(behaviorToInjectT, decaredFieldT);

                    case BehaviorCategory.View:

                        //4. Thou shalt not inject a View unto a View of the same type.
                        if (decaredFieldT == behaviorToInjectT)
                            throw new HQInjectionException("A behavior may not be injected into itself.");

                        //5. Thou shalt not inject a Controller unto a View.
                        if (typeof(HQController).IsAssignableFrom(decaredFieldT))
                            throw new HQInjectionException(behaviorToInjectT, decaredFieldT);

                        break;

                    default:
                        //6. Thou shalt not enter the inner injection sanctum unless you are a Controller, Service, or View
                        throw new HQInjectionException(behaviorToInjectT, decaredFieldT);
                }
            }
        }

        private void Inject(HQObject injectee, HQObject injector) {

            //Stop injecting yourself. Stop injecting yourself.
            if (injectee == injector)
                throw new HQInjectionException("An object may not be injected into itself.");

            Type injecteeType = injectee.GetType();
            Type injectorType = injector.GetType();

            //TODO: Make sure there are no circular dependencies?
            MemberFilter filter = new MemberFilter(InjectionMemberFilter);

            //Find all fields of the given type on injectee
            //If we ever decide to support other MemberTypes here, the logic will need to account for this below.
            MemberInfo[] injectableMembers = injecteeType.FindMembers(MemberTypes.Field, INJECT_BINDING_FLAGS, filter, injectorType);
            foreach(MemberInfo member in injectableMembers) {
                //Only inject with [HQInject]
                if (member.GetCustomAttribute<HQInject>() == null)
                    continue;

                try {
                    (member as FieldInfo).SetValue(injectee, injector);
                }
                catch(Exception e) {
                    //Catching and throwing so it's more obvious to fix this when we have an error class
                    throw e;
                }
            }
        }

        private bool InjectionMemberFilter(MemberInfo memberInfo, object filterCriteria) {
            Type injectorType = (Type)filterCriteria;
            //bool success = ((FieldInfo)memberInfo).FieldType == injectorType;
            bool success = ((FieldInfo)memberInfo).FieldType.IsAssignableFrom(injectorType);
            return success;
        }
    }
}
