using System.Threading.Tasks;

namespace FolderAnalyzer.FSM
{
    public interface IState
    {
        FiniteStateMachine StateMachine { get; }

        bool IsFinal { get; }

        Task OnEntered(IState oldState);

        Task<ITransition> Update();

        Task OnExited(IState newState);
    }
}
