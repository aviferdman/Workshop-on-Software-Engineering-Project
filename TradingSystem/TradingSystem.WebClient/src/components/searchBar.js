import React, {Component} from 'react';
import  './searchBar.css';

class SearchBar extends Component {
    render() {
        return (
            <div className="ui search">
                <div className="ui icon input">
                    <input
                        type="text"
                        placeholder="Search"
                        className="prompt"
                    />
                    <button className="searchButton Sprimary">Search</button>
                </div>

            </div>
        );
    }
}

export default SearchBar;

