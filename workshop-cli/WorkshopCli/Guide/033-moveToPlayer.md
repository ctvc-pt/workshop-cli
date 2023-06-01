
2. E pra concluir insere este código:

  function moveToPlayer(obj, dt)
    xSpeed = math.sin(math.rad (60)) * obj.speed
    ySpeed = math.cos(math.rad (60)) * obj.speed
    if (obj.yPos - player.yPos) > 10 then
      obj.yPos = obj.yPos - ySpeed * dt
      obj.xPos = obj.xPos - xSpeed * dt
    elseif (obj.yPos - player.yPos) < -10 then
      obj.yPos = obj.yPos + ySpeed * dt
      obj.xPos = obj.xPos - xSpeed * dt
    else
      obj.xPos = obj.xPos - obj.speed * dt
    end
    return moveToPlayer
  end

Esta função é um pouco complicada, pois ela calcula a posição do jogador e faz com que o míssil vá em direção a ele.

E agora podes verificar o que fizeste até agora.

