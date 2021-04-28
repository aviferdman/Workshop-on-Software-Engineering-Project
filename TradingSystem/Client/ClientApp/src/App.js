import React, { Component } from 'react';
import LoginPage from "./pages/Login";

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
        <LoginPage></LoginPage>
    );
  }
}
