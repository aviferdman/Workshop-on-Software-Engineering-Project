import React, {Component} from 'react';
import './searchBar.css';

class SearchBar extends Component {
    onKeyUp = e => {
        if (e.key === "Enter") {
            e.preventDefault();
            this.props.onSearch();
        }
    };

    render() {
        let disabled = this.props.enabled != null && !this.props.enabled ? { disabled: true, } : {}
        return (
            <div className="ui search">
                <div className="ui icon input">
                    <input
                        type="text"
                        placeholder="Search"
                        className="inputProps"
                        onChange={this.props.onChange}
                        onKeyUp={this.onKeyUp}
                        value={this.props.value}
                        {...disabled}
                    />
                    <button className="searchButton Sprimary" onClick={this.props.onSearch}>Search</button>
                </div>

            </div>
        );
    }
}

export default SearchBar;

