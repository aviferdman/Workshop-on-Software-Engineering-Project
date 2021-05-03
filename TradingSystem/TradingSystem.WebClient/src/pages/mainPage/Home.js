import React from "react";
import './Home.css';
import data from './productData.json';
import Products from "../../components/Products";
import SearchBar from "../../components/searchBar";
import Filter from "../../components/Filter";

    export class Home extends React.Component {
        constructor(props) {
            super(props);
            this.state = {
                products: data.products,
                category: "",
                ordered: ""
            };
        }
        sortProducts = (event) =>{
          //impl
            const sort = event.target.value;
            console.log(event.target.value);
            this.setState((state) => ({
                sort: sort,
                products: this.state.products.slice().sort((a,b) => (
                    sort === "lowest" ?
                        ((a.price > b.price)? 1 : -1):
                    sort === "highest" ?
                        ((a.price < b.price)? 1 : -1):
                        ((a.id < b.id)? 1 : -1)

                ))
            }));
        };

        filterProducts = (event) => {
            //impl
            console.log(event.target.value);
            if(event.target.value === ""){
                this.setState({category: event.target.value , products:data.products})
            }
            else{
                this.setState({
                    category: event.target.value,
                    products: data.products.filter(product => product.category === event.target.value),
                });
            }
        };

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
                                <Filter
                                    count={this.state.products.length}
                                    category={this.state.category}
                                    sort={this.state.ordered}
                                    filterProducts={this.filterProducts}
                                    sortProducts={this.sortProducts} >
                                </Filter>
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
