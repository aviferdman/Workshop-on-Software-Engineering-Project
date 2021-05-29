import StoreRestrictedComponentBase from "./StoreRestrictedComponentBase";

export default class StoreRestrictedComponentCustom extends StoreRestrictedComponentBase {
    renderPermitted(children, rest) {
        return this.props.render({
           children,
           ...rest
        });
    }
}