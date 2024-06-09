function showAddPlaylistForm() {
    document.getElementById('formOverlay').style.display = 'flex';
}

function hideAddPlaylistForm() {
    document.getElementById('formOverlay').style.display = 'none';
}
function showEditPlaylistForm(playlistId, playlistName) {
    document.getElementById('editPlaylistForm-' + playlistId).style.display = 'block';
}

function hideEditPlaylistForm(playlistId) {
    document.getElementById('editPlaylistForm-' + playlistId).style.display = 'none';
}
