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
            // Client-side validation (native or jQuery unobtrusive) may still
            // block this submit after our listener runs, since preventDefault()
            // from another handler doesn't stop us from firing too. Only show
            // the overlay when the form is actually about to navigate away.
            if (typeof form.checkValidity === 'function' && !form.checkValidity()) {
                return;
            }

            overlay.classList.remove('d-none');
            // Safety net: never let the overlay get stuck forever if something
            // else prevents the navigation (server error, blocked request, etc.).
            window.setTimeout(function () {
                overlay.classList.add('d-none');
            }, 15000);
        });
    });

    // If the page was restored from bfcache (e.g. back button), make sure
    // a stale overlay from a previous submit never sticks around.
    window.addEventListener('pageshow', function () {
        overlay.classList.add('d-none');
    });
})();

(function () {
    document.querySelectorAll('.career-quote-stack').forEach(function (stack) {
        var quotes = stack.querySelectorAll('.career-quote');
        if (quotes.length < 2) {
            return;
        }

        var index = 0;
        setInterval(function () {
            quotes[index].classList.remove('active');
            index = (index + 1) % quotes.length;
            quotes[index].classList.add('active');
        }, 5000);
    });
})();
