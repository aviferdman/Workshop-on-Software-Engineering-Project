import React, {Component} from 'react';
import './Discounts.css';
import ConditionalRender from "../../../ConditionalRender";


class ComplexDiscount extends Component {
    render() {
        if (this.props.discountRecords == null) {
            return null;
        }

        return (
            <div>
                <ul className = "simple-discount-ul">
                    {this.props.discountRecords.map((elem) => (
                        <li  key={elem.id}>
                            <div className = "simple-discount-li-div">
                                <p className= "discName">ID: {elem.serialNumber}  </p>
                                <p className= "discName">creator: {elem.creator}  </p>
                                <p className= "discName">discountRelation: {elem.discountRuleRelation} </p>
                                <p className= "discName"> First Discount: {elem.discount1_serialNumber}  </p>
                                <p className= "discName"> Second Discount: {elem.discount2_serialNumber} </p>

                                <ConditionalRender
                                    condition={elem.discountRuleRelation === 'Xor'}
                                    render={() => (<p className= "discName">Decide: {elem.decide ? 'First' : 'Second'} </p>)}
                                />

                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default ComplexDiscount;