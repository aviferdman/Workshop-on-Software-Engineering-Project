import React from "react";
import './MyStores.css';
import Navbar from "../../components/Navbar/Navbar";
import * as HiIcons from "react-icons/hi";
import Store from "./StoreList"
import axios from "axios";
import {GlobalContext} from "../../globalContext";

export class MyStores extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            stores: []
        };

        this.onCreateStoreButtonClick = this.onCreateStoreButtonClick.bind(this);
    }

    async componentDidMount() {
        await this.fetchStores();
    }

    async fetchStores() {
        try {
            let response = await axios.post('/Stores/MyStores', '"' + this.context.username + '"', {
                headers: {
                    'content-type': 'application/json'
                }
            });
            this.setState({
                stores: response.data
            });
        }
        catch (e) {
            console.error("search error occurred: ", e);
        }
    }

    onCreateStoreButtonClick = e => {
        this.props.history.push('/storeCreate');
    };

    render() {
        return (
            <div className="grid-container">
                <header className="header-container">
                    <a href="/">E - commerce Application</a>
                    <div></div>
                    <button className="icons">
                        <HiIcons.HiShoppingCart />
                    </button>
                    <Navbar></Navbar>
                </header>

                <main>{
                    (!this.state.stores || !this.state.stores.length) ? (
                        <div className='center-screen my-stores-no-stores-block'>
                            <div className='my-stores-no-stores-participation-text-block'>You don't participate in any stores</div>
                            <button className='button primary my-stores-create-first-store-button' onClick={this.onCreateStoreButtonClick}>Create my first store</button>
                        </div>
                    ) : (
                        <div>
                            <div className='my-stores-create-store-top-block'>
                                <button className='button primary' onClick={this.onCreateStoreButtonClick}>Create store</button>
                            </div>
                            <Store stores={this.state.stores}/>
                        </div>
                    )
                }</main>
                <footer> End of Stores</footer>
            </div>
        )
    }
}

MyStores.contextType = GlobalContext;
