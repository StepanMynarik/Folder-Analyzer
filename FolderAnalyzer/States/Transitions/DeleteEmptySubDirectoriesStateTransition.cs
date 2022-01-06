using FolderAnalyzer.FSM;

namespace FolderAnalyzer.States.Transitions
{
    public class DeleteEmptySubDirectoriesStateTransition : TransitionBase<DeleteEmptySubDirectoriesStateTransition, DeleteEmptySubDirectoriesState, string>
    {
        public DeleteEmptySubDirectoriesStateTransition(string args)
            : base(args)
        {
        }
    }
}
