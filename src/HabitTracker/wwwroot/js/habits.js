// Страница «Мои привычки».

let session;      // { user, meta }
let habits = [];

const STATUS_LABEL = { active: 'в процессе', completed: 'выполнена', failed: 'прервана' };
const SECTIONS = [
  { status: 'active', title: 'В процессе' },
  { status: 'completed', title: 'Выполнены' },
  { status: 'failed', title: 'Прерваны' },
];
const DAY_PRESETS = [7, 21, 30, 66];

(async function init() {
  session = await requireUser();
  if (!session) return;
  renderTopbar(session, 'habits');
  if (session.meta) {
    document.querySelector('[data-testid="today-date"]').textContent = `Сегодня ${formatDate(session.meta.today)}`;
  }
  document.getElementById('btn-new-habit').addEventListener('click', () => openHabitForm());
  await loadHabits();
})();

async function loadHabits() {
  const result = await api('GET', '/api/habits');
  if (result.status === 401) { location.href = '/'; return; }
  if (!result.ok) { toast(errorText(result), 'error'); return; }
  habits = result.data;
  render();
}

function render() {
  const root = document.getElementById('habits');
  root.replaceChildren();

  if (habits.length === 0) {
    root.append(el('div', { class: 'empty', testid: 'empty-state' },
      el('div', { class: 'empty-emoji' }, '🌱'),
      el('h3', {}, 'Пока пусто'),
      el('p', {}, 'Начни с одной маленькой привычки — этого достаточно.'),
      el('button', { class: 'btn btn-primary', testid: 'btn-empty-new-habit', onclick: () => openHabitForm() },
        'Выбрать привычку'),
    ));
    return;
  }

  for (const section of SECTIONS) {
    const items = habits.filter((h) => h.status === section.status);
    if (items.length === 0) continue;
    root.append(el('section', { class: 'section', testid: `section-${section.status}` },
      el('h2', {}, `${section.title} · ${items.length}`),
      el('div', { class: 'list' }, items.map(habitCard)),
    ));
  }
}

function habitCard(h) {
  const percent = Math.min(100, Math.round((h.progress / h.targetDays) * 100));

  const meta = [`цель ${daysWord(h.targetDays)}`];
  if (h.hoursPerDay !== null) meta.push(formatHours(h.hoursPerDay));
  meta.push(h.status === 'completed' ? `выполнена ${formatDate(h.completedOn)}` : `с ${formatDate(h.startDate)}`);

  const card = el('article', {
    class: `card habit is-${h.status}`, testid: 'habit-card',
    'data-habit-id': h.id, 'data-status': h.status,
  },
    el('div', { class: 'habit-emoji', testid: 'habit-emoji' }, h.emoji),
    el('div', { class: 'habit-main' },
      el('div', { class: 'habit-top' },
        el('span', { class: 'habit-title', testid: 'habit-title' }, h.title),
        el('span', { class: `badge badge-${h.status}`, testid: 'habit-status', 'data-status': h.status },
          STATUS_LABEL[h.status]),
      ),
      el('div', { class: 'habit-meta', testid: 'habit-meta' }, meta.join(' · ')),
      el('div', { class: 'progress' },
        el('div', {
          class: 'progress-track', role: 'progressbar', testid: 'habit-progress-bar',
          'aria-valuemin': 0, 'aria-valuemax': h.targetDays, 'aria-valuenow': h.progress,
        }, el('div', { class: 'progress-fill', style: `width:${percent}%` })),
        el('span', { class: 'progress-text', testid: 'habit-progress-text' }, `${h.progress} / ${h.targetDays}`),
      ),
    ),
  );

  if (h.status === 'completed') {
    card.append(el('div', { class: 'habit-note habit-note-success', testid: 'habit-message' },
      `🎉 ${h.completionMessage}`));
  }
  if (h.status === 'failed') {
    card.append(el('div', { class: 'habit-note habit-note-failed', testid: 'habit-failed-info' },
      `Пропущен день ${formatDate(h.failedOn)} — серия прервалась. Такую привычку можно только удалить и начать заново.`));
  }

  const actions = el('div', { class: 'habit-actions' });
  if (h.status === 'active') {
    actions.append(h.checkedToday
      ? el('button', { class: 'btn btn-done btn-sm', testid: 'btn-checkin', disabled: true }, '✓ Сегодня отмечено')
      : el('button', { class: 'btn btn-primary btn-sm', testid: 'btn-checkin', onclick: (e) => checkIn(h, e.currentTarget) },
        'Отметить сегодня'));
    actions.append(el('span', { class: 'spacer' }));
    actions.append(el('button', { class: 'btn btn-ghost btn-sm', testid: 'btn-edit', onclick: () => openHabitForm(h) },
      'Изменить'));
  } else {
    actions.append(el('span', { class: 'spacer' }));
  }
  actions.append(el('button', { class: 'btn btn-ghost btn-sm', testid: 'btn-delete', onclick: () => deleteHabit(h) },
    'Удалить'));
  card.append(actions);
  return card;
}

