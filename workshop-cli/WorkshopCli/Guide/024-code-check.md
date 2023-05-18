
Depois do desafio o teu codigo da função load() deve estar assim:

function love.update(dt)
  if love.keyboard.isDown("right") then      
    player.xPos = player.xPos + dt * player.speed
  end

  if love.keyboard.isDown("left") then
    player.xPos = player.xPos - dt * player.speed
  end
  if love.keyboard.isDown("down") then      
    player.yPos = player.yPos + dt * player.speed
  end

  if love.keyboard.isDown("up") then      
    player.yPos = player.yPos - dt * player.speed
  end 
end

