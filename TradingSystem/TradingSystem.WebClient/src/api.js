import axios from "axios";

export const post = async (...args) => {
    let response = await axios.post.apply(axios, args);
    return response.data;
}

const getDataForRequest = (fData, params) => {
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
        throw new Error('No data specified to send');
    }
    else {
        throw new Error('Must specify params selector function if more than 1 params have been passed');
    }

    return data;
}

export const getCurry = (url, fParams, fConfig) => async (...params) => {
    let config = fConfig ? fConfig.apply(this, params) : {};
    let data = getDataForRequest(fParams, params);

    if (config.params != null && typeof config.params === "object") {
        data = Object.assign(data, config.params);
    }
    config.params = data;

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
        approved: 1,
        declined: 2,
        customerNegotiate: 3,
        ownerNegotiate: 4,
    },
};

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
        addCompound: postCurry('/stores/rules/AddCompoundDiscount'),
    },

    policies: {
        fetchAll: postCurry('/stores/rules/Policies', storeId => ({
            id: storeId,
        })),
        add: postCurry('/stores/rules/AddPolicy'),
    },

    bids: {
        mine: postCurry('/stores/bids/Mine', username => ({
            username: username,
        })),
        ofStore: postCurry('/stores/bids/OfStore', (username, storeId) => ({
            username: username,
            storeId: storeId,
        })),
    },
};