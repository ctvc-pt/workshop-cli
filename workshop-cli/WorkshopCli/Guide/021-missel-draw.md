
2. Agora vamos desenhar os mísseis na função "draw()"

for index, missil in ipairs(missiles) do
      love.graphics.draw(missil.img, missil.xPos, missil.yPos)
    end

    