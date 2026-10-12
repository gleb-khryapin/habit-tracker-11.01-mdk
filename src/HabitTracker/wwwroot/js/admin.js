// Админка: предложенные привычки (шаблоны).

let session;
const root = document.getElementById('admin-root');

(async function init() {
  session = await requireUser();
  if (!session) return;
  renderTopbar(session, 'admin');

  if (session.user.role !== 'admin') {
    root.replaceChildren(el('div', { class: 'denied', testid: 'admin-denied' },
      el('div', { class: 'empty-emoji' }, '🔒'),
      el('h3', {}, 'Нет доступа'),
      el('p', { class: 'muted', style: 'margin:8px 0 20px' }, 'Этот раздел доступен только администратору.'),
      el('a', { class: 'btn', href: '/habits.html' }, 'К моим привычкам'),
    ));
    return;
  }

  renderLayout();
  await loadTemplates();
})();

function renderLayout() {
  const emojis = session.meta ? session.meta.emojis : ['🎯'];
  let emoji = emojis[0];

  const title = el('input', { class: 'input', id: 'tpl-title', testid: 'input-template-title', placeholder: 'Например, «Прогулка 30 минут»' });
  const description = el('input', { class: 'input', id: 'tpl-description', testid: 'input-template-description', placeholder: 'Коротко, что нужно делать' });
  const message = el('textarea', {
    class: 'input', id: 'tpl-message', testid: 'input-template-message',
    placeholder: 'Что получит человек, выполнив привычку до конца? Напиши тепло и про пользу.',
  });
  const errorBox = el('div', { class: 'form-error', testid: 'admin-form-error', role: 'alert' });
  const submit = el('button', { type: 'submit', class: 'btn btn-primary', testid: 'btn-add-template' }, 'Добавить в список');

  const form = el('form', { class: 'card', novalidate: true, testid: 'template-form' },
    el('div', { class: 'field' }, el('label', { class: 'label', for: 'tpl-title' }, 'Название'), title),
    el('div', { class: 'field' }, el('span', { class: 'label' }, 'Смайл'), emojiPicker(emojis, emoji, (e) => { emoji = e; })),
    el('div', { class: 'field' }, el('label', { class: 'label', for: 'tpl-description' }, 'Описание ', el('small', {}, '— до 200 символов, не обязательно')), description),
    el('div', { class: 'field' }, el('label', { class: 'label', for: 'tpl-message' }, 'Сообщение после выполнения ', el('small', {}, '— от 10 до 500 символов')), message),
    errorBox,
    el('div', { class: 'modal-foot' }, submit),
  );

  form.addEventListener('submit', async (e) => {
    e.preventDefault();
    errorBox.textContent = '';
    submit.disabled = true;
    const result = await api('POST', '/api/templates', {
      title: title.value, emoji, description: description.value, completionMessage: message.value,
    });
    submit.disabled = false;
    if (!result.ok) {
      errorBox.textContent = errorText(result);
      return;
    }
    form.reset();
    toast(`«${result.data.title}» добавлена в список`);
    await loadTemplates();
  });

  root.replaceChildren(
    el('div', { class: 'page-head' },
      el('div', {},
        el('h1', {}, 'Предложенные привычки'),
        el('p', { class: 'muted' }, 'Пользователи выбирают из этого списка при создании привычки'),
      ),
    ),
    el('section', { class: 'section' }, el('h2', {}, 'Добавить'), form),
    el('section', { class: 'section' },
      el('h2', { id: 'templates-heading' }, 'В списке'),
      el('div', { class: 'list', id: 'templates', testid: 'template-list' }),
    ),
  );
}

async function loadTemplates() {
  const result = await api('GET', '/api/templates');
  const list = document.getElementById('templates');
  if (!result.ok) {
    toast(errorText(result), 'error');
    return;
  }
  document.getElementById('templates-heading').textContent = `В списке · ${result.data.length}`;
  list.replaceChildren();
  if (result.data.length === 0) {
    list.append(el('div', { class: 'empty', testid: 'empty-state' }, el('p', {}, 'Список пуст. Добавь первую привычку выше.')));
    return;
  }
  for (const t of result.data) {
    list.append(el('div', { class: 'card tpl-row', testid: 'template-row', 'data-template-id': t.id },
      el('div', { class: 'habit-emoji' }, t.emoji),
      el('div', {},
        el('div', { class: 'habit-title', testid: 'template-row-title' }, t.title),
        t.description ? el('div', { class: 'habit-meta' }, t.description) : null,
        el('div', { class: 'tpl-message', testid: 'template-row-message' }, `🎉 ${t.completionMessage}`),
      ),
      el('button', { class: 'btn btn-ghost btn-sm', testid: 'btn-delete-template', onclick: () => remove(t) }, 'Удалить'),
    ));
  }
}

async function remove(template) {
  const ok = await confirmDialog(
    `Удалить «${template.title}» из списка?`,
    'Привычки, которые пользователи уже завели по этому шаблону, останутся без изменений.');
  if (!ok) return;
  const result = await api('DELETE', `/api/templates/${template.id}`);
  if (!result.ok) toast(errorText(result), 'error');
  else toast('Удалено из списка');
  await loadTemplates();
}
