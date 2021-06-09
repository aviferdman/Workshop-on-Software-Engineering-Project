import React from "react";
import './Discounts.css';
import {GlobalContext} from "../../../globalContext";
import SimpleDiscount from "./SimpleDiscount";
import AddSimpleDiscount from "./AddSimpleDiscount";
import AddComplexDiscount from "./AddCopmlexDiscount";
import ComplexDiscount from "./ComplexDiscount";
import * as api from "../../../api";
import {alertRequestError_default} from "../../../utils";
import * as util from "../../../utils";


export class Discounts extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            simpleDiscounts: null,
            complexDiscounts:null,
            simpleDiscountsSerialNumberMap: null,
            simpleDiscountsIdMap: null,
            nextSerialNumber: null,
            storeProducts: null,
            storeProductsMap: null,
        };
        this.storeId = this.props.match.params.storeId;
    }

    async componentDidMount() {
        let promise_storeProducts = this.fetchStoreProducts();
        let promise_discounts = this.fetchDiscounts();
        await Promise.all([
            promise_storeProducts,
            promise_discounts
        ]);
    }

    async fetchStoreProducts() {
        await api.stores.infoWithProducts(this.storeId)
            .then(storeInfo => {
                this.setState({
                    storeProducts: storeInfo.products,
                    storeProductsMap: util.arrayToMap(storeInfo.products, p => p.id),
                });
            }, alertRequestError_default);
    }

    fetchDiscounts = async () => {
        await api.stores.discounts.fetchData(this.storeId)
            .then(discounts => {
                let serialNumber = this.setSerialNumbersAll(discounts);
                let maps = this.buildMaps(discounts);
                this.setSerialNumberOnCompoundDiscounts(discounts.relationDiscounts, maps.simpleDiscountsIdMap);
                this.setState({
                    simpleDiscounts: discounts.leafDiscounts,
                    complexDiscounts: discounts.relationDiscounts,
                    simpleDiscountsSerialNumberMap: maps.simpleDiscountsSerialNumberMap,
                    simpleDiscountsIdMap: maps.simpleDiscountsIdMap,
                    nextSerialNumber: serialNumber,
                });
            }, alertRequestError_default);
    }

    setSerialNumbersAll(discounts) {
        let serialNumber = 1;
        serialNumber = this.setSerialNumbers(discounts.leafDiscounts, serialNumber);
        serialNumber = this.setSerialNumbers(discounts.relationDiscounts, serialNumber);
        return serialNumber;
    }

    setSerialNumbers(discounts, initialValue) {
        let nextSerialNumber = initialValue;
        discounts.forEach(discount => {
            discount.serialNumber = nextSerialNumber++;
        });
        return nextSerialNumber;
    }

    buildMaps(discounts) {
        let simpleDiscountsSerialNumberMap = {};
        let simpleDiscountsIdMap = {};
        this.updateMaps(discounts.leafDiscounts, simpleDiscountsSerialNumberMap, simpleDiscountsIdMap);
        this.updateMaps(discounts.relationDiscounts, simpleDiscountsSerialNumberMap, simpleDiscountsIdMap);
        return {
            simpleDiscountsSerialNumberMap: simpleDiscountsSerialNumberMap,
            simpleDiscountsIdMap: simpleDiscountsIdMap,
        };
    }

    updateMaps(discounts, simpleDiscountsSerialNumberMap, simpleDiscountsIdMap) {
        for (let discount of discounts) {
            simpleDiscountsSerialNumberMap[discount.serialNumber] = discount
            simpleDiscountsIdMap[discount.id] = discount;
        }
    }

    setSerialNumberOnCompoundDiscounts(compoundDiscounts, simpleDiscountsIdMap) {
        compoundDiscounts.forEach(compoundDiscount => {
            compoundDiscount.discount1_serialNumber = simpleDiscountsIdMap[compoundDiscount.discountId1].serialNumber;
            compoundDiscount.discount2_serialNumber = simpleDiscountsIdMap[compoundDiscount.discountId2].serialNumber;
        });
    }

    onSimpleDiscountAdd = discount => {
       this.onDiscountAddCore(this.state.simpleDiscounts, discount);
    }

    onCompoundDiscountAdd = discount => {
        this.onDiscountAddCore(this.state.complexDiscounts, discount);
    }

    onDiscountAddCore(discounts, discount) {
        let serialNumber = this.state.nextSerialNumber;
        discount.serialNumber = serialNumber;
        this.state.simpleDiscountsSerialNumberMap[serialNumber] = discount;
        this.state.simpleDiscountsIdMap[discount.id] = discount;
        discounts.push(discount);
        this.setState({
            nextSerialNumber: serialNumber + 1,
        });
    }

    render() {
        return (
            <main>

                <div className="discounts-grid">

                    {/*top grid - simple discounts*/}
                    <div  className="discounts-grid-simple">
                        <h2> Simple Discounts </h2>
                        <SimpleDiscount simpleDiscountRecords={this.state.simpleDiscounts} storeProductsMap={this.state.storeProductsMap} />
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
                            <AddSimpleDiscount storeId={this.storeId} onSuccess={this.onSimpleDiscountAdd}
                                               storeProducts={this.state.storeProducts} />
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
