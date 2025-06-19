import { useTheme } from '../context/ThemeContext';

export default function ThemeToggle() {
  const { theme, toggleTheme } = useTheme();

  return (
    <button style={{ float: 'right', margin: '1rem' }} onClick={toggleTheme}>
      {theme === 'light' ? '🌙 Escuro' : '☀️ Claro'}
    </button>
  );
}
