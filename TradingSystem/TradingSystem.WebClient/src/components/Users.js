import React, {Component} from 'react';
import * as AiIcons from "react-icons/ai";

class Users extends Component {
    render() {
        return (
            <div>
                <ul className = "products">
                    {this.props.staff.map((user) => (
                        <li key={user.id}>
                            <div className="control-buttons-staff">
                                <div>
                                    <button className="exit-button">
                                        <AiIcons.AiOutlineClose />
                                    </button>
                                </div>

                            </div>
                            <div className = "user">
                                <a href={"#" + user.id}>
                                    <p className= "userName">{user.username}</p>
                                </a>
                                <div>
                                    <p> {user.address} </p>
                                    <p> {user.phone} </p>
                                </div>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default Users;