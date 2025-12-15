// app.js (mejorado)
//window.__appbarInited = window.__appbarInited || false;
//window.__scrollObserver = null;

window.initAppBarScroll = () => {
    if (window.__appbarInited) return; // evitar listeners duplicados
    window.__appbarInited = true;

    let lastScrollTop = 0;
    const appBar = document.querySelector('.booking-appbar');
    if (!appBar) return;

    window.addEventListener('scroll', function () {
        const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
        if (scrollTop > lastScrollTop) appBar.classList.add('hide');
        else appBar.classList.remove('hide');
        lastScrollTop = scrollTop <= 0 ? 0 : scrollTop;
    }, { passive: true });
};

window.scrollToSection = (id) => {
    const element = document.getElementById(id);
    if (element) element.scrollIntoView({ behavior: 'smooth' });
};
