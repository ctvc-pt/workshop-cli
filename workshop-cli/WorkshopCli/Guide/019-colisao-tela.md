
Agora vou precisar que apagues a função "update()" que tens e metas este código por cima.

function love.update(dt)
  if love.keyboard.isDown("right") then
    if player.xPos < love.graphics.getWidth() - player.width then
      player.xPos = player.xPos + dt * player.speed
    end
  end

  if love.keyboard.isDown("left") then
    if player.xPos > 0 then
      player.xPos = player.xPos - dt * player.speed
    end
  end

  if love.keyboard.isDown("down") then
    if player.yPos < love.graphics.getHeight() - player.height then
      player.yPos = player.yPos + dt * player.speed
    end
  end

  if love.keyboard.isDown("up") then
    if player.yPos > 0 then
      player.yPos = player.yPos - dt * player.speed
    end
  end
end

O código que foi adicionado verifica se o jogador está perto das bordas do ecrã antes de se mover. Por exemplo, se estiver a mover-se para a direita, verifica se a posição horizontal do jogador (xPos) é menor do que a largura total do ecrã menos a largura do jogador (love.graphics.getWidth() - player.width). Isso impede que o jogador saia do ecrã.

O mesmo princípio é aplicado aos movimentos para a esquerda, para baixo e para cima, verificando se a posição do jogador está dentro dos limites do ecrã antes de se mover.

Desta forma, ao pressionar as teclas direcionais do teclado, o jogador irá mover-se dentro dos limites do ecrã, para a direita, esquerda, para cima ou para baixo no jogo.

