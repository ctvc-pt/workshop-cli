[color=white]
De seguida vamos adicionar a lógica para reiniciar o jogo quando 
o jogador pressionar a tecla 'R' após o game over.

1. Na função "update(dt)", em baixo da linha "if gameOver then" que adicionamos anteriormente 
adiciona o seguinte:
   [/color] [color=blue]
       if love.keyboard.isDown("r") then
           resetGame()  
       end
   [/color] [color=white]

2. Agora vamos adicionar a função que vai ser responsável por reiniciar o jogo
para isso adiciona no fim do teu código:
   [/color] [color=blue]
   function resetGame()
      bird.y = 150  -- Reposiciona o pássaro
      bird.speed = 0  -- Reseta a velocidade
      pipes = {}  -- Limpa os canos
      gameOver = false  -- Reseta o estado do jogo
   end
   [/color] [color=white]
3. Na função "draw()" em baixo da linha "love.graphics.printf("Game Over!", ....)"
adiciona esta linha de código:
   [/color] [color=blue]
   love.graphics.printf("Pressione 'R' para reiniciar", 0, 
   love.graphics.getHeight() / 1.5, 
   love.graphics.getWidth(), "center")
   [/color] [color=white]

Agora, o jogo vai reiniciar quando o jogador pressionar a tecla R após o game over, 
e a tela de "Game Over" será mostrada.
[/color] 