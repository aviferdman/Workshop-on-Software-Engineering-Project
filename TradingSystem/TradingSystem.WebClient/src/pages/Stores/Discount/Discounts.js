import React from "react";
import './Discounts.css';
import {GlobalContext} from "../../../globalContext";
import SimpleDiscount from "./SimpleDiscount";
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
            complexDiscounts:null,
            simpleDiscountsSerialNumberMap: null,
            simpleDiscountsIdMap: null,
            nextSerialNumber: null,
        };
        this.storeId = this.props.match.params.storeId;
    }

    async componentDidMount() {
        await this.fetchDiscounts();
    }

    fetchDiscounts = async () => {
        await api.stores.discounts.fetchData(this.storeId)
            .then(discounts => {
                let maps = this.buildMaps(discounts.leafDiscounts);
                this.setSerialNumberOnCompoundDiscounts(discounts.relationDiscounts, maps.simpleDiscountsIdMap);
                this.setState({
                    simpleDiscounts: discounts.leafDiscounts,
                    complexDiscounts: discounts.relationDiscounts,
                    simpleDiscountsSerialNumberMap: maps.simpleDiscountsSerialNumberMap,
                    simpleDiscountsIdMap: maps.simpleDiscountsIdMap,
                    nextSerialNumber: discounts.leafDiscounts.length + 1,
                });
            }, alertRequestError_default);
    }

    buildMaps(simpleDiscounts) {
        let simpleDiscountsSerialNumberMap = {};
        let simpleDiscountsIdMap = {};
        for (let simpleDiscount of simpleDiscounts) {
            simpleDiscountsSerialNumberMap[simpleDiscount.serialNumber] = simpleDiscount
            simpleDiscountsIdMap[simpleDiscount.id] = simpleDiscount;
        }
        return {
            simpleDiscountsSerialNumberMap: simpleDiscountsSerialNumberMap,
            simpleDiscountsIdMap: simpleDiscountsIdMap,
        };
    }

    setSerialNumberOnCompoundDiscounts(compoundDiscounts, simpleDiscountsIdMap) {
        compoundDiscounts.forEach(compoundDiscount => {
            compoundDiscount.discount1_serialNumber = simpleDiscountsIdMap[compoundDiscount.discountId1].serialNumber;
            compoundDiscount.discount2_serialNumber = simpleDiscountsIdMap[compoundDiscount.discountId2].serialNumber;
        });
    }

    onSimpleDiscountAdd = discount => {
        let serialNumber = this.state.nextSerialNumber;
        discount.serialNumber = serialNumber;
        this.state.simpleDiscountsSerialNumberMap[serialNumber] = discount;
        this.state.simpleDiscountsIdMap[discount.id] = discount;
        this.state.simpleDiscounts.push(discount);
        this.setState({
            nextSerialNumber: serialNumber + 1,
        });
    }

    onCompoundDiscountAdd = discount => {
        this.state.complexDiscounts.push(discount);
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
                        <ComplexDiscount discountRecords={this.state.complexDiscounts}
                                         simpleDiscountsSerialNumberMap={this.state.simpleDiscountsSerialNumberMap} />
                    </div>

                    {/*bottom grid - buttons*/}
                    <div className="discounts-grid-button">
                        <div className="center-btn-st">
                            <AddSimpleDiscount storeId={this.storeId} onSuccess={this.onSimpleDiscountAdd} />
                        </div>

                        <div className="center-btn-nd">
                            <AddComplexDiscount storeId={this.storeId} onSuccess={this.onCompoundDiscountAdd}
                                                simpleDiscountsSerialNumberMap={this.state.simpleDiscountsSerialNumberMap} />
                        </div>

                    </div>

                </div>



            </main>
        )
    }
}

Discounts.contextType = GlobalContext;
