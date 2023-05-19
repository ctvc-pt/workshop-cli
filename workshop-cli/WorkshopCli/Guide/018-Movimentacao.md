Já temos o nosso jogador criado e o proximo passo é fazer a movimentação da nossa nave.
Vamos começar com a movimentação para a direita.
No nosso update mete o seguinte codigo:

    if love.keyboard.isDown("right") then      
      player.xPos = player.xPos + dt * player.speed
    end

Agora corre o codigo usando 'Alt+j' e pressiona na tecla da seta para a direita


