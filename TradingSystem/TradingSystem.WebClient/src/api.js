import axios from "axios";

export const post = async (...args) => {
    let response = await axios.post.apply(axios, args);
    return response.data;
}

const getDataForRequest = (fData, params, noParamsDefault) => {
    let data;
    if (fData != null) {
        if (params.length === 0) {
            data = fData;
        }
        else {
            data = fData.apply(this, params);
        }
    }
    else if (params.length === 1) {
        data = params[0];
    }
    else if (params.length === 0) {
        if (noParamsDefault === undefined) {
            throw new Error('No data specified to send');
        }

        return noParamsDefault;
    }
    else {
        throw new Error('Must specify params selector function if more than 1 params have been passed');
    }

    return data;
}

export const getCurry = (url, fParams, fConfig) => async (...params) => {
    let config = fConfig ? fConfig.apply(this, params) : {};
    let data = getDataForRequest(fParams, params, null);

    if (config.params != null && typeof config.params === "object") {
        data = data == null ? config.params : Object.assign(data, config.params);
    }
    if (data != null) {
        config.params = data;
    }

    let response = await axios.get(url, config);
    return response.data;
}

export const postCurry = (url, fData, fConfig) => async (...params) => {
    let config = fConfig ? fConfig.apply(this, params) : undefined;
    let data = getDataForRequest(fData, params);
    let response = await axios.post(url, data, config);
    return response.data;
}

const permissionInfoRequest = action => async (username, storeId) => {
    return post(`/stores/permissions/${action}`, {
        username: username,
        storeId: storeId,
    });
};
const appointRequest = action => async (username, storeId, workerUsername) => {
    return post(`/stores/permissions/${action}`, {
        storeId: storeId,
        assigner: username,
        assignee: workerUsername,
    });
};

export const data = {
    stores: {
        permissions: {
            addProduct: 'AddProduct',
            appointManger: 'AppointManger',
            removeProduct: 'RemoveProduct',
            getPersonnelInfo: 'GetPersonnelInfo',
            editProduct: 'EditProduct',
            getShopHistory: 'GetShopHistory',
            editPermissions: 'EditPermissions',
            closeShop: 'CloseShop',
            editDiscount: 'EditDiscount',
            editPolicy: 'EditPolicy',
            bidRequests: 'BidRequests',
        },
    },
    bids: {
        approved: 0,
        declined: 1,
        customerNegotiate: 2,
        ownerNegotiate: 3,
    },
};

export const categories = {
    fetchAll: getCurry('/Products/Categories'),
};

export const shoppingCart = {
    addProduct: postCurry('/ShoppingCart/AddProduct'),
};

export const statistics = {
    fetchVisitingStatisticsForToday: getCurry('/Statistics/FetchVisitingStatisticsForToday'),
    fetchVisitingStatisticsForDatesRange: getCurry('/Statistics/FetchVisitingStatisticsForDatesRange', (start, end) => ({
        dateStart: start,
        dateEnd: end,
    })),
}

export const history = {
    mine: async username => {
        return await post('/History/Mine', {
            username: username,
        });
    },
    mineSpecific: async (username, paymentId) => {
        return await post('/History/MineSpecific', {
            username: username,
            key: paymentId,
        });
    },
    ofStore: async (username, storeId) => {
        return await post('/History/OfStore', {
           username: username,
           storeId: storeId,
        });
    },
    ofStoreSpecific: async (username, storeId, paymentId) => {
        return await post('/History/OfStoreSpecific', {
            username: username,
            storeId: storeId,
            paymentId: paymentId,
        });
    },
    all: async username => {
        return await post('/History/OfAll', {
            username: username,
        });
    },
    allSpecific: async (username, paymentId) => {
        return await post('/History/OfAllSpecific', {
            username: username,
            key: paymentId,
        });
    },
};

export const stores = {
    info: getCurry('/Stores/Info', storeId => ({
        storeId: storeId,
    })),
    infoWithProducts: getCurry('/Stores/InfoWithProducts', storeId => ({
        storeId: storeId,
    })),

    search: async storeName => {
        return post('/Stores/Search', {
            storeName: storeName,
        });
    },

    permissions: {
        mine: permissionInfoRequest('MyPermissions'),
        workersDetails: permissionInfoRequest('WorkersDetails'),
        workerSpecificDetails: appointRequest('WorkerSpecificDetails'),
        appointOwner: appointRequest('AppointOwner'),
        appointManager: appointRequest('AppointManager'),
        removeManager: appointRequest('RemoveManager'),
        removeOwner: appointRequest('RemoveOwner'),
        updateManagerPermissions: async (username, storeId, workerUsername, permissions) => {
            return post('/stores/permissions/UpdateManagerPermissions', {
                appointment: {
                    storeId: storeId,
                    assigner: username,
                    assignee: workerUsername,
                },
                permissions: permissions
            });
        },
    },

    discounts: {
        fetchData: postCurry('/stores/rules/Discounts', storeId => ({
            id: storeId,
        })),
        add: postCurry('/stores/rules/AddDiscount'),
        edit: postCurry('/stores/rules/EditDiscount'),
        remove: postCurry('/stores/rules/RemoveDiscount'),
        addCompound: postCurry('/stores/rules/AddCompoundDiscount'),
    },

    policies: {
        fetchAll: postCurry('/stores/rules/Policies', storeId => ({
            id: storeId,
        })),
        add: postCurry('/stores/rules/AddPolicy'),
        removeAll: postCurry('/stores/rules/RemovePolicies'),
    },

    bids: {
        mine: postCurry('/stores/bids/Mine', username => ({
            username: username,
        })),
        ofStore: postCurry('/stores/bids/OfStore', (username, storeId) => ({
            username: username,
            storeId: storeId,
        })),
        ofStoreSpecific: postCurry('/stores/bids/OfStoreSpecific'),
        changeBidPolicy: postCurry('/stores/bids/ChangeBidPolicy'),
        getBidPolicy: postCurry('/stores/bids/GetBidPolicy'),
        createCustomerBid: postCurry('/stores/bids/CreateCustomerBid'),

        customerAcceptBid: postCurry('/stores/bids/CustomerAcceptBid'),
        customerNegotiateBid: postCurry('/stores/bids/CustomerNegotiateBid'),
        customerDenyBid: postCurry('/stores/bids/CustomerDenyBid'),
        ownerAcceptBid: postCurry('/stores/bids/OwnerAcceptBid'),
        ownerNegotiateBid: postCurry('/stores/bids/OwnerNegotiateBid'),
        ownerDenyBid: postCurry('/stores/bids/OwnerDenyBid'),
    },
};