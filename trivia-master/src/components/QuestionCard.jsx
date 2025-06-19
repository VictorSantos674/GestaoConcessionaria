import { useEffect, useState } from 'react';
import { shuffleArray } from '../utils/shuffleArray';
import { motion } from 'framer-motion';

export default function QuestionCard({ question, index, onAnswer }) {
  const [answers, setAnswers] = useState([]);
  const [selected, setSelected] = useState(null);
  const [isCorrect, setIsCorrect] = useState(null);

  useEffect(() => {
    const shuffled = shuffleArray([
      ...question.incorrect_answers,
      question.correct_answer
    ]);
    setAnswers(shuffled);
    setSelected(null);
    setIsCorrect(null);
  }, [question]);

  const handleSelect = (answer) => {
    if (selected) return; // evita múltiplos cliques

    setSelected(answer);
    const correct = answer === question.correct_answer;
    setIsCorrect(correct);

    setTimeout(() => {
      onAnswer(correct);
    }, 1500);
  };

  return (
    <motion.div
      className="question-card"
      initial={{ opacity: 0, y: 30 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.4 }}
    >
      <h2 dangerouslySetInnerHTML={{ __html: question.question }} />
      <ul>
        {answers.map((answer, i) => {
          let className = '';
          if (selected) {
            if (answer === question.correct_answer) className = 'correct';
            else if (answer === selected) className = 'incorrect';
          }

          return (
            <li key={i}>
              <button
                className={className}
                onClick={() => handleSelect(answer)}
                disabled={!!selected}
                dangerouslySetInnerHTML={{ __html: answer }}
              />
            </li>
          );
        })}
      </ul>
      <p>Pergunta {index + 1}</p>
    </motion.div>
  );
}
