import React, {Component} from 'react';
import * as HiIcons from "react-icons/hi";
import Navbar from "../../components/Navbar/Navbar";
import './Store.css';
import {Link} from "react-router-dom";
import {GlobalContext} from "../../globalContext";
import {Route, Switch} from "react-router-dom";

class StoreContent extends Component {
    constructor(props) {
        super(props);
        this.onProductsButtonClick = this.onProductsButtonClick.bind(this);
    }

    onProductsButtonClick() {
        this.props.history.push(`/storeProducts/${this.props.match.params.storeId}`)
    }

    render() {
        return (
            <main className="main-conatiner">
                <div>
                    <h2>Store Name</h2>
                </div>

                <div className="internal-conatiner">
                    <button className="button-view" onClick={this.onProductsButtonClick}> Store Products </button>
                    <button className="button-view"> Store Staff </button>
                    <button className="button-view"> Store History </button>
                </div>
            </main>
        );
    }
}

export class Store extends Component {
    render() {
        return (
            <div className="grid-container">
                <header className="header-container" >
                    <a href="/">E - commerce Application</a>
                    <div>
                        <h3>{this.context.isLoggedIn ? this.context.username : ''}</h3>
                    </div>


                    <Link
                        className="icons"
                        to={{
                            pathname: "/ShoppingCart"
                        }}
                    >
                        <HiIcons.HiShoppingCart />
                    </Link>


                    <Navbar/>

                </header>

                <Switch>
                    <Route path={`${this.props.match.path}/:storeId`} component={StoreContent} />
                    <Route path={this.props.match.path}>
                        <h3 className='center-screen'>No store selected</h3>
                    </Route>
                </Switch>
                <footer> End of Store</footer>
            </div>
        )
    }
}

Store.contextType = GlobalContext;
