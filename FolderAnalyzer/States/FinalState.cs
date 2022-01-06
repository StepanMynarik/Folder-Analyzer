using FolderAnalyzer.FSM;
using FolderAnalyzer.States.Transitions;
using System.Threading.Tasks;

namespace FolderAnalyzer.States
{
    public class FinalState : StateBase<FinalState, FinalStateTransition>
    {
        public override bool IsFinal => true;

        public FinalState(FiniteStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        public override async Task<ITransition> Update()
        {
            return await Task.FromResult<ITransition>(null).ConfigureAwait(false);
        }
    }
}
