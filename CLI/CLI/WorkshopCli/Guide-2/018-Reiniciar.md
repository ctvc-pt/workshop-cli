[color=white]
De seguida vamos adicionar a lógica para reiniciar o jogo quando 
o jogador pressionar a tecla 'R' após o game over.

1. Na função "update(dt)", em baixo da linha "if gameOver then" que adicionamos anteriormente 
adiciona o seguinte:
   [/color] [color=blue]
   if love.keyboard.isDown("r") then
      reiniciarJogo()
   end
   [/color] [color=white]

2. Agora vamos adicionar a função que vai ser responsável por reiniciar o jogo
para isso adiciona no fim do teu código:
   [/color] [color=blue]
   function reiniciarJogo()
      passaro.y = 150
      passaro.velocidade = 0
      canos = {}
      gameOver = false
   end
   [/color] [color=white]
3. Na função "draw()" em baixo da linha "love.graphics.printf("Game Over!", 0, 
love.graphics.getHeight() / 2 - 20, love.graphics.getWidth(), "center")
adiciona esta linha de código:
   [/color] [color=blue]
   love.graphics.printf(
      "Press 'R' to restart",
      0,
      love.graphics.getHeight() / 1.5,
      love.graphics.getWidth(),
      "center"
   )
   [/color] [color=white]

Agora, o jogo vai reiniciar quando o jogador pressionar a tecla R após o game over, 
e a tela de "Game Over" será mostrada.
[/color] 