import React, {Component} from "react";


class Stores extends Component {
    constructor(props) {
        super(props);
        this.onStoreLinkClick = this.onStoreLinkClick.bind(this);
    }

    onStoreLinkClick = id => e => {
        e.preventDefault();
        this.props.history.push(`/store/${id}`);
        return false;
    }

    render() {
        return (
            <div>
                <ul className = "stores">
                    {this.props.stores.map((store) => (
                        <li key={store.id}>
                            <div className = "store">
                                <a onClick={this.onStoreLinkClick(store.id)} href={`#${store.id}`}>
                                    <p className= "productName">{store.name}</p>
                                </a>
                                <button className="button primary" onClick={this.onStoreLinkClick(store.id)}>Manage Store</button>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default Stores;