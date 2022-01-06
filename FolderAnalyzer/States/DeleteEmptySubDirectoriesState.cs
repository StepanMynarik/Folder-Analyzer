using FolderAnalyzer.FSM;
using FolderAnalyzer.Helpers;
using FolderAnalyzer.States.Transitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FolderAnalyzer.States
{
    public class DeleteEmptySubDirectoriesState : StateBase<DeleteEmptySubDirectoriesState, DeleteEmptySubDirectoriesStateTransition, string>
    {
        private static readonly EnumerationOptions _enumerationOptions = new EnumerationOptions
        {
            RecurseSubdirectories = false
        };
        private string _directoryPath;

        public override bool IsFinal => false;

        public DeleteEmptySubDirectoriesState(FiniteStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        public override async Task Initialize(string args)
        {
            _directoryPath = args;
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public override async Task<ITransition> Update()
        {
            static bool ignoreFileSystemEntryPredicate(string filePath) =>
                filePath.EndsWith(".meta", StringComparison.OrdinalIgnoreCase) ||
                filePath.EndsWith(".asmdef", StringComparison.OrdinalIgnoreCase) ||
                filePath.EndsWith(".asmdef.meta", StringComparison.OrdinalIgnoreCase);
            var deletedDirectories = new List<string>();
            DeleteEmptySubDirectoriesR(_directoryPath, ignoreFileSystemEntryPredicate, deletedDirectories);
            ConsoleHelper.WriteLine($"{deletedDirectories.Count} folders deleted.");
            ConsoleHelper.WriteEmptyLine();

            static ITransition<string> factory(string directoryPath) => new DeleteEmptySubDirectoriesStateTransition(directoryPath);
            return await Task.FromResult<ITransition>(new DirectoryPathPendingStateTransition(factory)).ConfigureAwait(false);
        }

        private static void DeleteEmptySubDirectoriesR(string directoryPath, Func<string, bool> ignoreFileSystemEntryPredicate, List<string> deletedDirectories)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                ConsoleHelper.WriteLine("Invalid folder path.", VerbosityLevel.Error);
                return;
            }
            
            try
            {
                foreach (var subDirectoryPath in Directory.EnumerateDirectories(directoryPath, "*", _enumerationOptions))
                {
                    DeleteEmptySubDirectoriesR(subDirectoryPath, ignoreFileSystemEntryPredicate, deletedDirectories);
                }

                var fileSystemEntries = Directory.EnumerateFileSystemEntries(directoryPath, "*", _enumerationOptions);
                var isEmpty = fileSystemEntries.All(ignoreFileSystemEntryPredicate);
                if (isEmpty)
                {
                    Directory.Delete(directoryPath, true);
                    deletedDirectories.Add(directoryPath);
                    ConsoleHelper.WriteLine($"Folder '{directoryPath}' deleted.", VerbosityLevel.Success);
                }
            }
            catch (UnauthorizedAccessException)
            {
                ConsoleHelper.WriteLine($"Folder '{directoryPath}' could not be analyzed. [UnauthorizedAccessException]", VerbosityLevel.Error);
            }
            catch (DirectoryNotFoundException)
            {
                ConsoleHelper.WriteLine($"Folder '{directoryPath}' could not be analyzed. [DirectoryNotFoundException]", VerbosityLevel.Error);
            }
            catch (Exception exception)
            {
                ConsoleHelper.WriteLine(exception.Message, VerbosityLevel.Error);
            }
        }
    }
}
