using FolderAnalyzer.FSM;
using FolderAnalyzer.Helpers;
using FolderAnalyzer.States.Transitions;
using System;
using System.Threading.Tasks;

namespace FolderAnalyzer.States
{
    public class MenuState : StateBase<MenuState, MenuStateTransition>
    {
        public override bool IsFinal => false;

        public MenuState(FiniteStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        public override async Task<ITransition> Update()
        {
            ConsoleHelper.WriteLine($"Folder Analyzer {Constants.Version}");
            ConsoleHelper.WriteLine("=== MENU ===");
            ConsoleHelper.WriteLine("1) Delete empty sub-folders");
            ConsoleHelper.WriteLine("9) Exit");
            while (true)
            {
                switch (Console.ReadKey(true))
                {
                    case {Key: ConsoleKey.D1}:
                    case {Key: ConsoleKey.NumPad1}:
                        ConsoleHelper.WriteEmptyLine();
                        static ITransition<string> factory(string directoryPath) => new DeleteEmptySubDirectoriesStateTransition(directoryPath);
                        return await Task.FromResult<ITransition>(new DirectoryPathPendingStateTransition(factory)).ConfigureAwait(false);
                    case {Key: ConsoleKey.D9}:
                    case {Key: ConsoleKey.NumPad9}:
                        return await Task.FromResult<ITransition>(new FinalStateTransition()).ConfigureAwait(false);
                    default:
                        continue;
                }
            }
        }
    }
}
