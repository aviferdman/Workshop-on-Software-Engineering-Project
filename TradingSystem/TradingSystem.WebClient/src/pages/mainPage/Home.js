import React from "react";
import './Home.css';
import data from './productData.json';
import Products from "../../components/Products";
import SearchBar from "../../components/searchBar";
import Filter from "../../components/Filter";
import MenuIcon from '@material-ui/icons/Menu';
import Navbar from "../../components/Navbar/Navbar";
import * as HiIcons from "react-icons/hi";
import {IconContext} from "react-icons";
import Cart from "../../components/Cart";

    export class Home extends React.Component {
        constructor(props) {
            super(props);
            this.state = {
                products: data.products,
                cartItems: [],
                category: "",
                ordered: ""
            };
        }

        removeFromCart = (product) => {
            const cartItems = this.state.cartItems.slice();
            this.setState({
                cartItems: cartItems.filter(x => x.id !== product.id),
            });
        };

        addToCart = (product) => {
            let alreadyInCart = false;
            const cartItems = this.state.cartItems.slice()
            cartItems.forEach( (item) => {
                if(item.id === product.id){
                    //console.log(this.state.cartItems.length);
                    item.count++;
                    alreadyInCart = true;
                }
            });

            if(!alreadyInCart){
                cartItems.push({...product, count: 1})
            }
            this.setState({cartItems: cartItems});
        };

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
                    <header className="header-container">
                        <a href="/">E - commerce Application</a>
                        <div>
                            <h3> User Name </h3>
                        </div>


                        <button className="icons">
                            <HiIcons.HiShoppingCart />
                        </button>

                        <Navbar></Navbar>

                    </header>

                    <div>

                    </div>
                    <search classname="search-grid">


                        <SearchBar></SearchBar>
                        {" "}

                        <div className="filter-category">
                            Category{" "}
                            <select value={this.props.category} onChange={this.props.filterProducts}>
                                <option value="">ALL</option>
                                <option value="Dairy">Dairy</option>
                                <option value="Pastries">Pastries</option>
                                <option value="Beverage">Beverage</option>

                            </select>
                        </div>

                        <div className="rangeinput">
                            <input
                                type="text"
                                placeholder="Min"
                                className="sizeInput"
                            />

                        </div>

                        {"  "}

                        <div className="rangeinput" >
                            <input
                                type="text"
                                placeholder="Max"
                                className="sizeInput"
                            />

                        </div>

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
                                <Products products={this.state.products} addToCart={this.addToCart}></Products>
                            </div>
                            <div className="sidebar">
                                <Cart cartItems={this.state.cartItems} removeFromCart={this.removeFromCart} />
                            </div>
                        </div>

                    </main>
                    <footer> End of products</footer>
                </div>
            )
        }
    }
