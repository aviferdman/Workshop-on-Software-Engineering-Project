import React from "react";
import {GlobalContext} from "../../../globalContext";
import * as api from '../../../api'
import {alertRequestError_default} from "../../../utils";
import FormFieldInfo from "../../../formsUtil/formFieldInfo";
import NumberFormField from "../../../formsUtil/NumberFormField";
import BidEditContent from "./BidEditContent";
import SimpleModal from "../../../components/SimpleModal";

export default class BidRecordStore extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            inCart: false,
            bidPrice: new NumberFormField(),
            action: new FormFieldInfo(),
            showDeclineModal: false,
            showApproveModal: false,
            showNegotiateModal: false,
        };
    }

    onPriceChange = e => {
        if (!this.state.bidPrice.trySetValueFromEvent(e)) {
            return;
        }

        this.setState({
            ...this.state
        });
    }

    onActionSelectionChange = e => {
        if (!this.state.action.trySetValueFromEvent(e)) {
            return;
        }

        this.setState({
            ...this.state
        });
    }

    applyAction = () => {
        switch (this.state.action.getValue()) {
            case '':
                break;

            case 'Approve':
                this.openApproveBidModal();
                break;

            case 'Negotiate':
                this.openNegotiateBidModal();
                break;

            case 'Decline':
                this.openDeclineBidModal();
                break;

            default:
                throw new Error('Invalid bid action');
        }
    }

    openApproveBidModal() {
        this.setState({
            showApproveModal: true,
        });
    }

    hideApproveBidModal = () => {
        this.setState({
            showApproveModal: false,
        });
    }

    openNegotiateBidModal() {
        this.setState({
            showNegotiateModal: true,
        });
    }

    hideNegotiateBidModal = () => {
        this.setState({
            showNegotiateModal: false,
        });
    }

    openDeclineBidModal() {
        this.setState({
            showDeclineModal: true,
        });
    }

    hideDeclineBidModal = () => {
        this.setState({
            showDeclineModal: false,
        });
    }

    approveBid = async () => {
        let bid = this.props.bid;
        let bidReqData = {
            username: this.context.username,
            storeId: this.props.storeId,
            bidId: bid.id,
        };
        await api.stores.bids.ownerAcceptBid(bidReqData)
            .then(() => {
                return api.stores.bids.ofStoreSpecific(bidReqData);
            }, alertRequestError_default)
            .then(updatedBid => {
                Object.assign(bid, updatedBid);
                bid.approvedByMe = true;
                this.hideApproveBidModal();
            }, e => {
                bid.approvedByMe = true;
                alertRequestError_default(e);
            });
    }

    negotiateBid = async () => {
        let bid = this.props.bid;
        let newPrice = this.state.bidPrice.getValue();
        await api.stores.bids.customerNegotiateBid({
            username: this.context.username,
            storeId: this.props.storeId,
            bidId: bid.id,
            newPrice: newPrice,
        }).then(() => {
            bid.approvedByMe = true;
            bid.status = api.data.bids.ownerNegotiate;
            bid.price = newPrice;
            this.hideNegotiateBidModal();
        }, alertRequestError_default);
    }

    declineBid = async () => {
        let bid = this.props.bid;
        await api.stores.bids.customerDenyBid({
            username: this.context.username,
            storeId: this.props.storeId,
            bidId: bid.id,
        }).then(() => {
            bid.status = api.data.bids.declined;
            this.hideDeclineBidModal();
        }, alertRequestError_default);
    }

    render() {
        let bid = this.props.bid;
        let product = bid.product;
        let isInNegotiation = (
            bid.status === api.data.bids.customerNegotiate ||
            bid.status === api.data.bids.ownerNegotiate
        );

        return (
            <div className="simple-bids-li-div">
                <p className="bidName">{<text style={{ fontWeight: "bold" }}>User: </text>} {bid.username}  </p>
                <p className="bidName"> {<text style={{ fontWeight: "bold" }}>Product: </text>} {(product && product.name) || "<deleted>"}  </p>
                <p className="bidName"> {<text style={{ fontWeight: "bold" }}>Current Offer:</text>} {bid.price}  </p>
                <p className="bidName">{<text style={{ fontWeight: "bold" }}>Status:</text>} {
                    bid.status === api.data.bids.approved ? (<label style={{ color: "green" }}>Approved</label>) :
                    bid.status === api.data.bids.ownerNegotiate ? (<label style={{ color: "orange" }}>Negotiation (waiting on customer)</label>) :
                    bid.status === api.data.bids.customerNegotiate && bid.approvedByMe ? (<label style={{ color: "orange" }}>Negotiation (waiting on staff)</label>) :
                    bid.status === api.data.bids.customerNegotiate ? (<label style={{ color: "orange" }}>Negotiation (waiting on you)</label>) :
                    bid.status === api.data.bids.declined ? (<label style={{ color: "red" }}>Declined</label>) :
                    null
                }
                </p>

                <p className="bidName" style={{ visibility: isInNegotiation ? 'visible' : 'hidden' }}>
                    <text style={{fontWeight: "bold"}}>Action: </text>
                    <select onChange={this.onActionSelectionChange}>
                        <option value=""/>

                        {/*if Approve is chosen a pop-up should appear to warn about Approving the offer permanently */}
                        {bid.status === api.data.bids.customerNegotiate && !bid.approvedByMe ? (<option value="Approve">Approve</option>) : null}
                        {/*if Negotiate is chosen a pop-up should appear to update the current price */}
                        <option value="Negotiate">Negotiate</option>
                        {/*if Decline is chosen a pop-up should appear to warn about declining the offer permanently */}
                        <option value="Decline">Decline</option>
                    </select>
                </p>

                <button className="button primary" style={{
                    margin: "2rem",
                    visibility: isInNegotiation ? 'visible' : 'hidden'
                }} onClick={this.applyAction}>Apply Action</button>

                <SimpleModal
                    title={`Decline bid`}
                    show={this.state.showDeclineModal}
                    width={'500px'}
                    height={'215px'}
                    btn1Text={'Close'}
                    btn1Handle={this.hideDeclineBidModal}
                    btn2Text={'Decline'}
                    btn2Handle={this.declineBid}
                    btn2Class={'danger'}
                >
                    <span style={{
                        marginLeft: '2rem',
                        fontSize: '2rem',
                    }}>Are you sure you would like to decline the bid?</span>
                </SimpleModal>

                <SimpleModal
                    title={'Approve bid'}
                    show={this.state.showApproveModal}
                    width={'500px'}
                    height={'215px'}
                    btn1Text={'Close'}
                    btn1Handle={this.hideApproveBidModal}
                    btn2Text={'Approve'}
                    btn2Handle={this.approveBid}
                >
                    <span style={{
                        marginLeft: '2rem',
                        fontSize: '2rem',
                    }}>Approve this bid?</span>
                </SimpleModal>

                <SimpleModal
                    title={'Negotiate bid'}
                    show={this.state.showNegotiateModal}
                    width={'500px'}
                    height={'300px'}
                    btn1Text={'Close'}
                    btn1Handle={this.hideNegotiateBidModal}
                    btn2Text={'Bid'}
                    btn2Handle={this.negotiateBid}>
                    <BidEditContent
                        value={this.state.bidPrice.getInputValue()}
                        onChange={this.onPriceChange}
                        lineGrid={false} />
                </SimpleModal>
            </div>
        );
    }
}

BidRecordStore.contextType = GlobalContext;
