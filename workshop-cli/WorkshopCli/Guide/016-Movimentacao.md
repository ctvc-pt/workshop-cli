
Já tens o jogador criado e o próximo passo é fazer a movimentação da nossa nave.
Vamos começar com a movimentação para a direita.
Na nossa função "update()" mete o seguinte código:

    if love.keyboard.isDown("right") then      
      player.xPos = player.xPos + dt * player.speed
    end

Agora corre o código usando 'Alt+L' e pressiona na tecla da seta para a direita

