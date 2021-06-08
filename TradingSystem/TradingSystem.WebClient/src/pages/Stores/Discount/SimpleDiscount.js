import React, {Component} from 'react';
import './Discounts.css';


class SimpleDiscount extends Component {
    render() {
        if (this.props.simpleDiscountRecords == null) {
            return null;
        }

        return (
            <div>
                <ul className = "simple-discount-ul">
                    {this.props.simpleDiscountRecords.map((elem) => (
                        <li  key={elem.id}>
                            <div className = "simple-discount-li-div">
                                <p className= "discName">ID: {elem.id}  </p>
                                <p className= "discName">creator: {elem.creator}  </p>
                                <p className= "discName">percent: {elem.percent * 100}%  </p>
                                <p className= "discName">discountType: {elem.discountType} </p>
                                <p className= "discName"> condition: {elem.conditionType || "None"}  </p>
                                <p className= "discName"> category: {elem.category} </p>
                                <p className= "discName">product: {elem.product} </p>
                                {elem.minValue == null ? null : (<p className= "discName">min value: {elem.minValue} </p>)}
                                {elem.maxValue == null ? null : (<p className= "discName">max value: {elem.maxValue} </p>)}
                                {elem.startDate == null ? null : (<p className= "discName">start date: {elem.startDate} </p>)}
                                {elem.endDate == null ? null : (<p className= "discName">end date: {elem.endDate} </p>)}
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default SimpleDiscount;