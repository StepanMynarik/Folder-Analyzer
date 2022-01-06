using System.Threading.Tasks;

namespace FolderAnalyzer.FSM
{
    public interface ITransition
    {
        Task<IState> GetState(FiniteStateMachine stateMachine);
    }

    public interface ITransition<out TArgs> : ITransition
    {
        TArgs Args { get; }
    }
}
