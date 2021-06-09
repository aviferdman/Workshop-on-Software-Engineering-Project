import React, {Component} from 'react';
import './History.css';
import ConditionalRender from "../ConditionalRender";

export const HistoryContext = {
    User: 'User',
    Store: 'Store',
    Admin: 'Admin',
};

class History extends Component {
    constructor(props) {
        super(props);
        this.state = {
            historyContext: null,
        };
    }

    componentDidMount() {
        let historyContext;
        if (this.props.storeId != null && this.props.username != null) {
            console.error('invalid history type');
            return;
        }
        else if (this.props.storeId == null && this.props.username != null) {
            historyContext = HistoryContext.User;
        }
        else if (this.props.storeId != null && this.props.username == null) {
            historyContext = HistoryContext.Store;
        }
        else {
            historyContext = HistoryContext.Admin;
        }

        this.setState({
            historyContext: historyContext,
        })
    }

    directToProductsPage = paymentId => () => {
        let historyContext = this.state.historyContext;

        let params;
        if (historyContext === HistoryContext.User) {
            params = `&user=1`;
        }
        else if (historyContext === HistoryContext.Store) {
            params = `&storeId=${this.props.storeId}`;
        }
        else if (historyContext === HistoryContext.Admin) {
            params = '';
        }
        else {
            return;
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
                                <ConditionalRender
                                    condition={this.state.historyContext !== HistoryContext.User}
                                    render={() => (<p className= "histName"> {<text style={{fontWeight: "bold"}}>Customer: </text>} {elem.username}</p>)}
                                />
                                <ConditionalRender
                                    condition={this.state.historyContext !== HistoryContext.Store}
                                    render={() => (<p className= "histName"> {<text style={{fontWeight: "bold"}}>Store: </text>} {elem.storeName}</p>)}
                                />
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