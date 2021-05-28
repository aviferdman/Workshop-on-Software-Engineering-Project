import axios from "axios";

export const get = async (...args) => {
    let response = await axios.get.apply(axios, args);
    return response.data;
}

export const post = async (...args) => {
    let response = await axios.post.apply(axios, args);
    return response.data;
}

export const stores = (function () {
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

    return {
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
            updateManagerPermissions: appointRequest('UpdateManagerPermissions'),
            removeManager: appointRequest('RemoveManager'),
            removeOwner: appointRequest('RemoveOwner'),
        }
    };
})();
