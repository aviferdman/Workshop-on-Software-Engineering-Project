import React, {Component} from 'react';
import * as HiIcons from "react-icons/hi";
import Navbar from "../../components/Navbar/Navbar";
import './Store.css';
import {Link} from "react-router-dom";
import {GlobalContext} from "../../globalContext";


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


                    <Navbar></Navbar>

                </header>

                <main className="main-conatiner">
                    <div>
                        <h2>Store Name</h2>
                    </div>

                   <div className="internal-conatiner">
                       <button className="button-view"> Store Products </button>
                       <button className="button-view"> Store Staff </button>
                       <button className="button-view"> Store History </button>
                   </div>

                </main>
                <footer> End of Store</footer>
            </div>
        )
    }
}

Store.contextType = GlobalContext;
