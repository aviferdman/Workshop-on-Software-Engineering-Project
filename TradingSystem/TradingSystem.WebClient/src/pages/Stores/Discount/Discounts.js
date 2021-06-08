import React from "react";
import './Discounts.css';
import {GlobalContext} from "../../../globalContext";
import Header from "../../../header";
import SimpleDiscount from "./SimpleDiscount";
import DataSimple from "../../../data/simpleDiscount.json"
import DataComplex from "../../../data/complexDiscount.json"
import AddSimpleDiscount from "./AddSimpleDiscount";
import AddComplexDiscount from "./AddCopmlexDiscount";
import ComplexDiscount from "./ComplexDiscount";
import * as api from "../../../api";
import {alertRequestError_default} from "../../../utils";


export class Discounts extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            simpleDiscounts: null,
            complexDiscounts:DataComplex.discounts,
        };
        this.storeId = this.props.match.params.storeId;
    }

    async componentDidMount() {
        await this.fetchDiscounts();
    }

    fetchDiscounts = async () => {
        await api.stores.discounts.fetchData(this.storeId)
            .then(discounts => {
                this.setState({
                    simpleDiscounts: discounts,
                });
            }, alertRequestError_default);
    }

    onSimpleDiscountAdd = discount => {
        this.state.simpleDiscounts.push(discount);
        this.setState({
            name: this.state.name,
        });
    }

    render() {
        return (
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
                            <AddSimpleDiscount storeId={this.storeId} onSuccess={this.onSimpleDiscountAdd} />
                        </div>

                        <div className="center-btn-nd">
                            <AddComplexDiscount />
                        </div>

                    </div>

                </div>



            </main>
        )
    }
}

Discounts.contextType = GlobalContext;
