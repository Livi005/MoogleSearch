// ============================================
// MOOGLE · JS auxiliar
// ============================================

// Sonido de olas
window.playWaveSound = function () {
    const audio = document.getElementById('wave-sound');
    if (!audio) return;
    try {
        audio.currentTime = 0;
        audio.volume = 0.4;
        audio.play().catch(() => { /* navegador bloqueó autoplay */ });
    } catch (e) { /* ignora */ }
};

// Salpicón al hacer click en el botón
window.splashBurst = function (buttonEl) {
    if (!buttonEl) return;
    const splash = buttonEl.querySelector('.splash');
    if (!splash) return;
    splash.classList.remove('active');
    void splash.offsetWidth; // reflow
    splash.classList.add('active');
};

// Espuma al hacer scroll
window.initFoamOnScroll = function () {
    if (window.__foamInit) return; // evita doble inicialización
    window.__foamInit = true;

    let lastScroll = window.scrollY;
    let cooldown = 0;

    window.addEventListener('scroll', () => {
        const now = Date.now();
        if (now - cooldown < 60) return; // no saturar
        cooldown = now;

        const delta = Math.abs(window.scrollY - lastScroll);
        lastScroll = window.scrollY;

        if (delta < 4) return; // movimientos muy pequeños

        const layer = document.getElementById('foam-layer');
        if (!layer) return;

        // Cuanta más velocidad, más espuma (máx 6)
        const numFoam = Math.min(6, Math.max(1, Math.floor(delta / 12)));

        for (let i = 0; i < numFoam; i++) {
            const foam = document.createElement('span');
            foam.className = 'foam';
            foam.style.left = (Math.random() * 100) + 'vw';
            foam.style.top  = (Math.random() * 100) + 'vh';
            const size = 8 + Math.random() * 12;
            foam.style.width  = size + 'px';
            foam.style.height = size + 'px';
            layer.appendChild(foam);

            setTimeout(() => foam.remove(), 1500);
        }
    }, { passive: true });
};

// Arranca automáticamente cuando el DOM esté listo
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', window.initFoamOnScroll);
} else {
    window.initFoamOnScroll();
}