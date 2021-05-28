import React, {Component} from 'react';
import './Store.css';
import {Route, Switch} from "react-router-dom";
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import {StoreProducts} from "./StoreProducts";
import {StoreStaff} from "./StoreStaff";
import {StoreHistory} from "./StoreHistory";

class StoreContent extends Component {
    onNavigationButtonClick = route => e => {
        this.props.history.push(`/store/${route}/${this.props.match.params.storeId}`)
    }

    render() {
        return (
            <main className="main-conatiner">
                <div>
                    <h2>Store Name</h2>
                </div>

                <div className="internal-conatiner">
                    <button className="button-view" onClick={this.onNavigationButtonClick('products')}> Store Products </button>
                    <button className="button-view" onClick={this.onNavigationButtonClick('staff')}> Store Staff </button>
                    <button className="button-view" onClick={this.onNavigationButtonClick('history')}> Store History </button>
                </div>
            </main>
        );
    }
}

export class Store extends Component {
    render() {
        return (
            <div className="grid-container">
                <Header />

                <Switch>
                    <Route path={`${this.props.match.path}/products/:storeId`} component={StoreProducts} />
                    <Route path={`${this.props.match.path}/staff/:storeId`} component={StoreStaff} />
                    <Route path={`${this.props.match.path}/history/:storeId`} component={StoreHistory} />
                    <Route path={`${this.props.match.path}/:storeId`} component={StoreContent} />
                    <Route path={this.props.match.path}>
                        <h3 className='center-screen'>No store selected</h3>
                    </Route>
                </Switch>
                <footer>End of Store</footer>
            </div>
        )
    }
}

Store.contextType = GlobalContext;
