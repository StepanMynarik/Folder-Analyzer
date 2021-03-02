import { contextBridge, ipcRenderer } from "electron";

contextBridge.exposeInMainWorld(
    'api', {
        showOpenFolderDialog: async (): Promise<Electron.OpenDialogReturnValue> => {
            return await ipcRenderer.invoke('show-open-folder-dialog');
        },
    }
);

declare global {
    interface Window {
        api: {
            showOpenFolderDialog: () => Promise<Electron.OpenDialogReturnValue>
        }
    }
}
