import React, {Component} from 'react';
import './PolicyRecord.css';


class PolicyRecord extends Component {


    render() {
        return (
            <div>
                <ul className = "simple-discount-ul">
                    {this.props.policyRecords.map((elem) => (
                        <li  key={elem.id}>
                            <div className = "simple-discount-li-div">
                                <p className= "discName">creator: {elem.creator}  </p>
                                <p className= "discName">Policy Rule: {elem.ruleRelation} </p>
                                <p className= "discName"> Policy Context: {elem.ruleContext}  </p>
                                <p className= "discName"> Rule Type: {elem.ruleType}  </p>
                                <p className= "discName"> category: {elem.category} </p>
                                <p className= "discName">product: {elem.productId} </p>
                                <p className= "discName">min value: {elem.minVal} </p>
                                <p className= "discName"> max value: {elem.maxVal}  </p>
                                <p className= "discName">start date: {elem.startDate} </p>
                                <p className= "discName">end date: {elem.endDate} </p>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default PolicyRecord;