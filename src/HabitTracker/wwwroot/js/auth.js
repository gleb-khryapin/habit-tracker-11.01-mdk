// Страница входа и регистрации.

let mode = 'login';
const form = document.getElementById('auth-form');
const errorBox = document.querySelector('[data-testid="auth-error"]');
const submit = document.querySelector('[data-testid="btn-submit"]');
const tabs = document.querySelectorAll('.segmented button');

function setMode(next) {
  mode = next;
  tabs.forEach((t) => t.classList.toggle('selected', t.dataset.mode === mode));
  submit.textContent = mode === 'login' ? 'Войти' : 'Создать аккаунт';
  document.getElementById('password').autocomplete = mode === 'login' ? 'current-password' : 'new-password';
  document.getElementById('password-hint').classList.toggle('hidden', mode === 'login');
  errorBox.textContent = '';
}

tabs.forEach((t) => t.addEventListener('click', () => setMode(t.dataset.mode)));

form.addEventListener('submit', async (e) => {
  e.preventDefault();
  errorBox.textContent = '';
  submit.disabled = true;
  const body = {
    username: form.username.value,
    password: form.password.value,
  };
  const result = await api('POST', mode === 'login' ? '/api/auth/login' : '/api/auth/register', body);
  submit.disabled = false;
  if (result.ok) {
    location.href = '/habits.html';
  } else {
    errorBox.textContent = errorText(result);
  }
});

(async () => {
  // уже вошёл — сразу к привычкам
  const me = await api('GET', '/api/auth/me');
  if (me.ok) {
    location.href = '/habits.html';
    return;
  }
  const meta = await api('GET', '/api/meta');
  if (meta.ok && meta.data.testMode) {
    document.querySelector('[data-testid="demo-hint"]').classList.remove('hidden');
  }
})();
