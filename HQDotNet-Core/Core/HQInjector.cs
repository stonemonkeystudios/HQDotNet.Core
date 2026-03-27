using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using HQDotNet.Model;

namespace HQDotNet
{
    /// <summary>
    /// HQInjector is the primary Dependency Injection class.
    /// This class scans newly registered <see cref="HQCoreBehavior"/> instances for the [HQInject] tag, and looks for appropriate object instances that match type.
    /// </summary>
    public sealed class HQInjector : HQCoreBehavior, IHQInjector{

        //TODO: Injector flags in an injectormodel
        const BindingFlags INJECT_BINDING_FLAGS = BindingFlags.NonPublic | BindingFlags.Instance;

        private IHQRegistry _registry;

        public void SetRegistry(IHQRegistry registry) {
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

                Inject(behavior, _registry.Controllers[existingControllerT]);
                Inject(_registry.Controllers[existingControllerT], behavior);
            }

            //Service -> Controller
            foreach(Type existingServiceT in _registry.Services.Keys) {
                if (behavior.GetType() == existingServiceT) {
                    continue;
                }
                switch (behaviorCategory) {
                    case BehaviorCategory.Controller:
                    case BehaviorCategory.Service:
                        Inject(behavior, _registry.Services[existingServiceT]);
                        break;
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
            MemberFilter filter = new MemberFilter(InjectionMemberFilter);

            FieldInfo[] injectableMembers = behaviorToInject.GetType().GetFields(INJECT_BINDING_FLAGS);
            foreach (FieldInfo member in injectableMembers) {
                if (member.GetCustomAttribute<HQInject>() == null) {
                    continue;
                }

                var decaredFieldT = member.FieldType;


                switch (behaviorToInjectCategory) {
                    case BehaviorCategory.Controller:
                    case BehaviorCategory.Service:
                        //Do not inject a behavior into itself.
                        if (decaredFieldT == behaviorToInjectT)
                            throw new HQInjectionException("A behavior may not be injected into itself.");

                        //Do not directly inject a view into a controller
                        if (typeof(HQView).IsAssignableFrom(decaredFieldT))
                            throw new HQInjectionException("Views may not be injected into other classes. They should communicate with Dispatches and IDispatchListeners.");

                        break;

                    case BehaviorCategory.View:

                        //4. Thou shalt not inject a View unto a View of the same type.
                        if (decaredFieldT == behaviorToInjectT)
                            throw new HQInjectionException("A behavior may not be injected into itself.");

                        break;
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
            //bool success = injectorType.IsAssignableFrom(((FieldInfo)memberInfo).FieldType);
            bool success = ((FieldInfo)memberInfo).FieldType.IsAssignableFrom(injectorType);
            return success;
        }

    }
}
