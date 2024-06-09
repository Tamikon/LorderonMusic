document.addEventListener('DOMContentLoaded', () => {
    const overlay = document.getElementById('overlay');
    const welcomeMessage = document.querySelector('.welcome-message');
    
    setTimeout(() => {
        overlay.classList.add('darkened');
    }, 250);
    
    setTimeout(() => {
        welcomeMessage.style.opacity = 1;
    }, 1000);
});

const video = document.getElementById('bgvideo');
video.addEventListener('ended', () => {
    video.classList.add('fade-out');
    setTimeout(() => {
        video.currentTime = 0;
        video.play();
        video.classList.remove('fade-out');
    }, ); 
});