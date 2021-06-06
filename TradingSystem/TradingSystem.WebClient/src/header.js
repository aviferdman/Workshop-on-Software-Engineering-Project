import {GlobalContext, UserRole} from "./globalContext";
import {Link} from "react-router-dom";
import * as HiIcons from "react-icons/hi";
import * as BiIcons from "react-icons/bi";
import Navbar from "./components/Navbar/Navbar";
import React from "react";

export default class Header extends React.Component {
    render() {
        return (
            <header className="header-container">
                <Link to={{ pathname: "/" }}>E - commerce Application</Link>
                <div>
                    <h3>{this.context.role && this.context.role !== UserRole.guest ? this.context.username : ''}</h3>
                </div>
                <span>{this.context.role === UserRole.admin ? 'Admin' : ''}</span>
                {!this.context.role || this.context.role === UserRole.guest ? <span /> : (<button className="icons" onClick={this.context.logout}><BiIcons.BiLogInCircle /> </button>)}
                <Link
                    className="icons"
                    to={{
                        pathname: "/ShoppingCart"
                    }}
                >
                    <HiIcons.HiShoppingCart />
                </Link>
                <Navbar/>
            </header>
        )
    }
}

Header.contextType = GlobalContext;
