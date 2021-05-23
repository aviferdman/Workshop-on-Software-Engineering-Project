import React from "react";
import './MyStores.css';
import Navbar from "../../components/Navbar/Navbar";
import * as HiIcons from "react-icons/hi";
import Store from "./StoreList"
import axios from "axios";
import {GlobalContext, UserRole} from "../../globalContext";
import {Link} from "react-router-dom";
import Header from "../../header";

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
        let response;
        try {
            response = await axios.post('/Stores/MyStores', '"' + this.context.username + '"', {
                headers: {
                    'content-type': 'application/json'
                }
            });
        }
        catch (e) {
            console.error("search error occurred: ", e);
            return;
        }
        this.setState({
            stores: response.data
        });
    }

    onCreateStoreButtonClick = e => {
        this.props.history.push('/storeCreate');
    };

    render() {
        return (
            <div className="grid-container">
                <Header />

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
                            <Store stores={this.state.stores} history={this.props.history} />
                        </div>
                    )
                }</main>
                <footer> End of Stores</footer>
            </div>
        )
    }
}

MyStores.contextType = GlobalContext;
