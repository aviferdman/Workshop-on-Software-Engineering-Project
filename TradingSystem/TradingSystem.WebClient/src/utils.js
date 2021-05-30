export function alertRequestError_default(e) {
    let msg = (e.response && e.response.data) || e.message || (e.response && e.response.title);
    if (msg) {
        msg = ': ' + msg;
    }
    alert('An error occurred' + msg);
}

export const arrayToHashset = array => {
    return Object.fromEntries(array.map(key => [key, {}]));
}
