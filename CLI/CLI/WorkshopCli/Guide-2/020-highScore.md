[color=white]
Agora vamos adicionar uma variável para armazenar a pontuação máxima do jogador.

1. Dentro da função "load()", coloca a seguinte linha: 
   [/color] [color=blue]
      melhorPontuacao = 0
   [/color] [color=white]

2. Dentro da função "update(dt)" em cima deste código:

gameOver = true
somColisao:play()
somGameOver:play()

Adiciona o seguinte:
   [/color] [color=blue]
   if pontuacao > melhorPontuacao then
      melhorPontuacao = pontuacao
   end
   [/color] [color=white]

3. Por fim para ver a pontuação máxima vamos colocar na função "draw()", em baixo de 
onde escrevemos o "Score" adiciona esta linha:
   [/color] [color=blue]
   love.graphics.printf("High Score: " .. melhorPontuacao, 10, 10, 
   love.graphics.getWidth(), "center")
   [/color] [color=white]
Agora, o High Score sempre manterá o maior valor alcançado, mesmo depois de reiniciar o 
jogo com "R".
[/color] 