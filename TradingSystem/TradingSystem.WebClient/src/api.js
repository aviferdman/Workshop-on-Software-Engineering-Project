import axios from "axios";

export const storeInfo = async storeId => {
    let response = await axios.get('/Stores/Info', {
        params: {
            storeId: storeId
        }
    });
    return response.data;
}