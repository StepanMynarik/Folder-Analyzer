using FolderAnalyzer.FSM;
using System;

namespace FolderAnalyzer.States.Transitions
{
    public class DirectoryPathPendingStateTransition : TransitionBase<DirectoryPathPendingStateTransition, DirectoryPathPendingState, Func<string, ITransition<string>>>
    {
        public DirectoryPathPendingStateTransition(Func<string, ITransition<string>> args)
            : base(args)
        {
        }
    }
}
