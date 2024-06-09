var historyStack = [];

document.getElementById("backButton").addEventListener("click", function () {
    goBackWithoutDuplicates();
});

function goBackWithoutDuplicates() {
    var previousUrl = getPreviousUniqueUrl();

    if (previousUrl) {
        window.location.href = previousUrl;
    } else {
        window.history.back();
    }
}

function getPreviousUniqueUrl() {
    var currentUrl = window.location.href;
    var previousUrlIndex = historyStack.length - 2; 
    
    while (previousUrlIndex >= 0) {
        if (historyStack[previousUrlIndex] !== currentUrl) {
            return historyStack[previousUrlIndex];
        }
        previousUrlIndex--;
    }

    return null; 
}

window.addEventListener("popstate", function (event) {
    historyStack.push(window.location.href);
});
