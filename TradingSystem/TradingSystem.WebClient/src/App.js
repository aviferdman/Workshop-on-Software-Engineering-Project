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
    this.setWebSocket = this.setWebSocket.bind(this);

    this.state = {
      showErrorDialog: false,
      errorMessage: null,
      globalContext: {
        username: '',
        setUsername: this.setUsername,
        isLoggedIn: false,
        webSocket: null,
        setWebSocket: this.setWebSocket
      }
    };
  }

  setUsername = (username, loggedIn) => {
    this.setState({
      globalContext: {
        ...this.state.globalContext,
        username: username,
        isLoggedIn: !!loggedIn
      }
    });
  }

  setWebSocket = username => {
    // Create WebSocket connection.
    const socket = new WebSocket('wss://localhost:5001/login');

    // Connection opened
    socket.addEventListener('open', function (event) {
      console.log('web socket connection opened');
      socket.send(username);
      console.log('sent username');
    });

    this.setState({
      globalContext: {
        ...this.state.globalContext,
        webSocket: socket
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
    return (
      <GlobalContext.Provider value={this.state.globalContext}>
        <Routes />
        <SimpleAlertDialog message={this.state.errorMessage} isShown={this.state.showErrorDialog} onClose={this.handleCloseErrorDialog} />
      </GlobalContext.Provider>
    );
  }

  async init() {
    try {
      let response = await axios.post('/UserGateway/AddGuest/', { });
      this.setState({
        showErrorDialog: false,
        errorMessage: null,
      });
      this.setUsername(response.data, false);
    }
    catch (e) {
      this.setUsername('', false);
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
