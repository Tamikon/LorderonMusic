//function showEditPlaylistForm(playlistId) {
//    document.getElementById('editPlaylistForm-' + playlistId).style.display = 'block';
//}

//function hideEditPlaylistForm(playlistId) {
//    document.getElementById('editPlaylistForm-' + playlistId).style.display = 'none';
//}


function showEditPlaylistForm(playlistId) {
    $.get('/Home/ServerDetails/GetEditPlaylistForm', { playlistId: playlistId }, function (data) {
        $('#editFormOverlay').html(data).show();
    });
}

function hideEditPlaylistForm() {
    document.getElementById('editFormOverlay').style.display = 'none';
}

function showAddPlaylistForm() {
    document.getElementById('formOverlay').style.display = 'block';
}

function hideAddPlaylistForm() {
    document.getElementById('formOverlay').style.display = 'none';
}
