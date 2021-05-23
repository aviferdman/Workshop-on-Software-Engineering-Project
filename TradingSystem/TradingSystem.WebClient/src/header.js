import {GlobalContext, UserRole} from "./globalContext";
import {Link} from "react-router-dom";
import * as HiIcons from "react-icons/hi";
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
                {!this.context.role || this.context.role === UserRole.guest ? <span /> : (<button className='logout-btn' onClick={this.context.logout}>Log out</button>)}
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
