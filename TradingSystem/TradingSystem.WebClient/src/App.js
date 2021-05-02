import React from 'react';
import {useEffect} from "react";
import Routes from "./routes";
import axios from "axios";
import SimpleAlertDialog from "./components/simpleAlertDialog";

export function useTitle(title) {
  useEffect(() => {
    document.title = 'Ecommerce - ' + title;
  }, [title]);
}

export let guestUsername;

// TODO: make it flexible based on environment build
// Setup the base URL of the web API server.
axios.defaults.baseURL = 'https://localhost:5001/api';
guestUsername = '';

class App extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      showErrorDialog: false,
      errorMessage: null
    };

    this.handleCloseErrorDialog = this.handleCloseErrorDialog.bind(this);
  }

  handleCloseErrorDialog = () => {
    this.setState({
      showErrorDialog: false
    })
  };

  async componentDidMount() {
    await this.init();
  }

  async componentWillUnmount() {
    guestUsername = '';
  }

  render() {
    if (this.state.showErrorDialog) {
      return <SimpleAlertDialog message={this.state.errorMessage} isShown={true} onClose={this.handleCloseErrorDialog} />;
    }
    return <Routes />;
  }

  async init() {
    try {
      let response = await axios.post('/UserGateway/AddGuest/', { });
      guestUsername = response.data;
      this.setState({
        showErrorDialog: false,
        errorMessage: null
      })
    }
    catch (e) {
      guestUsername = '';
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
