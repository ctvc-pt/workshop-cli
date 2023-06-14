
Já temos a nossa nave a desenhar, mas para facilitar o trabalho para mais tarde, vamos criar atributos para a nossa nave. Altera o teu código para o que está aqui:

function love.load()
  imagem = love.graphics.newImage("nave1.png")
  posicaoX = 0
  posicaoY = 0
end

function love.draw()
  love.graphics.draw(imagem, posicaoX, posicaoY, 0, 1, 1)
end

Volta a correr o jogo (alt+l) para verificar se esta tudo bem.

