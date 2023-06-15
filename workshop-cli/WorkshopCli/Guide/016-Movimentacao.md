
Já tens o jogador criado e o próximo passo é fazer a movimentação da nossa nave.
Vamos começar com a movimentação para a direita.
Na nossa função "update()" mete o seguinte código:

    if love.keyboard.isDown("right") then
      if posicaoX < love.graphics.getWidth() - 64 then
        posicaoX = posicaoX + dt * 200
      end
    end

Com este código também não deixa que a nave não saia do ecrã, usando "love.graphics.getWidth() - 64" que é a largura do ecrã.

Agora corre o código usando 'Alt+L' e pressiona na tecla da seta para a direita

