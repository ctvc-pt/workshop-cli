
Agora vamos desenhar os misseis na função draw()

for index, missil in ipairs(missiles) do
      love.graphics.draw(missil.img, missil.xPos, missil.yPos)
    end