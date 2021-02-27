import * as React from 'react';
import * as ReactDOM from 'react-dom';

function render() {
  ReactDOM.render(
    <React.StrictMode>
        <button onClick={()=>{
        window.api.showMessageBox('message');
        }}>Select folder</button>
    </React.StrictMode>,
    document.getElementById('root'));
}

render();