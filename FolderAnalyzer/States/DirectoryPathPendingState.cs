using FolderAnalyzer.FSM;
using FolderAnalyzer.Helpers;
using FolderAnalyzer.States.Transitions;
using System;
using System.Threading.Tasks;

namespace FolderAnalyzer.States
{
    public class DirectoryPathPendingState : StateBase<DirectoryPathPendingState, DirectoryPathPendingStateTransition, Func<string, ITransition<string>>>
    {
        private Func<string, ITransition> _nextTransitionFactory;

        public override bool IsFinal => false;

        public DirectoryPathPendingState(FiniteStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        public override async Task Initialize(Func<string, ITransition<string>> args)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));
            _nextTransitionFactory = args;
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public override async Task<ITransition> Update()
        {
            ConsoleHelper.Write("Folder path: ");
            string directoryPath = Console.ReadLine();
            ITransition nextTransition = _nextTransitionFactory(directoryPath);
            return await Task.FromResult(nextTransition).ConfigureAwait(false);
        }
    }
}
