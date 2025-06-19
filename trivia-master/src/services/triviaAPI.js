export async function fetchQuestions(amount = 10, category = '', difficulty = '', type = 'multiple') {
  let url = `https://opentdb.com/api.php?amount=${amount}&type=${type}`;

  if (category) url += `&category=${category}`;
  if (difficulty) url += `&difficulty=${difficulty}`;

  const response = await fetch(url);
  const data = await response.json();

  return data.results;
}
