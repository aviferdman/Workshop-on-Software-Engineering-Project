import React from "react";

export default class StoreRestrictedComponentBase extends React.Component {
    isAllowed = (actions, allowedActions) => {
        for (const action of allowedActions) {
            if (action in actions) {
                return true;
            }
        }

        return false;
    }

    renderPermitted(children, rest) {
        return null;
    }

    render() {
        let { permissions, allowedActions, children, ...rest } = this.props;
        if (permissions.role === 'guest') {
            return null;
        }
        else if (permissions.role === 'manager') {
            if (!this.isAllowed(permissions.actions, allowedActions)) {
                return null;
            }
        }
        else if (permissions.role !== 'owner' && permissions.role !== 'founder') {
            console.error('store restricted button - invalid role:', permissions.role);
            return null;
        }

        return this.renderPermitted(children, {...rest});
    }
}