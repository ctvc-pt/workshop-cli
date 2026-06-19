[color=white]
[/color] [color=red]
.____  _____ ____    _    _____ ___ ___  
|  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
| | | |  _| \___ \ / _ \ | |_   | | | | |
| |_| | |___ ___) / ___ \|  _|  | | |_| |
|____/|_____|____/_/   \_\_|   |___\___/
[/color] [color=white]

Tens uma surpresa na pasta do teu jogo: um som chamado
"somGameOver.wav".

O teu desafio e: faz com que esse som toque UMA VEZ quando
o jogo acaba (game over).

Pistas:
- Em "love.load()" carrega o ficheiro com "love.audio.newSource"
  (igual ao que fizeste para o som do salto).
- A variavel "fimDeJogo" passa a "true" no momento da colisao.
  Procura no codigo onde isso acontece (ficheiro 022-colisao).
- Logo a seguir a "fimDeJogo = true", chama o teu novo som com
  ":play()".

Cuidado: nao queres o som a tocar muitas vezes seguidas. Garante
que so toca quando o jogo passa de "a jogar" para "fim de jogo".

Boa sorte!
[/color]
