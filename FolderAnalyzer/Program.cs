using FolderAnalyzer.FSM;
using FolderAnalyzer.States.Transitions;
using System.Threading.Tasks;

namespace FolderAnalyzer
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var defaultTransition = new MenuStateTransition();
            var stateMachine = await FiniteStateMachine.Create(defaultTransition).ConfigureAwait(false);
            int exitCode = await stateMachine.StartLoop().ConfigureAwait(false);

            return exitCode;
        }
    }
}
