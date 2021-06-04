import React, {Component} from 'react';
import './Discounts.css';


class ComplexDiscount extends Component {


    render() {
        return (
            <div>
                <ul className = "simple-discount-ul">
                    {this.props.simpleDiscountRecords.map((elem) => (
                        <li  key={elem.id}>
                            <div className = "simple-discount-li-div">

                                <p className= "discName">creator: {elem.creator}  </p>
                                <p className= "discName">discountRelation: {elem.discountRelation} </p>
                                <p className= "discName"> First Discount: {elem.condition}  </p>
                                <p className= "discName"> Second Discount: {elem.category} </p>
                                <p className= "discName">Decide: {elem.product} </p>

                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default ComplexDiscount;