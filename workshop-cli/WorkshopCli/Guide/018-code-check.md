
Depois do desafio o teu código da função "load()" deve estar assim:

function love.update(dt)
  if love.keyboard.isDown("right") then      
    posicaoX = posicaoX + dt * 200
  end

  if love.keyboard.isDown("left") then
    posicaoX = posicaoX - dt * 200
  end
  if love.keyboard.isDown("down") then      
    posicaoY = posicaoY + dt * 200
  end

  if love.keyboard.isDown("up") then      
    posicaoY = posicaoY - dt * 200
  end
end

