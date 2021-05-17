import React from 'react'

export const GlobalContext = React.createContext({
    username: '',
    setUsername: () => {},
    webSocket: null,
    setWebSocket: () => {}
});