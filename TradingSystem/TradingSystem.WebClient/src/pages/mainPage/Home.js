import React from "react";
import './Home.css';
import SearchBar from "../../components/searchBar";
import Filter from "../../components/Filter";
import axios from "axios";
import {GlobalContext} from "../../globalContext";
import HomeProducts from "../../components/HomeProducts";
import Header from "../../header";
import * as api from '../../api';
import {categories} from "../../api";
import {alertRequestError_default} from "../../utils";

export class Home extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            categories: null,
            products: null,
            productsFiltered: null,
            cartItems: [],
            category: "",
            ordered: "",
            searchKeywords: "",
            searchCategory: "",
            searchPriceMin: "",
            searchPriceMax: "",
        };
    }

    async componentDidMount() {
        await this.fetchCategories();
    }

    async fetchCategories() {
        await api.categories.fetchAll()
            .then(categories => {
                this.setState({
                    categories: categories,
                });
            }, alertRequestError_default);
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
            products: response.data,
            productsFiltered: response.data,
        });
        this.sortProductsByCurrent();
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
        this.sortProductsCore(sort);
    };

    sortProductsByCurrent() {
        this.sortProductsCore(this.state.sort);
    }

    sortProductsCore(sort) {
        let products = this.sortProductsArray(this.state.products, sort);
        let productsFiltered = this.sortProductsArray(this.state.productsFiltered, sort);
        this.setState({
            sort: sort,
            products: products,
            productsFiltered: productsFiltered,
        });
    }

    sortProductsArray(products, sort) {
        return products.slice().sort((a,b) => (
            sort === "lowest" ?
                ((a.price > b.price)? 1 : -1):
                sort === "highest" ?
                    ((a.price < b.price)? 1 : -1):
                    ((a.id < b.id)? 1 : -1)

        ));
    }

    filterProducts = (event) => {
        if(event.target.value === ""){
            this.setState({category: event.target.value , productsFiltered: this.state.products});
        }
        else{
            this.setState({
                category: event.target.value,
                productsFiltered: this.state.products.filter(product => product.category === event.target.value),
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
                            {this.state.categories == null ? null : this.state.categories.map(category => {
                                return (<option value={category} key={category}>{category}</option>);
                            })}
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
                                count={this.state.productsFiltered == null ? 0 : this.state.productsFiltered.length}
                                categories={this.state.categories}
                                category={this.state.category}
                                sort={this.state.ordered}
                                filterProducts={this.filterProducts}
                                sortProducts={this.sortProducts} >
                            </Filter>
                            {this.state.products === null ? null : (
                                <HomeProducts products={this.state.productsFiltered} addToCart={this.addToCart}
                                              history={this.props.history}/>
                            )}
                        </div>
                    </div>
                </main>
                <footer> End of products</footer>
            </div>
        )
    }
}

Home.contextType = GlobalContext;