async function checkIn(habit, button) {
  button.disabled = true;
  const result = await api('POST', `/api/habits/${habit.id}/checkins`);
  if (!result.ok) {
    toast(errorText(result), 'error');
    await loadHabits();
    return;
  }
  const updated = result.data;
  habits = habits.map((h) => (h.id === updated.id ? updated : h));
  render();
  if (updated.status === 'completed') {
    showCompletion(updated);
  } else {
    toast(`Отмечено ✓ День ${updated.progress} из ${updated.targetDays}`);
  }
}

async function deleteHabit(habit) {
  const lost = habit.progress > 0
    ? `Прогресс (${daysWord(habit.progress)}) пропадёт без возможности восстановить.`
    : 'Действие нельзя отменить.';
  const ok = await confirmDialog(`Удалить «${habit.title}»?`, lost);
  if (!ok) return;
  const result = await api('DELETE', `/api/habits/${habit.id}`);
  if (!result.ok) toast(errorText(result), 'error');
  else toast('Привычка удалена');
  await loadHabits();
}

function showCompletion(habit) {
  const dialog = el('dialog', { testid: 'complete-modal' });
  dialog.append(el('div', { class: 'complete' },
    el('div', { class: 'complete-emoji' }, '🎉'),
    el('h3', { testid: 'complete-title' }, `«${habit.title}» — ${daysWord(habit.targetDays)} подряд!`),
    el('p', { class: 'complete-message', testid: 'complete-message' }, habit.completionMessage),
    el('button', { class: 'btn btn-primary', testid: 'btn-complete-close', onclick: () => dialog.close() }, 'Спасибо!'),
  ));
  dialog.addEventListener('close', () => dialog.remove());
  document.body.append(dialog);
  openDialog(dialog);
}

