var historyStack = [];

document.getElementById("backButton").addEventListener("click", function () {
    goBackWithoutDuplicates();
});

function goBackWithoutDuplicates() {
    // Проверяем, есть ли предыдущий уникальный URL в истории
    var previousUrl = getPreviousUniqueUrl();

    if (previousUrl) {
        // Если есть, переходим на него
        window.location.href = previousUrl;
    } else {
        // Если нет, просто перемещаемся назад
        window.history.back();
    }
}

function getPreviousUniqueUrl() {
    var currentUrl = window.location.href;
    var previousUrlIndex = historyStack.length - 2; // Индекс предыдущего URL в стеке

    // Ищем предыдущий уникальный URL в стеке
    while (previousUrlIndex >= 0) {
        if (historyStack[previousUrlIndex] !== currentUrl) {
            return historyStack[previousUrlIndex];
        }
        previousUrlIndex--;
    }

    return null; // Возвращаем null, если предыдущий уникальный URL не найден
}

// Добавляем обработчик события для отслеживания изменений в истории браузера
window.addEventListener("popstate", function (event) {
    // Добавляем новый URL в стек при каждом переходе
    historyStack.push(window.location.href);
});
