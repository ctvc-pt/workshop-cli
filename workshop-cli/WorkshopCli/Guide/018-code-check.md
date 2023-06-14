
Depois do desafio o teu código da função "load()" deve estar assim:

  function love.update(dt)
    if love.keyboard.isDown("right") then
      if posicaoX < love.graphics.getWidth() - 64 then
        posicaoX = posicaoX + dt * 200
      end
    end
    if love.keyboard.isDown("left") then
      if posicaoX > 0 then
        posicaoX = posicaoX - dt * 200
      end
    end
    if love.keyboard.isDown("down") then
      if posicaoY < love.graphics.getHeight() - 64 then
        posicaoY = posicaoY + dt * 200
      end
    end
    if love.keyboard.isDown("up") then
      if posicaoY > 0 then
        posicaoY = posicaoY - dt * 200
      end
    end
  end

