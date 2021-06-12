import React, {Component} from 'react';
import './AdminHistory.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import StatisticsData from "../../data/statisticsData.json"
import { Bar, Pie } from 'react-chartjs-2'

export class Statistics extends Component {
    constructor(props) {
        super(props);
        this.state = {
            statistics: StatisticsData.statistics,
            graph: ""
        };
    }

    componentDidMount() {
        this.context.webSocket.addEventListener('message', this.onNotificationReceived);
    }

    onNotificationReceived = e => {
        console.log('statistics page, web socket message from server', e.data);
        let notification = JSON.parse(e.data);
        if (!notification.kind || notification.kind === 'LiveNotification') {
            return;
        }

        console.log('Statistics notification', this.state);
    }

    changeGraphType = e => {
        this.setState({
            graph: e.target.value
        });
    };

    render() {
        return (
            <div className="grid-container">
                <Header />

                <main>

                    <div>
                        <p className= "bidName"> {<text style={{fontWeight: "bold"}}>Graph Type: </text>}

                                <select value={this.state.graph} onChange={this.changeGraphType}>
                                    <option value="Bar">Bar</option>
                                    <option value="Pie">Pie</option>
                                </select>

                        </p>

                    </div>

                    <div className= "bidName">

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


                    {this.state.graph === "Bar" ?
                    (
                        <div style={{marginTop: "20rem"}}>
                            <Bar
                                data = {{
                                    labels: ['Guests', 'Members', 'Managers', 'Owners', 'Admins',],
                                    datasets: [{
                                        label: '# of Users',
                                        data: [this.state.statistics.guests, this.state.statistics.pureMembers, this.state.statistics.pureManagers, this.state.statistics.owners, this.state.statistics.admins],
                                        backgroundColor: [
                                            'rgba(255, 99, 132, 0.2)',
                                            'rgba(54, 162, 235, 0.2)',
                                            'rgba(255, 206, 86, 0.2)',
                                            'rgba(75, 192, 192, 0.2)',
                                            'rgba(153, 102, 255, 0.2)',
                                        ],
                                        borderColor: [
                                            'rgba(255, 99, 132, 1)',
                                            'rgba(54, 162, 235, 1)',
                                            'rgba(255, 206, 86, 1)',
                                            'rgba(75, 192, 192, 1)',
                                            'rgba(153, 102, 255, 1)',
                                        ],
                                        borderWidth: 3
                                    }]
                                }}
                                height={200}
                                width={300}
                                options={{
                                    maintainAspectRatio: false,
                                }}
                            />
                        </div>
                    ):
                        (
                            <div style={{marginTop: "20rem"}} >
                                <Pie
                                    data = {{
                                        labels: ['Guests', 'Members', 'Managers', 'Owners', 'Admins',],
                                        datasets: [{
                                            label: '# of Users',
                                            data: [this.state.statistics.guests, this.state.statistics.pureMembers, this.state.statistics.pureManagers, this.state.statistics.owners, this.state.statistics.admins],
                                            backgroundColor: [
                                                'rgba(255, 99, 132, 0.2)',
                                                'rgba(54, 162, 235, 0.2)',
                                                'rgba(255, 206, 86, 0.2)',
                                                'rgba(75, 192, 192, 0.2)',
                                                'rgba(153, 102, 255, 0.2)',
                                            ],
                                            borderColor: [
                                                'rgba(255, 99, 132, 1)',
                                                'rgba(54, 162, 235, 1)',
                                                'rgba(255, 206, 86, 1)',
                                                'rgba(75, 192, 192, 1)',
                                                'rgba(153, 102, 255, 1)',
                                            ],
                                            borderWidth: 3
                                        }]
                                    }}
                                    height={200}
                                    width={300}
                                    options={{
                                        maintainAspectRatio: false,
                                    }}
                                />
                            </div>
                        )
                    }



                </main>

                <footer> End of Page</footer>
            </div>
        )
    }
}

Statistics.contextType = GlobalContext;
