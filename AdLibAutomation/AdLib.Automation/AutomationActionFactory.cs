// AutomationActionFactory.cs
using System;
using System.Collections.Generic;
using System.Linq;
//using AdLib.Automation.Actions;
using AdLib.Automation.Interfaces;

namespace AdLib.Automation
{
    public static class AutomationActionFactory
    {
        private static readonly Dictionary<string, Type> ActionTypes = new Dictionary<string, Type>
        {
            //{ "ClickButtonAction", typeof(ClickButtonAction) },
            //{ "CloseApplicationAction", typeof(CloseApplicationAction) },
            //{ "FocusApplicationAction", typeof(FocusApplicationAction) },
            //{ "InputTextAction", typeof(InputTextAction) },
            //{ "OpenApplicationAction", typeof(OpenApplicationAction) },
            //{ "SubmitAction", typeof(SubmitAction) }
        };

        public static List<Type> GetAvailableActions()
        {
            return ActionTypes.Values.ToList();
        }

        public static IAutomationAction CreateAction(Type actionType, IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService(actionType) as IAutomationAction;
        }

        // Optional: To allow dynamic registration of new actions, e.g., from plugins
        public static void RegisterAction(string actionName, Type actionType)
        {
            if (!ActionTypes.ContainsKey(actionName))
            {
                ActionTypes[actionName] = actionType;
            }
        }
    }
}
