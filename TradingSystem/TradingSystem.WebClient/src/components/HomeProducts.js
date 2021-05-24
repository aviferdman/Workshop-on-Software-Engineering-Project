import React, {Component} from 'react';
import HomeProduct from "./HomeProduct";

class HomeProducts extends Component {
    render() {
        return (
            <div>
                <ul className = "products">
                    {this.props.products.map((product) => (
                        <li key={product.id}>
                            <HomeProduct product={product} addToCart={this.props.addToCart} history={this.props.history} />
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default HomeProducts;