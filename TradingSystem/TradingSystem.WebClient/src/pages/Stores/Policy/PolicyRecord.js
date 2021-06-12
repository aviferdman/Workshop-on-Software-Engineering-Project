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
                                <div className="simple-discount-li-div" style={{height: "35rem"}}>
                                    <p className="discName">{<text style={{fontWeight: "bold"}}>Creator: </text>} {elem.creator}  </p>
                                    <p className="discName">{<text style={{fontWeight: "bold"}}>Policy Rule:  </text>}  {elem.ruleRelation === "Condition" ? "Xor" : elem.ruleRelation} </p>
                                    <p className="discName">{<text style={{fontWeight: "bold"}}>Rule Type: </text>} {elem.ruleType || "Simple"}  </p>
                                    <ConditionalRender
                                        condition={elem.ruleType === 'Quantity'}
                                        render={() => [(
                                            <p className="discName">{<text style={{fontWeight: "bold"}}>Policy Context:  </text>} {elem.ruleContext}  </p>
                                        ), (
                                            <ConditionalRender
                                                condition={elem.ruleContext === 'Category'}
                                                render={() => (<p className="discName">  {<text style={{fontWeight: "bold"}}>Category:</text>}  {elem.category} </p>)}
                                            />
                                        ), (
                                            <ConditionalRender
                                                condition={elem.ruleContext === 'Product'}
                                                render={() => {
                                                    let product = this.props.storeProductsMap[elem.productId];
                                                    return (<p className="discName"> {<text style={{fontWeight: "bold"}}>Product: </text>} {(product && product.name) || "<deleted>"} </p>);
                                                }}
                                            />
                                        )]}
                                    />
                                    <ConditionalRender
                                        condition={elem.ruleType != null && elem.ruleType !== 'Time' && elem.minValue != null}
                                        render={() => (<p className="discName"> {<text style={{fontWeight: "bold"}}>Min Value: </text>} {elem.ruleType === 'Quantity' ? elem.minValue : formatFloat(elem.minValue)} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.ruleType != null && elem.ruleType !== 'Time' && elem.maxValue != null}
                                        render={() => (<p className="discName"> {<text style={{fontWeight: "bold"}}>Max Value: </text>} {elem.ruleType === 'Quantity' ? elem.maxValue : formatFloat(elem.maxValue)} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.ruleType === 'Time' && elem.startDate != null}
                                        render={() => (
                                            <p className="discName">{<text style={{fontWeight: "bold"}}>Start Date:</text>} {formatDate(elem.startDate)} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.ruleType === 'Time' && elem.endDate != null}
                                        render={() => (<p className="discName">{<text style={{fontWeight: "bold"}}>End Date:</text>} {formatDate(elem.endDate)} </p>)}
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