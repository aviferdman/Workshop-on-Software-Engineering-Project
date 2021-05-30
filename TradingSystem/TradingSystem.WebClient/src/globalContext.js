import React from 'react'

export const UserRole = Object.freeze({
    "guest": 1,
    "member": 2,
    "admin": 3
});

export const GlobalContext = React.createContext({
    username: '',
    setUsername: () => {},
    role: null,
    webSocket: null,
    setWebSocket: () => {},
    logout: () => {},
});