
Já temos a nossa nave a desenhar, mas para nos facilitar o trabalho mais tarde vamos criar atributos para a nossa nave. Altera o teu codigo para o que esta aqui:

function love.load()
  shipImage = love.graphics.newImage("spaceShips_008.png")
  player = {xPos = 0,yPos = 0,width = 64,height = 64,speed=200,img=shipImage}
end

function love.draw()
  love.graphics.draw(player.img, player.xPos, player.yPos, 0, 1, 1)
end
