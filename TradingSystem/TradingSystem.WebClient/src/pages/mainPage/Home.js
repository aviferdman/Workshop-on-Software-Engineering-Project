import React from "react";
import './Home.css';
import data from '../../data/productData.json';
import SearchBar from "../../components/searchBar";
import Filter from "../../components/Filter";
import Cart from "../../components/Cart";
import axios from "axios";
import {GlobalContext} from "../../globalContext";
import HomeProducts from "../../components/HomeProducts";
import Header from "../../header";

export class Home extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            products: [],
            cartItems: [],
            category: "",
            ordered: "",
            searchKeywords: "",
            searchCategory: "",
            searchPriceMin: "",
            searchPriceMax: "",
        };
    }

    onSearchInputChange = e => {
        this.setState({
            searchKeywords: e.target.value
        });
    };

    onSearchCategoryChange = e => {
        this.setState({
            searchCategory: e.target.value
        });
    };

    onSearchPriceMinChange = e => {
        this.setState({
            searchPriceMin: e.target.value
        });
    };

    onSearchPriceMaxChange = e => {
        this.setState({
            searchPriceMax: e.target.value
        });
    };

    onSearch = async () => {
        let response;
        try {
            response = await axios.get('/Products/Search', {
                params: {
                    keywords: this.state.searchKeywords,
                    category: this.state.searchCategory,
                    priceRange_Low: this.state.searchPriceMin === "" ? -1 : this.state.searchPriceMin,
                    priceRange_High: this.state.searchPriceMax === "" ? -1 : this.state.searchPriceMax,
                }
            });
        }
        catch (e) {
            alert('An error occurred: ' + (e.response && e.response.data) || e.message);
            console.error("search error occurred: ", e);
            return;
        }
        this.setState({
            products: response.data
        });
    };

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
                item.count += product.quantity;
                alreadyInCart = true;
            }
        });

        if(!alreadyInCart){
            product.count = product.cartQuantity;
            product._inCart = true;
            cartItems.push(product);
        }
        this.setState({cartItems: cartItems});
    };

    sortProducts = (event) =>{
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

    onProceedToPurchase = () => {
        this.props.history.push('/ShoppingCart');
    }

    render() {
        return (
            <div className="grid-container home">
                <Header />

                <div>

                </div>
                <div className="search-grid search-block">


                    <SearchBar onChange={this.onSearchInputChange} value={this.state.searchKeywords} onSearch={this.onSearch}/>
                    {" "}

                    <div >
                        Category{" "}
                        <select value={this.state.searchCategory} onChange={this.onSearchCategoryChange}>
                            <option value="">ALL</option>
                            <option value="Dairy">Dairy</option>
                            <option value="Pastries">Pastries</option>
                            <option value="Beverage">Beverage</option>

                        </select>
                    </div>

                    <div className="rangeinput">
                        <input
                            type="number"
                            placeholder="Min"
                            className="sizeInput"
                            value={this.state.searchPriceMin}
                            onChange={this.onSearchPriceMinChange}
                        />

                    </div>

                    {"  "}

                    <div className="rangeinput" >
                        <input
                            type="number"
                            placeholder="Max"
                            className="sizeInput"
                            value={this.state.searchPriceMax}
                            onChange={this.onSearchPriceMaxChange}
                        />
                    </div>
                </div>

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
                            <HomeProducts products={this.state.products} addToCart={this.addToCart} history={this.props.history} />
                        </div>
                    </div>
                </main>
                <footer> End of products</footer>
            </div>
        )
    }
}

Home.contextType = GlobalContext;
