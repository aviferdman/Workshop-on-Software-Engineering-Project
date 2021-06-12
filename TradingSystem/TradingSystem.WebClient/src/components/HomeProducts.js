import React, {Component} from 'react';
import HomeProduct from "./HomeProduct";
import AddBid from "../pages/Stores/StoreBids/AddBid";

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
                                <li key={product.id} >
                                    <div className="control-buttons-bid" >
                                        {/*TODO: Added store id from home search as well*/}
                                            <AddBid product={product}
                                                storeId={product.storeId || this.props.storeId} />
                                    </div>

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