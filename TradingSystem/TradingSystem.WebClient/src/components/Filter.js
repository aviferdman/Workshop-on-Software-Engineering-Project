import React, {Component} from 'react';

class Filter extends Component {
    render() {
        return (
            <div className="filter">
                <div className="filter-result">{this.props.count} Products </div>
                <div className="filter-sort">
                Order{" "}
                 <select value={this.props.sort} onChange={this.props.sortProducts}>
                 <option> Latest </option>
                 <option value="lowest">Lowest</option>
                 <option value="highest">Highest</option>


                </select>
                </div>
                <div className="filter-category">
                    Filter{" "}
                    <select value={this.props.category} onChange={this.props.filterProducts}>
                        <option value="">ALL</option>
                        {this.props.categories == null ? null : this.props.categories.map(category => {
                            return (<option value={category} key={category}>{category}</option>);
                        })}
                    </select>
                </div>



            </div>
        );
    }
}

export default Filter;