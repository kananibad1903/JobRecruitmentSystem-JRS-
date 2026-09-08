(function () {
    var toggleBtn = document.getElementById('chat-toggle-btn');
    var closeBtn = document.getElementById('chat-close-btn');
    var chatBox = document.getElementById('chat-box');
    var form = document.getElementById('chat-form');
    var input = document.getElementById('chat-input');
    var messages = document.getElementById('chat-messages');

    if (!toggleBtn || !chatBox) {
        return;
    }

    toggleBtn.addEventListener('click', function () {
        chatBox.classList.toggle('d-none');
    });

    closeBtn.addEventListener('click', function () {
        chatBox.classList.add('d-none');
    });

    function appendMessage(text, sender) {
        var el = document.createElement('div');
        el.className = 'chat-message chat-message-' + sender;
        el.textContent = text;
        messages.appendChild(el);
        messages.scrollTop = messages.scrollHeight;
    }

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        var text = input.value.trim();
        if (!text) {
            return;
        }

        appendMessage(text, 'user');
        input.value = '';
        input.disabled = true;

        fetch('/Chat/Send', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ message: text })
        })
            .then(function (response) { return response.json(); })
            .then(function (data) {
                appendMessage(data.reply || data.message || '...', 'bot');
            })
            .catch(function () {
                appendMessage('...', 'bot');
            })
            .finally(function () {
                input.disabled = false;
                input.focus();
            });
    });
})();
