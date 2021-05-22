export function alertRequestError_default(e) {
    let msg = (e.response && e.response.data) || e.message;
    if (msg) {
        msg = ': ' + msg;
    }
    alert('An error occurred' + msg);
    console.error("search error occurred: ", e);
}
