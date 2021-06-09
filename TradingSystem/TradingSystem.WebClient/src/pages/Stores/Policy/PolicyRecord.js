import React, {Component} from 'react';
import './PolicyRecord.css';
import {formatDate, formatFloat} from "../../../utils";
import ConditionalRender from "../../../ConditionalRender";

class PolicyRecord extends Component {


    render() {
        if (this.props.policyRecords == null) {
            return null;
        }

        return (
            <div>
                <ul className = "simple-discount-ul">
                    {this.props.policyRecords.map((elem) => {
                        if (typeof elem.startDate === 'string') {
                            elem.startDate = new Date(elem.startDate);
                        }
                        if (typeof elem.endDate === 'string') {
                            elem.endDate = new Date(elem.endDate);
                        }

                        return (
                            <li key={elem.id}>
                                <div className="simple-discount-li-div">
                                    <p className="discName">creator: {elem.creator}  </p>
                                    <p className="discName">Policy Rule: {elem.ruleRelation === "Condition" ? "Xor" : elem.ruleRelation} </p>
                                    <p className="discName"> Policy Context: {elem.ruleContext}  </p>
                                    <p className="discName"> Rule Type: {elem.ruleType || "Simple"}  </p>
                                    <ConditionalRender
                                        condition={elem.ruleContext === 'Category'}
                                        render={() => (<p className="discName"> category: {elem.category} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.ruleContext === 'Product'}
                                        render={() => (
                                            <p className="discName">product: {this.props.storeProductsMap[elem.productId].name} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.ruleType != null && elem.ruleType !== 'Time' && elem.minValue != null}
                                        render={() => (<p className="discName">min value: {elem.ruleType === 'Quantity' ? elem.minValue : formatFloat(elem.minValue)} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.ruleType != null && elem.ruleType !== 'Time' && elem.maxValue != null}
                                        render={() => (<p className="discName">max value: {elem.ruleType === 'Quantity' ? elem.maxValue : formatFloat(elem.maxValue)} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.ruleType === 'Time' && elem.startDate != null}
                                        render={() => (
                                            <p className="discName">start date: {formatDate(elem.startDate)} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.ruleType === 'Time' && elem.endDate != null}
                                        render={() => (<p className="discName">end date: {formatDate(elem.endDate)} </p>)}
                                    />
                                </div>
                            </li>
                        )
                    })}
                </ul>
            </div>
        );
    }
}

export default PolicyRecord;