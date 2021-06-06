import React, {Component} from 'react';
import './Discounts.css';


class SimpleDiscount extends Component {


    render() {
        return (
            <div>
                <ul className = "simple-discount-ul">
                    {this.props.simpleDiscountRecords.map((elem) => (
                        <li  key={elem.id}>
                            <div className = "simple-discount-li-div">
                                <p className= "discName">ID: {elem.id}  </p>
                                <p className= "discName">creator: {elem.creator}  </p>
                                <p className= "discName">discountType: {elem.discountType} </p>
                                <p className= "discName"> condition: {elem.condition}  </p>
                                <p className= "discName"> category: {elem.category} </p>
                                <p className= "discName">product: {elem.product} </p>
                                <p className= "discName">min value: {elem.minVal} </p>
                                <p className= "discName"> max value: {elem.maxVal}  </p>
                                <p className= "discName">initialDate: {elem.initialDate} </p>
                                <p className= "discName">endDate: {elem.endDate} </p>

                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default SimpleDiscount;