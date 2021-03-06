import React, {Component} from 'react';
import './Discounts.css';
import {alertRequestError_default, formatDate, formatFloat} from "../../../utils";
import ConditionalRender from "../../../ConditionalRender";
import EditSimpleDiscount from "./EditSimpleDiscount";
import * as AiIcons from "react-icons/ai";
import * as api from "../../../api";
import {GlobalContext} from "../../../globalContext";

class SimpleDiscount extends Component {
    onRemoveClick = discount => async e => {
        await api.stores.discounts.remove({
            username: this.context.username,
            storeId: this.props.storeId,
            discountId: discount.id,
        }).then(id => {
            this.props.onRemove(discount);
        }, alertRequestError_default);
    }

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
                                <div className = "simple-discount-li-div" style={{height: "50rem"}}>

                                        <div className="control-buttons" style={{marginLeft:"1rem" , marginTop:"1rem"}}>
                                            <div >
                                                <button className="exit-button" onClick={this.onRemoveClick(elem)} >
                                                    <AiIcons.AiOutlineClose />
                                                </button>
                                            </div>

                                            <div style={{marginLeft:"2.5rem"}}>
                                                <EditSimpleDiscount
                                                    discount={elem}
                                                    storeId={this.props.storeId}
                                                    storeProducts={this.props.storeProducts}
                                                    onSuccess={this.props.onEdit} />
                                            </div>
                                        </div>


                                    <p className= "discName"> {<text style={{fontWeight: "bold"}}>ID: </text>} {elem.serialNumber}  </p>
                                    <p className= "discName"> {<text style={{fontWeight: "bold"}}>creator: </text>} {elem.creator}  </p>
                                    <p className= "discName"> {<text style={{fontWeight: "bold"}}>percent:  </text>} {formatFloat(elem.percent * 100)}%  </p>
                                    <p className= "discName"> {<text style={{fontWeight: "bold"}}>condition: </text>} {elem.conditionType || "None"}  </p>
                                    <ConditionalRender
                                        condition={elem.conditionType === 'Quantity'}
                                        render={() => [(
                                            <p className= "discName"> {<text style={{fontWeight: "bold"}}>discountType: </text>} {elem.discountType} </p>), (
                                            <ConditionalRender
                                                condition={elem.discountType === 'Category'}
                                                render={() => (<p className= "discName"> {<text style={{fontWeight: "bold"}}>category:  </text>} {elem.category} </p>)}
                                            />), (
                                            <ConditionalRender
                                                condition={elem.discountType === 'Product'}
                                                render={() => {
                                                    let product = this.props.storeProductsMap && this.props.storeProductsMap[elem.productId];
                                                    return (<p className="discName"> {<text style={{fontWeight: "bold"}}>product: </text>} {(product && product.name) || "<deleted>"} </p>);
                                                }}
                                            />)
                                        ]}
                                    />
                                    <ConditionalRender
                                        condition={elem.conditionType != null && elem.conditionType !== 'Time' && elem.minValue != null}
                                        render={() => (<p className="discName">{<text style={{fontWeight: "bold"}}>min value: </text>} {elem.conditionType === 'Quantity' ? elem.minValue : formatFloat(elem.minValue)} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.conditionType != null && elem.conditionType !== 'Time' && elem.maxValue != null}
                                        render={() => (<p className="discName">{<text style={{fontWeight: "bold"}}>max value: </text>} {elem.conditionType === 'Quantity' ? elem.maxValue : formatFloat(elem.maxValue)} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.conditionType === 'Time' && elem.startDate != null}
                                        render={() => (<p className= "discName">{<text style={{fontWeight: "bold"}}>start date: </text>} {formatDate(elem.startDate)} </p>)}
                                    />
                                    <ConditionalRender
                                        condition={elem.conditionType === 'Time' && elem.endDate != null}
                                        render={() => (<p className= "discName"> {<text style={{fontWeight: "bold"}}>end date:</text>} {formatDate(elem.endDate)} </p>)}
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

SimpleDiscount.contextType = GlobalContext;
export default SimpleDiscount;