using System;
using System.Threading.Tasks;

namespace FolderAnalyzer.FSM
{
    public abstract class TransitionBase<TTransition, TToState> : ITransition
        where TTransition : TransitionBase<TTransition, TToState>
        where TToState : StateBase<TToState, TTransition>
    {
        public virtual async Task<IState> GetState(FiniteStateMachine stateMachine)
        {
            var state = await Task.FromResult((IState)Activator.CreateInstance(typeof(TToState), stateMachine)).ConfigureAwait(false);
            return state;
        }
    }

    public abstract class TransitionBase<TTransition, TToState, TArgs> : TransitionBase<TTransition, TToState>, ITransition<TArgs>
        where TTransition : TransitionBase<TTransition, TToState, TArgs>
        where TToState : StateBase<TToState, TTransition, TArgs>
    {
        public TArgs Args { get; }

        protected TransitionBase(TArgs args)
        {
            Args = args;
        }

        public override async Task<IState> GetState(FiniteStateMachine stateMachine)
        {
            var state = (TToState)await base.GetState(stateMachine).ConfigureAwait(false);
            await state.Initialize(Args).ConfigureAwait(false);
            return state;
        }
    }
}
