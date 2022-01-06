using System;
using System.Threading.Tasks;
using FolderAnalyzer.Helpers;

namespace FolderAnalyzer.FSM
{
    public class FiniteStateMachine
    {
        public IState CurrentState { get; private set; }

        private FiniteStateMachine()
        {
        }

        public static async Task<FiniteStateMachine> Create(ITransition defaultTransition)
        {
            var stateMachine = new FiniteStateMachine();
            await stateMachine.SetCurrentStateFromTransition(defaultTransition).ConfigureAwait(false);
            return stateMachine;
        }

        public async Task<int> StartLoop()
        {
            int exitCode;
            
            try
            {
                while (true)
                {
                    ITransition transition = await CurrentState.Update().ConfigureAwait(false);
                    if (transition != null)
                    {
                        if (CurrentState.IsFinal)
                            throw new InvalidOperationException($"Final state '{CurrentState.GetType().Name}' tried transitioning to other state.");

                        await SetCurrentStateFromTransition(transition).ConfigureAwait(false);
                    }
                    else if (CurrentState.IsFinal)
                        break;
                }

                exitCode = Constants.ExitCodes.ERROR_SUCCESS;
            }
            catch (Exception exception)
            {
                exitCode = Constants.ExitCodes.ERROR_EXCEPTION;
                ConsoleHelper.WriteLine(exception.ToString(), VerbosityLevel.Error);
            }

            return await Task.FromResult(exitCode);
        }

        private async Task SetCurrentStateFromTransition(ITransition transition)
        {
            _ = transition ?? throw new ArgumentNullException(nameof(transition));

            IState newState = await transition.GetState(this).ConfigureAwait(false) ?? throw new NullReferenceException("Transition returned NULL state.");
            IState oldState = CurrentState;
            CurrentState = newState;
            if (oldState != null)
                await oldState.OnExited(newState).ConfigureAwait(false);
            await newState.OnEntered(oldState).ConfigureAwait(false);
        }
    }
}
