import React, {Component} from "react";


class Stores extends Component {
    render() {
        return (
            <div>
                <ul className = "stores">
                    {this.props.stores.map((store) => (
                        <li key={store.id}>
                            <div className = "store">
                                <a href={"#" + store.id}>
                                    <p className= "productName">{store.name}</p>
                                </a>
                                <button className="button primary">Manage Store</button>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default Stores;