
Já temos a nossa nave a desenhar, mas para facilitar o trabalho para mais tarde, vamos criar atributos para a nossa nave. Altera o teu código para o que está aqui:

function love.load()
  shipImage = love.graphics.newImage("nave1.png")
  player = {xPos = 0,yPos = 0,width = 64,height = 64,speed=200,img=shipImage}
end

function love.draw()
  love.graphics.draw(player.img, player.xPos, player.yPos, 0, 1, 1)
end

Volta a correr o jogo (alt+l) para verificar se esta tudo bem.

