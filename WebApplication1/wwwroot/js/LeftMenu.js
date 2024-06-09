var historyStack = [];

document.getElementById("backButton").addEventListener("click", function () {
    goBackWithoutDuplicates();
});

function goBackWithoutDuplicates() {
    // ���������, ���� �� ���������� ���������� URL � �������
    var previousUrl = getPreviousUniqueUrl();

    if (previousUrl) {
        // ���� ����, ��������� �� ����
        window.location.href = previousUrl;
    } else {
        // ���� ���, ������ ������������ �����
        window.history.back();
    }
}

function getPreviousUniqueUrl() {
    var currentUrl = window.location.href;
    var previousUrlIndex = historyStack.length - 2; // ������ ����������� URL � �����

    // ���� ���������� ���������� URL � �����
    while (previousUrlIndex >= 0) {
        if (historyStack[previousUrlIndex] !== currentUrl) {
            return historyStack[previousUrlIndex];
        }
        previousUrlIndex--;
    }

    return null; // ���������� null, ���� ���������� ���������� URL �� ������
}

// ��������� ���������� ������� ��� ������������ ��������� � ������� ��������
window.addEventListener("popstate", function (event) {
    // ��������� ����� URL � ���� ��� ������ ��������
    historyStack.push(window.location.href);
});
