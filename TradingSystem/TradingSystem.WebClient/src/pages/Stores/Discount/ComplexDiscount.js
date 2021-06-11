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
                            <div className = "simple-discount-li-div" style={{height: "30rem"}}>
                                <p className= "discName">{<text style={{fontWeight: "bold"}}>ID: </text>} {elem.serialNumber}  </p>
                                <p className= "discName"> {<text style={{fontWeight: "bold"}}>Creator: </text>} {elem.creator}  </p>
                                <p className= "discName">{<text style={{fontWeight: "bold"}}>Discount Relation:  </text>} {elem.discountRuleRelation} </p>
                                <p className= "discName">  {<text style={{fontWeight: "bold"}}>First Discount: </text>} {elem.discount1_serialNumber}  </p>
                                <p className= "discName">  {<text style={{fontWeight: "bold"}}>Second Discount: </text>} {elem.discount2_serialNumber} </p>

                                <ConditionalRender
                                    condition={elem.discountRuleRelation === 'Xor'}
                                    render={() => (<p className= "discName"> {<text style={{fontWeight: "bold"}}>Decide: </text>} {elem.decide ? 'First' : 'Second'} </p>)}
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