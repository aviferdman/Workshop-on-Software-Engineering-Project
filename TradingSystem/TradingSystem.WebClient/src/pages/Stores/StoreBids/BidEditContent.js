import React from "react";

export default class BidEditContent extends React.Component {
    render() {
        return (
            <div className={this.props.lineGrid ? 'disc-comp-check-line-grid' : ''}>

                <div className="center-item">
                    <div className= "disc-col-grd-perm">
                        <div className="disc-text-props">
                            <label>Bid Price</label>
                        </div>

                        <div >
                            <input
                                type="number"
                                step="0.01"
                                className="disc-input-props"
                                required
                                value={this.props.value}
                                onChange={this.props.onChange}
                            />
                        </div>
                    </div>
                </div>

            </div>
        );
    }
}