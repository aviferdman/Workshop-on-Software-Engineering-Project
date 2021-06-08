import React, {Component} from 'react';
import './AdminHistory.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import StatisticsData from "../../data/statisticsData.json"

export class Statistics extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            statistics: StatisticsData.statistics
        };
    }


    render() {
        return (
            <div className="grid-container">
                <Header />

                <main>

                    <div >

                        <label >Start Date</label>
                        <input
                            type="date"
                            className="disc-input-props"
                            style={{width: "15rem" , height: "3rem" }}

                        />
                        <label style={{marginLeft: "5rem" }}>End Date</label>
                        <input
                            type="date"
                            className="disc-input-props"
                            style={{width: "15rem" , height: "3rem" }}

                        />

                    </div>

                    <p> {<text style={{fontWeight: "bold"}}>Guests: </text>} {this.state.statistics.guests}</p>
                    <p>  {<text style={{fontWeight: "bold"}}>Members: </text>} {this.state.statistics.pureMembers}</p>
                    <p> {<text style={{fontWeight: "bold"}}>Managers:  </text>} {this.state.statistics.pureManagers}</p>
                    <p>  {<text style={{fontWeight: "bold"}}>Owners: </text>} {this.state.statistics.owners}</p>
                    <p>  {<text style={{fontWeight: "bold"}}>Admins: </text>} {this.state.statistics.admins}</p>

                </main>

                <footer> End of Page</footer>
            </div>
        )
    }
}

Statistics.contextType = GlobalContext;
