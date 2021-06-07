import React, {Component} from 'react';
import HomeProduct from "./HomeProduct";

class HomeProducts extends Component {
    render() {
        return (
            <div>
                {
                    this.props.products.length === 0 ? (
                        <h1 className='center-screen'>No products</h1>
                    ) : (
                        <ul className = "products">
                            {this.props.products.map((product) => (
                                <li key={product.id}>
                                    <HomeProduct product={product}
                                                 addToCart={this.props.addToCart}
                                                 history={this.props.history}
                                                 storeId={this.props.storeId} />
                                </li>
                            ))}
                        </ul>
                    )
                }
            </div>
        );
    }
}

export default HomeProducts;