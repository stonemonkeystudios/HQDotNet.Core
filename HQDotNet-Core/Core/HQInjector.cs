using System;
using System.Collections.Generic;
using System.Reflection;
using HQDotNet.Model;

namespace HQDotNet
{
    public sealed class HQInjector : HQCoreBehavior{

        //TODO: Injector flags in an injectormodel
        const BindingFlags INJECT_BINDING_FLAGS = BindingFlags.NonPublic | BindingFlags.Instance;

        private HQRegistry _registry;
        private Dictionary<Type, List<Type>> _ruleset;

        public void SetRegistry(HQRegistry registry) {
            _registry = registry;
        }

        public bool InjectAll() {
            return false;
        }

        public bool Inject(HQObject behavior){
            BehaviorCategory behaviorCategory = HQRegistry.GetBehaviorCategory(behavior.GetType());

            foreach(Type type in _registry.Controllers.Keys) {
                switch (behaviorCategory) {
                    //Controller <--> Controller
                    case BehaviorCategory.Controller:
                        Inject(behavior, _registry.Controllers[type]);
                        Inject(_registry.Controllers[type], behavior);
                        break;
                    //Controller --> View
                    case BehaviorCategory.View:
                        Inject(behavior, _registry.Controllers[type]);
                        break;
                    //Controller --> Service
                    case BehaviorCategory.Service:
                        Inject(_registry.Controllers[type], behavior);
                        break;
                }
            }

            //Service -> Controller
            foreach(Type type in _registry.Services.Keys) {
                switch (behaviorCategory) {
                    case BehaviorCategory.Controller:
                        Inject(behavior, _registry.Services[type]);
                        break;
                }
            }

            
            /*foreach (Type type in _registry.Views.Keys) {
                foreach (var view in _registry.Views[type]) {
                    switch (behaviorCategory) {
                        case BehaviorCategory.Controller:
                            Inject(view, behavior);
                            break;
                    }
                }
            }*/

                return true;
        }

        public void UninjectBehavior(HQCoreBehavior behavior) {
        }

        public bool ValidateInjectionRules(Type typeInjectee, Type typeInjector) {

            //switch (true) {
            //    case true when typeof(HQController<HQControllerModel>).IsAssignableFrom(typeInjector)
            //}




            /*
             * Controllers can be injected to:
             *      -Views
             *      -Controllers
             *      
             * Services can be injected into:
             *      -Controllers
             * 
             * Views can be injected into
             *      -Other Views? Views really shouoldn't be singletons anyway, so they would need to be named
             * 
             * Session:
             *      Anything
             *      
             * Dispatcher:
             *      For now, anything
             *      
             * Registry:
             *      Nothing for now, only accessed by dispatcher and injector
             *      
             * Injector:
             *      Nothing: Only a single instance in hq
             * 
             */


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
             *   InjectionPairs       
             *  Service -> Controller
             *  Service -> View
             *  Controller -> View (View can update controller)
             *  
             *     Listeners
             *  Controller: IModelCollectionListener, IModelListener
             *  View: IModelListener (Updates when model is updated)
             *  CollectionView<Tmodel>: IModelCollectionListener<TModel>, IModelListener<TModel>
             *   
             *          
             */

            return true;
        }

        private void Inject(HQObject injectee, HQObject injector) {

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
            //bool success = ((FieldInfo)memberInfo).FieldType == injectorType;
            bool success = ((FieldInfo)memberInfo).FieldType.IsAssignableFrom(injectorType);
            return success;
        }
    }
}
