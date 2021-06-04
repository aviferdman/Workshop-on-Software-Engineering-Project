import React, {Component} from "react";
import './MyStores.css';

class StoresUserView extends Component {
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
                {
                    this.props.stores.length === 0 ? (
                        <h1 className='center-screen'>No Stores</h1>
                    ) : (
                    <ul className = "stores">
                        {this.props.stores.map((store) => (
                                <li key={store.id}>
                                    <div className = "store" >
                                        <a onClick={this.onStoreLinkClick(store.id)} href={`#${store.id}`}>
                                            <p className= "productName">{store.name}</p>
                                        </a>
                                        <button className="button primary" onClick={this.onStoreLinkClick(store.id)}>Show Store</button>
                                    </div>
                                </li>
                        ))}
                    </ul>
                )}
            </div>
        );
    }
}

export default StoresUserView;