import React, {Component} from 'react';
import HistoryProduct from "./HistoryProduct";

class HistoryProducts extends Component {
    render() {
        return (
            <div>
                <ul className = "products">
                    {this.props.products.map((product) => (
                        <li key={product.id}>
                            <HistoryProduct product={product} />
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default HistoryProducts;