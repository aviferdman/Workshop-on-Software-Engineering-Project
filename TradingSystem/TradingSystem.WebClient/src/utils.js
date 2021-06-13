export function getDefaultErrorMessage(e) {
    let msg = (e.response && e.response.data && e.response.data.title) ||
        (e.response && typeof e.response.data === "string" && e.response.data) ||
        (e.response && e.response.title) ||
        e.message;
    return msg;
}

export function alertRequestError_default(e) {
    let msg = getDefaultErrorMessage(e);
    if (msg) {
        msg = ': ' + msg;
    }
    alert('An error occurred' + msg);
}

export const arrayToHashset = array => {
    return Object.fromEntries(array.map(key => [key, {}]));
}

export const arrayToMap = (array, fKey) => {
    return Object.fromEntries(array.map(item => [fKey(item), item]));
}

export function formatFloat(float) {
    return float.toFixed(1).toLocaleString();
}

function formatNum_leadingZero(n) {
    if (n < 10) {
        return '0' + n;
    }
    return '' + n;
}

export function formatDate(date) {
    let dayInMonth = formatNum_leadingZero(date.getDate());
    let month = formatNum_leadingZero(date.getMonth() + 1);
    let year = date.getFullYear();
    return `${dayInMonth}/${month}/${year}`;
}

export function formatDateForInput(date) {
    if (date == null) {
        return '';
    }
    let dayInMonth = formatNum_leadingZero(date.getDate());
    let month = formatNum_leadingZero(date.getMonth() + 1);
    let year = date.getFullYear();
    return `${year}-${month}-${dayInMonth}`;
}
