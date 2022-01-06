using System.Threading.Tasks;

namespace FolderAnalyzer.FSM
{
    public abstract class StateBase<TState, TTransition> : IState
        where TState : StateBase<TState, TTransition>
        where TTransition : TransitionBase<TTransition, TState>
    {
        public FiniteStateMachine StateMachine { get; }

        public abstract bool IsFinal { get; }

        protected StateBase(FiniteStateMachine stateMachine)
        {
            StateMachine = stateMachine; 
        }

        public virtual async Task OnEntered(IState oldState)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public abstract Task<ITransition> Update();

        public virtual async Task OnExited(IState newState)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }

    public abstract class StateBase<TState, TTransition, TArgs> : StateBase<TState, TTransition>
        where TState : StateBase<TState, TTransition, TArgs>
        where TTransition : TransitionBase<TTransition, TState, TArgs>
    {
        protected StateBase(FiniteStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        public abstract Task Initialize(TArgs args);
    }
}
