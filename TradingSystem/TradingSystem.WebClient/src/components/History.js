import React, {Component} from 'react';
import './History.css';


class History extends Component {
    render() {
        return (
            <div>
                <ul className = "history">
                    {this.props.history.map((elem) => (
                        <li key={elem.id}>
                            <div className = "history_s">
                                <a href={"#" + elem.id}>
                                    <p className= "histName">History:  {elem.id}</p>
                                </a>

                                <p className= "histName">reception id:  {elem.reception_ID}</p>

                                    <p className= "histName"> delivery id: {elem.delivery_ID} </p>

                                <button className= "button primary"> show producs </button>

                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default History;