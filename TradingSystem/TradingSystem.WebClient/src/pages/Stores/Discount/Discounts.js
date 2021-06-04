import React from "react";
import './Discounts.css';
import {GlobalContext} from "../../../globalContext";
import Header from "../../../header";
import History from "../../../components/History";
import SimpleDiscount from "./SimpleDiscount";
import DataSimple from "../../../data/simpleDiscount.json"
import DataComplex from "../../../data/complexDiscount.json"
import AddSimpleDiscount from "./AddSimpleDiscount";
import AddComplexDiscount from "./AddCopmlexDiscount";
import ComplexDiscount from "./ComplexDiscount";


export class Discounts extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            simpleDiscounts: DataSimple.discounts,
            complexDiscounts:DataComplex.discounts
        };
    }

    render() {
        return (
            <div className="grid-container">
                <Header />

                <main>

                    <div className="discounts-grid">

                        {/*top grid - simple discounts*/}
                        <div  className="discounts-grid-simple">
                            <h2> Simple Discounts </h2>
                            <SimpleDiscount simpleDiscountRecords={this.state.simpleDiscounts}  />
                        </div>

                        {/*middle grid - complex discounts*/}
                        <div  className="discounts-grid-complex">
                            <h2> Complex Discounts </h2>
                            <ComplexDiscount simpleDiscountRecords={this.state.complexDiscounts}  />
                        </div>

                        {/*bottom grid - buttons*/}
                        <div className="discounts-grid-button">
                            <div className="center-btn-st">
                                <AddSimpleDiscount  />
                            </div>

                            <div className="center-btn-nd">
                                <AddComplexDiscount />
                            </div>

                        </div>

                    </div>



                </main>

                <footer> End of Stores</footer>
            </div>
        )
    }
}

Discounts.contextType = GlobalContext;
