import React, {Component} from 'react';
import './AdminActions.css';
import {Route, Switch} from "react-router-dom";
import {GlobalContext} from "../../globalContext";
import Header from "../../header";

class AdminContent extends Component {
    constructor(props) {
        super(props);
        this.onProductsButtonClick = this.onProductsButtonClick.bind(this);
    }

    onProductsButtonClick() {
        this.props.history.push(`/adminHistory/`)
    }

    render() {
        return (
            <main className="admin-main-container">
                <div>
                    <h2>Admin Name</h2>
                </div>

                <div className="internal-conatiner">
                    <button className="button-view" onClick={this.onProductsButtonClick}> View History </button>
                </div>
            </main>
        );
    }
}

export class AdminActions extends Component {
    render() {
        return (
            <div className="grid-container">
                <Header />

                <Switch>
                    <Route path={`/:adminHistory`} component={AdminContent} />
                    <Route path={this.props.match.path}>
                        <h3 className='center-screen'>No store BlaBla</h3>
                    </Route>
                </Switch>
                <footer> End of Admin Control Page</footer>
            </div>
        )
    }
}

AdminActions.contextType = GlobalContext;
