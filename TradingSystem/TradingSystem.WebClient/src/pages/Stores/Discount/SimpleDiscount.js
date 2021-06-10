import React, {Component} from 'react';
import './Discounts.css';
import {formatDate, formatFloat} from "../../../utils";
import ConditionalRender from "../../../ConditionalRender";


class SimpleDiscount extends Component {
    render() {
        if (this.props.simpleDiscountRecords == null) {
            return null;
        }

        return (
            <div>
                <ul className = "simple-discount-ul">
                    {this.props.simpleDiscountRecords.map((elem) => {
                        if (typeof elem.startDate === 'string') {
                            elem.startDate = new Date(elem.startDate);
                        }
                        if (typeof elem.endDate === 'string') {
                            elem.endDate = new Date(elem.endDate);
                        }

                        return (
                            <li  key={elem.id}>
                                <div className = "simple-discount-li-div">
                                    <p className= "discName">ID: {elem.serialNumber}  </p>
                                    <p className= "discName">creator: {elem.creator}  </p>
                                    <p className= "discName">percent: {formatFloat(elem.percent * 100)}%  </p>
                                    <p className= "discName">discountType: {elem.discountType} </p>
                                    <p className= "discName"> condition: {elem.conditionType || "None"}  </p>
                                    <ConditionalRender
                                        condition={elem.discountType === 'Category'}
                                        render={() => (<p className= "discName"> category: {elem.category} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.discountType === 'Product'}
                                        render={() => {
                                            let product = this.props.storeProductsMap[elem.productId];
                                            return (<p className="discName">product: {(product && product.name) || "<deleted>"} </p>);
                                        }}
                                    />
                                    <ConditionalRender
                                        condition={elem.conditionType != null && elem.conditionType !== 'Time' && elem.minValue != null}
                                        render={() => (<p className="discName">min value: {elem.conditionType === 'Quantity' ? elem.minValue : formatFloat(elem.minValue)} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.conditionType != null && elem.conditionType !== 'Time' && elem.maxValue != null}
                                        render={() => (<p className="discName">max value: {elem.conditionType === 'Quantity' ? elem.maxValue : formatFloat(elem.maxValue)} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.conditionType === 'Time' && elem.startDate != null}
                                        render={() => (<p className= "discName">start date: {formatDate(elem.startDate)} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.conditionType === 'Time' && elem.endDate != null}
                                        render={() => (<p className= "discName">end date: {formatDate(elem.endDate)} </p>)}
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

export default SimpleDiscount;