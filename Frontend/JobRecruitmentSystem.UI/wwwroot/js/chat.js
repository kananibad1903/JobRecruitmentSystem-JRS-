(function () {
    var toggleBtn = document.getElementById('chat-toggle-btn');
    var closeBtn = document.getElementById('chat-close-btn');
    var chatBox = document.getElementById('chat-box');
    var hintBubble = document.getElementById('chat-hint-bubble');
    var form = document.getElementById('chat-form');
    var input = document.getElementById('chat-input');
    var messages = document.getElementById('chat-messages');

    if (!toggleBtn || !chatBox) {
        return;
    }

    var idleWiggleTimer = null;

    function greetWiggle() {
        toggleBtn.classList.add('chat-toggle-greet');
        setTimeout(function () {
            toggleBtn.classList.remove('chat-toggle-greet');
        }, 600);
    }

    function stopIdleWiggle() {
        if (idleWiggleTimer) {
            clearInterval(idleWiggleTimer);
            idleWiggleTimer = null;
        }
    }

    function startIdleWiggle() {
        // A gentle nudge every ~9s until the user opens the chat for the first time.
        idleWiggleTimer = setInterval(function () {
            if (!chatBox.classList.contains('chat-box-visible')) {
                greetWiggle();
            }
        }, 9000);
    }

    function showHintBubbleOnce() {
        if (!hintBubble) {
            return;
        }
        try {
            if (sessionStorage.getItem('qarisqaChatHintShown')) {
                return;
            }
        } catch (e) {
            // sessionStorage unavailable — show the hint anyway, just not "once per tab".
        }

        setTimeout(function () {
            hintBubble.classList.add('visible');
            try {
                sessionStorage.setItem('qarisqaChatHintShown', '1');
            } catch (e) { /* ignore */ }

            setTimeout(function () {
                hintBubble.classList.remove('visible');
            }, 4000);
        }, 1500);
    }

    function openChat() {
        hintBubble?.classList.remove('visible');
        chatBox.classList.remove('d-none');
        // force reflow so the opacity/transform transition actually plays
        void chatBox.offsetWidth;
        chatBox.classList.add('chat-box-visible');
        greetWiggle();
        stopIdleWiggle();
        input.focus();
    }

    function closeChat() {
        chatBox.classList.remove('chat-box-visible');
        window.setTimeout(function () {
            chatBox.classList.add('d-none');
        }, 300);
    }

    toggleBtn.addEventListener('click', function () {
        if (chatBox.classList.contains('chat-box-visible')) {
            closeChat();
        } else {
            openChat();
        }
    });

    closeBtn.addEventListener('click', closeChat);

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

    showHintBubbleOnce();
    startIdleWiggle();
})();
