using Prism.Commands;
using Stateless;
using System;
using System.Windows.Input;

namespace Ui
{
    public static class StateMachineCommandEx
    {

        /// <summary>
        /// Creates a DelegateCommand using a trigger and a state machine.
        /// The command can be executed if the trigger can be executed on the current state machine status and the specified &quot;CanExecute&quot; function is null or returns true.
        /// When the command is executed the specified action is executed and then the trigger is fired
        /// </summary>
        /// <typeparam name=&quot;TState&quot;>State machine status type.</typeparam>
        /// <typeparam name=&quot;TTrigger&quot;>State machine status trigger.</typeparam>
        /// <typeparam name=&quot;TCommandParam&quot;>Command parameter type</typeparam>
        /// <param name=&quot;stateMachine&quot;>A state machine instance</param>
        /// <param name=&quot;trigger&quot;>A trigger.</param>
        /// <param name=&quot;execute&quot;>Action to execute when the command is executed.</param>
        /// <param name=&quot;canExecute&quot;>The command can be executed only if this function is null or return true and the current status of the machine supports the trigger.</param>
        public static ICommand CreateCommand<ViewState, ViewTrigger>(this StateMachine<ViewState, ViewTrigger> stateMachine, ViewTrigger trigger)
        {

            return new DelegateCommand(
                     () => stateMachine.Fire(trigger),
                     () => stateMachine.CanFire(trigger)
                     );
        }
        }
    }
