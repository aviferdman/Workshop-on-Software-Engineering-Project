import React from "react";
import './Home.css';
import data from './productData.json';
import Products from "../../components/Products";
import SearchBar from "../../components/searchBar";

    export class Home extends React.Component {
        constructor(props) {
            super(props);
            this.state = {
                products: data.products,
                size: "",
                sort: ""
            };
        }

        render() {
            return (
                <div className="grid-container">
                    <header>
                        <a href="/">E - commerce Application</a>

                    </header>
                    <search>
                        <SearchBar></SearchBar>
                    </search>
                    <main>
                        <div className="content">
                            <div className="main">
                                <Products products={this.state.products}></Products>
                            </div>
                            <div className="sidebar">Cart Items</div>
                        </div>

                    </main>
                    <footer> End of products</footer>

                </div>
            )
        }
    }
