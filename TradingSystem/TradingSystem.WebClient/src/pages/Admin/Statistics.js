import React, {Component} from 'react';
import './AdminHistory.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import { Bar, Pie } from 'react-chartjs-2';
import * as api from '../../api';
import {alertRequestError_default} from "../../utils";
import DateFormField from "../../formsUtil/DateFormField";
import FormFields from "../../formsUtil/formFields";

export class Statistics extends Component {
    constructor(props) {
        super(props);
        this.state = {
            statistics: null,
            datesRange: new FormFields({
                start: new DateFormField(),
                end: new DateFormField(),
            }),
            graph: "",
        };
    }

    async componentDidMount() {
        this.context.webSocket.addEventListener('message', this.onNotificationReceived);
        await this.fetchStatistics();
    }

    componentWillUnmount() {
        this.context.webSocket.removeEventListener('message', this.onNotificationReceived);
    }

    onNotificationReceived = async e => {
        console.log('statistics page, web socket message from server');
        let notification = JSON.parse(e.data);
        if (notification.kind !== 'Statistics') {
            return;
        }

        let datesRange = this.state.datesRange.valuesObject();
        if (datesRange.start == null && datesRange.end == null) {
            await this.fetchStatistics();
        }
    }

    fetchStatistics = async () => {
        await api.statistics.fetchVisitingStatisticsForToday()
            .then(statistics => {
                this.setState({
                    statistics: statistics,
                });
            }, alertRequestError_default);
    }

    fetchStatisticsDatesRange = async () => {
        let datesRange = this.state.datesRange.valuesObject();
        await api.statistics.fetchVisitingStatisticsForDatesRange(datesRange.start, datesRange.end)
            .then(statistics => {
                this.setState({
                    statistics: statistics,
                });
            }, alertRequestError_default);
    }

    changeGraphType = e => {
        this.setState({
            graph: e.target.value
        });
    };

    getInputValue = field => {
        return this.state.datesRange.getField(field).getInputValue();
    }

    onDateInputChange = field => async e => {
        if (!this.state.datesRange.getField(field).trySetValueFromEvent(e)) {
            return;
        }

        let datesRange = this.state.datesRange.valuesObject();
        if (datesRange.start != null) {
            await this.fetchStatisticsDatesRange();
        }
        if (datesRange.start == null && datesRange.end == null) {
            await this.fetchStatistics();
        }
    }

    render() {
        if (this.state.statistics == null) {
            return null;
        }
        let isZero = true;
        for (let statName of Object.keys(this.state.statistics)) {
            let stat = this.state.statistics[statName];
            if (stat > 0) {
                isZero = false;
                break;
            }
        }

        return (
            <div className="grid-container">
                <Header />

                <main>

                    <div>
                        <p className= "bidName"> {<text style={{fontWeight: "bold"}}>Graph Type: </text>}

                                <select value={this.state.graph} onChange={this.changeGraphType}>
                                    <option value="Pie">Pie</option>
                                    <option value="Bar">Bar</option>
                                </select>

                        </p>

                    </div>

                    <div className= "bidName">

                        <label >Start Date</label>
                        <input
                            type="date"
                            className="disc-input-props"
                            style={{width: "15rem" , height: "3rem" }}
                            onChange={this.onDateInputChange('start')}

                        />
                        <label style={{marginLeft: "5rem" }}>End Date</label>
                        <input
                            type="date"
                            className="disc-input-props"
                            style={{width: "15rem" , height: "3rem" }}
                            onChange={this.onDateInputChange('end')}

                        />

                    </div>


                    {isZero ? (
                        <h1 className='center-screen'>No visitors</h1>
                    ) : this.state.graph === "Bar" ? (
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
                    ) : (
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
                    )}



                </main>

                <footer> End of Page</footer>
            </div>
        )
    }
}

Statistics.contextType = GlobalContext;
