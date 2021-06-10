import React from 'react'

export default class ConditionalRender extends React.Component {
    render() {
        if (this.props.condition) {
            return this.props.render();
        }
        else if (this.props.renderElse) {
            this.props.renderElse();
        }
        else {
            return null;
        }
    }
}