import React from 'react';
import {useEffect} from "react";
import Routes from "./routes";
import axios from "axios";
import SimpleAlertDialog from "./components/simpleAlertDialog";
import {GlobalContext} from "./globalContext";

export function useTitle(title) {
  useEffect(() => {
    document.title = 'Ecommerce - ' + title;
  }, [title]);
}

// TODO: make it flexible based on environment build
// Setup the base URL of the web API server.
axios.defaults.baseURL = 'https://localhost:5001/api';

class App extends React.Component {
  constructor(props) {
    super(props);

    this.handleCloseErrorDialog = this.handleCloseErrorDialog.bind(this);
    this.setUsername = this.setUsername.bind(this);

    this.state = {
      showErrorDialog: false,
      errorMessage: null,
      globalContext: {
        username: '',
        setUsername: this.setUsername,
        webSocket: null,
        setWebSocket: () => {}
      }
    };
  }

  setUsername = username => {
    this.setState({
      globalContext: {
        ...this.state.globalContext,
        username: username,
      }
    });
  }

  handleCloseErrorDialog = () => {
    this.setState({
      showErrorDialog: false
    })
  };

  async componentDidMount() {
    await this.init();
  }

  componentWillUnmount() {
    this.setState({
      globalContext: {
        ...this.state.globalContext,
        username: '',
      }
    });
  }

  render() {
    if (this.state.showErrorDialog) {
      return <SimpleAlertDialog message={this.state.errorMessage} isShown={true} onClose={this.handleCloseErrorDialog} />;
    }
    return (
      <GlobalContext.Provider value={this.state.globalContext}>
        <Routes />;
      </GlobalContext.Provider>
    );
  }

  async init() {
    try {
      let response = await axios.post('/UserGateway/AddGuest/', { });
      this.setState({
        showErrorDialog: false,
        errorMessage: null,
        globalContext: {
          ...this.state.globalContext,
          username: response.data,
        }
      });
    }
    catch (e) {
      this.setState({
        globalContext: {
          ...this.state.globalContext,
          username: '',
        }
      });
      if (e.response) {
        // The request was made and the server responded with a status code
        // that falls out of the range of 2xx
        if (e.response.status === 400) {
          this.setState({
            errorMessage: 'Failed to connect as a guest.'
          });
        }
      }
      if (!this.state.errorMessage) {
        this.setState({
          errorMessage: 'Unknown error occurred: ' + e.message
        });
      }
      this.setState({
        showErrorDialog: true
      });
    }
  }
}

export default App;
