function showAddPlaylistForm() {
    document.getElementById('formOverlay').style.display = 'flex';
}

function hideAddPlaylistForm() {
    document.getElementById('formOverlay').style.display = 'none';
}

function showEditPlaylistForm(playlistId) {
    $.get('/Home/ServerDetails/GetEditPlaylistForm', { playlistId: playlistId }, function (data) {
        $('#editFormOverlay').html(data).css('display', 'flex');
    });
}

function hideEditPlaylistForm() {
    document.getElementById('editFormOverlay').style.display = 'none';
}