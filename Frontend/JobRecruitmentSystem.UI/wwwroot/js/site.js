(function () {
    document.querySelectorAll('[data-confirm]').forEach(function (form) {
        form.addEventListener('submit', function (e) {
            if (!confirm(form.getAttribute('data-confirm'))) {
                e.preventDefault();
            }
        });
    });
})();

(function () {
    var overlay = document.getElementById('page-loading-overlay');
    if (!overlay) {
        return;
    }

    document.querySelectorAll('form.js-loading-form').forEach(function (form) {
        form.addEventListener('submit', function () {
            overlay.classList.remove('d-none');
        });
    });

    // If the page was restored from bfcache (e.g. back button), make sure
    // a stale overlay from a previous submit never sticks around.
    window.addEventListener('pageshow', function () {
        overlay.classList.add('d-none');
    });
})();
