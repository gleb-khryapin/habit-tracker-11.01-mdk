// Общие помощники для всех страниц.

/** Запрос к API. Возвращает { ok, status, data }; data — тело ответа или null. */
async function api(method, path, body) {
  const options = { method, headers: {} };
  if (body !== undefined) {
    options.headers['Content-Type'] = 'application/json';
    options.body = JSON.stringify(body);
  }
  let response;
  try {
    response = await fetch(path, options);
  } catch {
    return { ok: false, status: 0, data: { error: 'Сервер недоступен. Проверь, что приложение запущено' } };
  }
  const text = await response.text();
  let data = null;
  if (text) {
    try { data = JSON.parse(text); } catch { data = { error: text }; }
  }
  return { ok: response.ok, status: response.status, data };
}

function errorText(result) {
  return (result.data && result.data.error) || `Ошибка ${result.status}`;
}

/**
 * Создание DOM-элемента. Текст вставляется через textContent,
 * поэтому пользовательский ввод не исполняется как HTML.
 */
function el(tag, attrs = {}, ...children) {
  const node = document.createElement(tag);
  for (const [key, value] of Object.entries(attrs)) {
    if (value === null || value === undefined || value === false) continue;
    if (key === 'class') node.className = value;
    else if (key === 'testid') node.dataset.testid = value;
    else if (key.startsWith('on')) node.addEventListener(key.slice(2), value);
    else if (key === 'text') node.textContent = value;
    else node.setAttribute(key, value === true ? '' : value);
  }
  for (const child of children.flat()) {
    if (child === null || child === undefined || child === false) continue;
    node.append(child instanceof Node ? child : document.createTextNode(String(child)));
  }
  return node;
}

// Один постоянный элемент уведомления: меняется только текст и видимость.
// Так UI-тесты не ловят StaleElementReference, когда уведомления идут одно за другим.
let toastTimer;
function toast(message, type = 'info') {
  let node = document.querySelector('[data-testid="toast"]');
  if (!node) {
    node = el('div', { class: 'toast hidden', testid: 'toast', role: 'status', 'aria-live': 'polite' });
    document.body.append(node);
  }
  clearTimeout(toastTimer);
  node.textContent = message;
  node.dataset.type = type;
  node.classList.toggle('toast-error', type === 'error');
  node.classList.remove('hidden');
  toastTimer = setTimeout(() => node.classList.add('hidden'), 3500);
}

/** '2026-10-09' → '09.10.2026' */
function formatDate(iso) {
  if (!iso) return '';
  const [y, m, d] = iso.split('-');
  return `${d}.${m}.${y}`;
}

/** 1 день, 2 дня, 5 дней */
function plural(n, one, few, many) {
  const mod10 = n % 10, mod100 = n % 100;
  if (mod10 === 1 && mod100 !== 11) return one;
  if (mod10 >= 2 && mod10 <= 4 && (mod100 < 12 || mod100 > 14)) return few;
  return many;
}
const daysWord = (n) => `${n} ${plural(n, 'день', 'дня', 'дней')}`;

function formatHours(h) {
  return `${String(h).replace('.', ',')} ч/день`;
}

/** Загружает пользователя и meta; без входа отправляет на страницу входа. */
async function requireUser() {
  const [me, meta] = await Promise.all([api('GET', '/api/auth/me'), api('GET', '/api/meta')]);
  if (!me.ok) {
    location.href = '/';
    return null;
  }
  return { user: me.data, meta: meta.ok ? meta.data : null };
}

function renderTopbar({ user, meta }, current) {
  const right = el('div', { class: 'topbar-right' },
    el('span', { class: 'user-chip', testid: 'header-username' }, user.username),
    user.role === 'admin' && current !== 'admin'
      ? el('a', { class: 'btn btn-ghost btn-sm', href: '/admin.html', testid: 'link-admin' }, 'Админка')
      : null,
    current === 'admin'
      ? el('a', { class: 'btn btn-ghost btn-sm', href: '/habits.html', testid: 'link-habits' }, 'Мои привычки')
      : null,
    el('button', { class: 'btn btn-ghost btn-sm', testid: 'btn-logout', onclick: logout }, 'Выйти'),
  );
  const bar = el('header', { class: 'topbar' },
    el('a', { class: 'brand', href: '/habits.html' }, el('span', { class: 'brand-mark' }, '✦'), 'Привычки'),
    right,
  );
  document.body.prepend(bar);

  if (meta && meta.clockOverridden) {
    bar.after(el('div', { class: 'test-banner', testid: 'test-date-banner' },
      `🧪 Тестовая дата: ${formatDate(meta.today)}`));
  }
}

async function logout() {
  await api('POST', '/api/auth/logout');
  location.href = '/';
}

/** Открывает <dialog> и возвращает его. Закрытие по Esc и клику по фону. */
function openDialog(dialog) {
  if (!dialog.dataset.bound) {
    dialog.addEventListener('click', (e) => { if (e.target === dialog) dialog.close(); });
    dialog.dataset.bound = '1';
  }
  dialog.showModal();
  return dialog;
}

/** Окно подтверждения. Возвращает Promise<boolean>. */
function confirmDialog(title, text, confirmLabel = 'Удалить') {
  return new Promise((resolve) => {
    const dialog = el('dialog', { testid: 'confirm-modal' });
    const done = (value) => { dialog.close(); dialog.remove(); resolve(value); };
    dialog.append(el('div', { class: 'modal-body' },
      el('h3', { testid: 'confirm-title' }, title),
      el('p', { class: 'muted', style: 'margin-top:8px' }, text),
      el('div', { class: 'modal-foot' },
        el('button', { class: 'btn btn-ghost', testid: 'btn-confirm-cancel', onclick: () => done(false) }, 'Отмена'),
        el('button', { class: 'btn btn-danger', testid: 'btn-confirm-delete', onclick: () => done(true) }, confirmLabel),
      ),
    ));
    dialog.addEventListener('cancel', () => done(false));
    dialog.addEventListener('click', (e) => { if (e.target === dialog) done(false); });
    document.body.append(dialog);
    dialog.showModal();
  });
}

/** Выбор смайла: ряд кнопок, выбранная подсвечивается. */
function emojiPicker(emojis, selected, onChange) {
  const wrap = el('div', { class: 'chips', testid: 'emoji-picker' });
  for (const emoji of emojis) {
    const chip = el('button', {
      type: 'button', class: `chip chip-emoji ${emoji === selected ? 'selected' : ''}`,
      testid: 'emoji-option', 'data-emoji': emoji, 'aria-label': `Смайл ${emoji}`,
      onclick: () => {
        wrap.querySelectorAll('.chip').forEach((c) => c.classList.remove('selected'));
        chip.classList.add('selected');
        onChange(emoji);
      },
    }, emoji);
    wrap.append(chip);
  }
  return wrap;
}
