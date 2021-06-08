import React, {Component} from 'react';
import './History.css';


class History extends Component {
    directToProductsPage = paymentId => () => {
        let params;
        if (this.props.storeId != null && this.props.username != null) {
            console.error('invalid history type');
            return;
        }
        else if (this.props.storeId == null && this.props.username != null) {
            params = `&user=1`;
        }
        else if (this.props.storeId != null && this.props.username == null) {
            params = `&storeId=${this.props.storeId}`;
        }
        else {
            params = '';
        }

        this.props.history.push(`/historyProducts?paymentId=${paymentId}${params}`);
    }

    render() {
        return (
            <div>
                <ul className = "history">
                    {this.props.historyRecords.map((elem) => (
                        <li key={elem.paymentId}>
                            <div className = "history_s">
                                {/*<a href={"#" + elem.id}>*/}
                                {/*    <p className= "histName">History:  {elem.id}</p>*/}
                                {/*</a>*/}
                                <p className= "histName"> {<text style={{fontWeight: "bold"}}>Customer: </text>} {elem.username}</p>
                                <p className= "histName"> {<text style={{fontWeight: "bold"}}>Store: </text>} {elem.storeName}</p>
                                <p className= "histName">{<text style={{fontWeight: "bold"}}>Reception: </text>} {elem.paymentId}</p>

                                    <p className= "histName"> {<text style={{fontWeight: "bold"}}>Delivery: </text>}{elem.deliveryId} </p>

                                <button className= "button primary" onClick={this.directToProductsPage(elem.paymentId)}> Show Products </button>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default History;