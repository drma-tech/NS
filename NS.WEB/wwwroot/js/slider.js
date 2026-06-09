"use strict";

export const slider = {
    initLists(id, size) {
        let mobile = window.innerHeight < 600 || window.innerWidth < 600;

        const el = document.getElementById(id);
        if (!el) return;
        const margin = mobile ? "4px" : "8px";

        if (el.dataset.splideInit === "1") return;
        el.dataset.splideInit = "1";

        const temp = new Splide(el, {
            autoWidth: true,
            gap: margin,
            pagination: false
        });

        temp.mount();
    },
};