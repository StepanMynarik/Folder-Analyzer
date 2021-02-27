import { contextBridge, dialog, ipcRenderer } from "electron";

console.log('preload loaded');

contextBridge.exposeInMainWorld(
    'api', {
        showMessageBox: (message: string) => {
            dialog.showMessageBox({
                type:'info',
                title:'Info',
                message:message,
                buttons:['Howdy?','Alright']
            }
            ).then(result=>{
               console.log(result.response)
            })
        },
    }
);

declare global {
    interface Window {
        api: {
            showMessageBox(message: string): void
        }
    }
}
