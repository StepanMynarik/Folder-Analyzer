import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Button } from 'primereact/button';
import { ListBox } from 'primereact/listbox';
import { SelectItem } from 'primereact/api';

interface IProps {
}

interface IState {
  folderPaths?: FolderModel[];
}

class FolderModel implements SelectItem {
  label?: string;
  value: string;
  className?: string;
  icon?: string;
  title?: string;
  disabled?: boolean;

  constructor(value: string) {
    this.label = value;
    this.value = value;
    this.title = value;
  }
}

export default class App extends React.PureComponent<IProps, IState> {
  readonly state: IState = {
    folderPaths: [],
  };

  countryTemplate = (option: FolderModel) => {
    return (
      <div>
        <div>{option.label}</div>
        <Button onClick={() => {
          this.setState({folderPaths: this.state.folderPaths.filter(x => x !== option)});
        }}>Remove</Button>
      </div>
    );
  }

  render() {
    return (
      <div>
        <Button onClick={async () => {
          var folderPaths = (await window.api.showOpenFolderDialog()).filePaths;
          this.setState({folderPaths: folderPaths.map(x => new FolderModel(x))});
        }}>Select folder</Button>
        <ListBox
          options={this.state.folderPaths} optionLabel='label' optionValue='value'
          filter filterBy='label'
          itemTemplate={this.countryTemplate} style={{width: '15rem'}} listStyle={{maxHeight: '250px'}} />
      </div>
    );
  }
}

ReactDOM.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
  document.getElementById('root'));
  