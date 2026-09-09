(function () {
    var bellBtn = document.getElementById('notification-bell-btn');
    var badge = document.getElementById('notification-badge');
    var list = document.getElementById('notification-list');
    var markAllBtn = document.getElementById('notification-mark-all');

    if (!bellBtn || !list) {
        return;
    }

    var emptyHtml = '<p class="text-muted text-center small py-3 mb-0">' + list.getAttribute('data-empty-text') + '</p>';

    function updateBadge(count) {
        if (count > 0) {
            badge.textContent = count > 9 ? '9+' : String(count);
            badge.classList.remove('d-none');
        } else {
            badge.classList.add('d-none');
        }
    }

    function fetchUnreadCount() {
        fetch('/Notification/UnreadCount')
            .then(function (r) { return r.json(); })
            .then(function (data) { updateBadge(data.count || 0); })
            .catch(function () { /* ignore transient network errors */ });
    }

    function renderList(notifications) {
        if (!notifications || !notifications.length) {
            list.innerHTML = emptyHtml;
            return;
        }

        list.innerHTML = '';
        notifications.forEach(function (n) {
            var item = document.createElement(n.link ? 'a' : 'div');
            item.className = 'notification-item' + (n.isRead ? '' : ' unread');
            if (n.link) {
                item.href = n.link;
            }

            var message = document.createElement('div');
            message.className = 'notification-message';
            message.textContent = n.message;

            var time = document.createElement('div');
            time.className = 'notification-time';
            time.textContent = new Date(n.createdAt).toLocaleString();

            item.appendChild(message);
            item.appendChild(time);

            item.addEventListener('click', function () {
                if (!n.isRead) {
                    n.isRead = true;
                    item.classList.remove('unread');
                    fetch('/Notification/MarkRead?id=' + n.id, { method: 'POST' })
                        .then(fetchUnreadCount)
                        .catch(function () { /* ignore */ });
                }
            });

            list.appendChild(item);
        });
    }

    function fetchList() {
        fetch('/Notification/List')
            .then(function (r) { return r.json(); })
            .then(renderList)
            .catch(function () { /* ignore */ });
    }

    bellBtn.addEventListener('click', fetchList);

    if (markAllBtn) {
        markAllBtn.addEventListener('click', function (e) {
            e.stopPropagation();
            fetch('/Notification/MarkAllRead', { method: 'POST' })
                .then(function () {
                    updateBadge(0);
                    fetchList();
                })
                .catch(function () { /* ignore */ });
        });
    }

    fetchUnreadCount();
    setInterval(fetchUnreadCount, 45000);
})();
