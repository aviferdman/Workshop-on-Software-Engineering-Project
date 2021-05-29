import axios from "axios";

export const get = async (...args) => {
    let response = await axios.get.apply(axios, args);
    return response.data;
}

export const post = async (...args) => {
    let response = await axios.post.apply(axios, args);
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
        }
    }
};

export const history = {
    mine: async username => {
        await post('/History/Mine', {
            username: username,
        });
    },
    ofStore: async (username, storeId) => {
        await post('/History/OfStore', {
           username: username,
           storeId: storeId,
        });
    },
    all: async username => {
        await post('/History/OfAll', {
            username: username,
        });
    }
};

export const stores = {
    info: async storeId => {
        return get('/Stores/Info', {
            params: {
                storeId: storeId
            }
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
    }
};
