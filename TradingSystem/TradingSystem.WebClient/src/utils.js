export function alertRequestError_default(e) {
    let msg = (e.response && e.response.data && e.response.data.title) ||
        (e.response && typeof e.response.data === "string" && e.response.data) ||
        (e.response && e.response.title) ||
        e.message;
    if (msg) {
        msg = ': ' + msg;
    }
    alert('An error occurred' + msg);
}

export const arrayToHashset = array => {
    return Object.fromEntries(array.map(key => [key, {}]));
}
