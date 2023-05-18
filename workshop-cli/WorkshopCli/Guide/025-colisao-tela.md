
Agora vou precisar que apagues a função aupdate() que tens e metas este codigo por cima.

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
