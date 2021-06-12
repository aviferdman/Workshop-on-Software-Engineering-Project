import React, {useEffect} from 'react';
import Routes from "./routes";
import axios from "axios";
import SimpleAlertDialog from "./components/simpleAlertDialog";
import {GlobalContext, UserRole} from "./globalContext";
import './App.css'
import {__RouterContext} from 'react-router'

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
        this.logout = this.logout.bind(this);

        this.state = {
            showErrorDialog: false,
            errorMessage: null,
            globalContext: {
                username: '',
                setUsername: this.setUsername,
                role: null,
                webSocket: null,
                setWebSocket: this.setWebSocket,
                logout: this.logout,
            }
        };
    }

    logoutCore = async (unmounting) => {
        let newUsername = '';
        try {
            let promise = axios.post('/UserGateway/Logout/', {
                username: this.state.globalContext.username,
            });
            if (!unmounting) {
                let response = await promise;
                newUsername = response.data;
            }
        }
        catch (e) {
            newUsername = '';
        }

        return newUsername;
    }

    releaseContext = (unmounting) => {
        this.logoutCore(unmounting).then(newUsername => {
            this.setState({
                globalContext: {
                    ...this.state.globalContext,
                    username: newUsername,
                    role: unmounting ? null : UserRole.guest,
                    webSocket: null
                }
            })
        });
    }

    setUsername = (username, role) => {
        this.setState({
            globalContext: {
                ...this.state.globalContext,
                username: username,
                role: role
            }
        });
    }

    logout = () => {
        if (this.state.globalContext.webSocket) {
            this.state.globalContext.webSocket.close(1000, 'logout');
        }
        this.releaseContext(false);
        this.context.history.push('/login');
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

        // Listen for messages
        socket.addEventListener('message', function (event) {
            console.log('web socket message from server', event.data);
            let notification = JSON.parse(event.data);
            if (!notification.kind || notification.kind === 'Statistics') {
                return;
            }

            alert(`Live notification: ${notification.content}`);
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
        this.releaseContext(true);
    }

    render() {
        return (
            <GlobalContext.Provider value={this.state.globalContext}>
                <Routes/>
                <SimpleAlertDialog message={this.state.errorMessage} isShown={this.state.showErrorDialog}
                                   onClose={this.handleCloseErrorDialog}/>
            </GlobalContext.Provider>
        );
    }

    async init() {
        try {
            let response = await axios.post('/UserGateway/AddGuest/', {});
            this.setState({
                showErrorDialog: false,
                errorMessage: null,
            });
            this.setUsername(response.data, UserRole.guest);
        }
        catch (e) {
            this.setUsername('', null);
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

App.contextType = __RouterContext;

export default App;
