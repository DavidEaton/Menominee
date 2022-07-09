export function elementWidthByClass(selector) {
    var element = document.querySelector(selector);

    if (element) {
        return element.offsetWidth;
    }
    else {
        return 0;
    }
}

export function elementWidthById(id) {
    var element = document.getElementById(id);

    if (element) {
        return element.offsetWidth;
    }
    else {
        return 0;
    }
}
