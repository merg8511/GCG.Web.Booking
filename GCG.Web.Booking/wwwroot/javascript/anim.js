// =======================================
//  Refugio del Sol - Animations (AOS + GSAP)
// =======================================

// ---------- Helpers ----------
window.scrollToSection = (id) => {
    const el = document.getElementById(id);
    if (!el) return;
    const y = el.getBoundingClientRect().top + window.scrollY - 80;
    window.scrollTo({ top: y, behavior: "smooth" });
};

// ---------- AOS ----------
window.animInitAOS = () => {
    if (!window.AOS) return;
    AOS.init({ once: true, offset: 80, duration: 600, easing: "ease-out", mirror: false });
};

// ---------- GSAP (HERO + ABOUT + SERVICES + GALLERY + REVIEWS + LOCATION) ----------
window.animInitGSAP = () => {
    if (!window.gsap) return;
    if (window.ScrollTrigger) gsap.registerPlugin(ScrollTrigger);

    // Limpieza (SPA)
    try {
        if (window.ScrollTrigger) ScrollTrigger.getAll().forEach(t => t.kill());
        gsap.globalTimeline.clear();
        gsap.killTweensOf("*");
    } catch (_) { }

    // ===== HERO =====
    const tlHero = gsap.timeline({ defaults: { ease: "power3.out", duration: 0.9 } });
    tlHero
        .from(".gsap-hero-title", { y: 40, opacity: 0 })
        .from(".gsap-hero-subtitle", { y: 30, opacity: 0 }, "-=0.55")
        .from(".gsap-hero-cta", { y: 20, opacity: 0, scale: 0.98 }, "-=0.50");

    if (window.ScrollTrigger) {
        gsap.to(".gsap-hero-bg .carousel", {
            scale: 1.06, yPercent: 5, ease: "none",
            scrollTrigger: { trigger: ".hero-section", start: "top top", end: "bottom top", scrub: true }
        });
        gsap.to(".carousel-overlay", {
            opacity: 0.25, ease: "none",
            scrollTrigger: { trigger: ".hero-section", start: "top top", end: "bottom top", scrub: true }
        });
        gsap.to(".hero-content", {
            yPercent: -10, ease: "none",
            scrollTrigger: { trigger: ".hero-section", start: "top top", end: "bottom top", scrub: true }
        });

        setTimeout(() => ScrollTrigger.refresh(), 250);
        window.addEventListener("resize", () => {
            clearTimeout(window.__rszT);
            window.__rszT = setTimeout(() => ScrollTrigger.refresh(), 150);
        }, { passive: true });
    }
};

// Re-init SPA
window.animReInit = () => {
    try {
        if (window.AOS) AOS.refreshHard();
        if (window.ScrollTrigger) ScrollTrigger.refresh();
    } catch (_) { }
};