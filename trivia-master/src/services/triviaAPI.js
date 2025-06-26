export async function fetchQuestions(amount = 10, category = '', difficulty = '', type = 'multiple') {
  let url = `https://opentdb.com/api.php?amount=${amount}&type=${type}`;
  if (category) url += `&category=${category}`;
  if (difficulty) url += `&difficulty=${difficulty}`;

  try {
    const response = await fetch(url);
    if (!response.ok) throw new Error('Erro ao buscar perguntas');

    const data = await response.json();
    return data.results;
  } catch (error) {
    console.error('Erro na API de trivia:', error);
    return [];
  }
}