/** Форма создания (habit = null) или изменения привычки. */
async function openHabitForm(habit = null) {
  const isEdit = habit !== null;
  const emojis = session.meta ? session.meta.emojis : ['🎯'];
  const state = { mode: 'template', templateId: null, emoji: isEdit ? habit.emoji : emojis[0] };

  let templates = [];
  if (!isEdit) {
    const result = await api('GET', '/api/templates');
    if (result.ok) templates = result.data;
    if (templates.length === 0) state.mode = 'custom';
  }

  const dialog = el('dialog', { testid: 'habit-modal' });
  const errorBox = el('div', { class: 'form-error', testid: 'modal-error', role: 'alert' });

  // --- выбор из списка ---
  const templateList = el('div', { class: 'options', testid: 'template-list' },
    templates.length === 0
      ? el('p', { class: 'muted' }, 'Список пока пуст — создай свою привычку.')
      : templates.map((t) => el('button', {
        type: 'button', class: 'option', testid: 'template-option', 'data-template-id': t.id,
        onclick: (e) => {
          templateList.querySelectorAll('.option').forEach((o) => o.classList.remove('selected'));
          e.currentTarget.classList.add('selected');
          state.templateId = t.id;
        },
      },
        el('span', { class: 'option-emoji' }, t.emoji),
        el('span', {},
          el('div', { class: 'option-title' }, t.title),
          t.description ? el('div', { class: 'option-desc' }, t.description) : null),
      )),
  );

  // --- своя привычка ---
  const titleInput = el('input', {
    class: 'input', id: 'habit-title', testid: 'input-title', placeholder: 'Например, «Растяжка»',
    value: isEdit ? habit.title : '',
  });
  const customBlock = el('div', {},
    el('div', { class: 'field' }, el('label', { class: 'label', for: 'habit-title' }, 'Название'), titleInput),
    el('div', { class: 'field' }, el('span', { class: 'label' }, 'Смайл'),
      emojiPicker(emojis, state.emoji, (e) => { state.emoji = e; })),
  );

  // --- срок и часы ---
  const daysInput = el('input', {
    class: 'input', id: 'habit-days', type: 'number', min: 1, max: 365, step: 1, testid: 'input-days',
    value: isEdit ? habit.targetDays : 21,
  });
  const hoursInput = el('input', {
    class: 'input', id: 'habit-hours', type: 'number', min: 0.5, max: 24, step: 0.5, testid: 'input-hours',
    placeholder: 'не обязательно', value: isEdit && habit.hoursPerDay !== null ? habit.hoursPerDay : '',
  });
  const presets = el('div', { class: 'chips', style: 'margin-top:8px' },
    DAY_PRESETS.map((d) => el('button', {
      type: 'button', class: 'chip', testid: 'days-preset', 'data-days': d,
      onclick: () => { daysInput.value = d; },
    }, daysWord(d))));

  const scheduleBlock = el('div', { style: 'margin-top:18px' },
    el('div', { class: 'row' },
      el('div', { class: 'field' }, el('label', { class: 'label', for: 'habit-days' }, 'Сколько дней подряд'), daysInput),
      el('div', { class: 'field' }, el('label', { class: 'label', for: 'habit-hours' }, 'Часов в день'), hoursInput),
    ),
    presets,
    isEdit && habit.progress > 0
      ? el('p', { class: 'muted', style: 'margin-top:10px;font-size:13px', testid: 'edit-progress-hint' },
        `Уже отмечено: ${daysWord(habit.progress)}. Новый срок должен быть больше.`)
      : null,
  );

  // --- переключатель режимов (только при создании) ---
  const segmented = el('div', { class: 'segmented' },
    el('button', { type: 'button', testid: 'mode-template', 'data-mode': 'template' }, 'Из списка'),
    el('button', { type: 'button', testid: 'mode-custom', 'data-mode': 'custom' }, 'Своя'),
  );
  const applyMode = () => {
    segmented.querySelectorAll('button').forEach((b) => b.classList.toggle('selected', b.dataset.mode === state.mode));
    templateList.classList.toggle('hidden', state.mode !== 'template');
    customBlock.classList.toggle('hidden', state.mode !== 'custom');
    errorBox.textContent = '';
  };
  segmented.addEventListener('click', (e) => {
    const button = e.target.closest('button');
    if (!button) return;
    state.mode = button.dataset.mode;
    applyMode();
  });

  const saveButton = el('button', { type: 'submit', class: 'btn btn-primary', testid: 'btn-save' },
    isEdit ? 'Сохранить' : 'Создать');

  const form = el('form', { class: 'modal-body', novalidate: true },
    el('div', { class: 'modal-head' },
      el('h3', {}, isEdit ? 'Изменить привычку' : 'Новая привычка'),
      el('button', { type: 'button', class: 'btn btn-ghost btn-sm', 'aria-label': 'Закрыть', onclick: () => dialog.close() }, '✕'),
    ),
    isEdit ? null : segmented,
    isEdit ? null : templateList,
    customBlock,
    scheduleBlock,
    errorBox,
    el('div', { class: 'modal-foot' },
      el('button', { type: 'button', class: 'btn btn-ghost', testid: 'btn-cancel', onclick: () => dialog.close() }, 'Отмена'),
      saveButton,
    ),
  );

  form.addEventListener('submit', async (e) => {
    e.preventDefault();
    errorBox.textContent = '';
    const body = {
      targetDays: toNumber(daysInput.value),
      hoursPerDay: toNumber(hoursInput.value),
    };
    if (!isEdit && state.mode === 'template') {
      if (state.templateId === null) {
        errorBox.textContent = 'Выбери привычку из списка';
        return;
      }
      body.templateId = state.templateId;
    } else {
      body.title = titleInput.value;
      body.emoji = state.emoji;
    }

    saveButton.disabled = true;
    const result = isEdit
      ? await api('PUT', `/api/habits/${habit.id}`, body)
      : await api('POST', '/api/habits', body);
    saveButton.disabled = false;

    if (!result.ok) {
      errorBox.textContent = errorText(result);
      return;
    }
    dialog.close();
    toast(isEdit ? 'Сохранено' : 'Привычка добавлена — первый день уже сегодня');
    await loadHabits();
  });

  dialog.append(form);
  dialog.addEventListener('close', () => dialog.remove());
  document.body.append(dialog);
  if (!isEdit) applyMode();
  openDialog(dialog);
}

/** Пустое поле → null; иначе число (некорректный ввод уйдёт на сервер и вернётся ошибкой). */
function toNumber(value) {
  const trimmed = String(value).trim().replace(',', '.');
  return trimmed === '' ? null : Number(trimmed);
}
