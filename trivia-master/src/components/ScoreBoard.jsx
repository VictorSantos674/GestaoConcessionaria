export default function ScoreBoard({ name, score, current, total }) {
  return (
    <div className="score-board">
      <p><strong>👤 Jogador:</strong> {name}</p>
      <p><strong>🏆 Pontuação:</strong> {score}</p>
      <p><strong>❓ Pergunta:</strong> {current} / {total}</p>
    </div>
  );
}
