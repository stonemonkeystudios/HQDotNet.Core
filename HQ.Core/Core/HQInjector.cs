using System;
using System.Reflection;
using HQ.Model;

namespace HQ
{
    public class HQInjector {

        const BindingFlags INJECT_BINDING_FLAGS = BindingFlags.NonPublic | BindingFlags.Instance;

        HQSession _hq;

        public HQInjector(HQSession hq) {
            _hq = hq;
        }

        public bool InjectAll() {
            if(_hq.Model == null) {
                throw new NullReferenceException("Missing State Model, has HQ been shut down somehow?");
            }

            return false;
        }

        public bool InjectBehavior(HQBehavior<HQBehaviorModel> newBehavior) {
            if (_hq.Model == null) {
                throw new NullReferenceException("Missing State Model, has HQ been shut down somehow?");
            }

            var category = HQBehaviorBindings.GetBehaviorCategory(newBehavior.GetType());
            foreach(HQControllerModel controllerModel in _hq.Model.ControllerModels) {

            }

            var behaviorList = _hq.Model.GetAllBehaviors();
            foreach(var dict in behaviorList) {
                foreach (var existingBehavior in _hq.Bindings) {
                    //Try injecting the new into the existing
                    Inject(existingBehavior, newBehavior);

                    //Try to inject the existing into the new
                    Inject(newBehavior, existingBehavior);
                }
            }

            return true;
        }

        public void UninjectBehavior(HQBehavior<HQBehaviorModel> behavior) {
            if (_hq.Model == null) {
                throw new NullReferenceException("Missing State Model, has HQ been shut down somehow?");
            }
        }



        public bool ValidateInjectionRules(Type typeInjectee, Type typeInjector) {
            //TODO:
            // This is to validate separation of function
            // Here we can establish framework rules about how different services may interact
            /*
             * Injectees
             *      -Views
             *          Controllers:Y, Models:? (are models ever actually injected?
             *      -Controllers
             *          DataSources: No, Views: No?, Controllers: Yes
             *          
             *  Report an error to the system, something is set up wrong if the get an error
             *  Something like ValidationError or ConformanceError
             *          
             */

            return true;
        }

        private void Inject(HQBehavior<HQBehaviorModel> injectee, HQBehavior<HQBehaviorModel> injector) {

            //Stop injecting yourself. Stop injecting yourself.
            if (injectee == injector)
                return;

            Type injecteeType = injectee.GetType();
            Type injectorType = injector.GetType();

            //TODO: Make sure there are no circular dependencies?
            MemberFilter filter = new MemberFilter(InjectionMemberFilter);

            //Find all fields of the given type on injectee
            //If we ever decide to support other MemberTypes here, the logic will need to account for this below.
            MemberInfo[] injectableMembers = injecteeType.FindMembers(MemberTypes.Field, INJECT_BINDING_FLAGS, filter, injectorType);
            foreach(MemberInfo member in injectableMembers) {


                //Enforce our separation-of-function rules
                //If injectee has declared an [HQInject] method that does conform with the ruleset, it needs a refactor.
                if (!ValidateInjectionRules(injecteeType, injectorType))
                    continue;

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
            return memberInfo.ReflectedType.IsAssignableFrom(injectorType);
        }
    }
}
