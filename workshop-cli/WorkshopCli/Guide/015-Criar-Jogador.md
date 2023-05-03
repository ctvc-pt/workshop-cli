
Agora que já carregaste a imagem de fundo, é hora de criar o jogador.

1. Dentro do ficheiro "main.lua", inserimos o seguinte código:

	function love.load()
  		background = love.graphics.newImage('sprites/nome_da_imagem.png')
  		player = {}
  		player.x = 400
		player.y = 200
 		player.sprite = love.graphics.newImage('sprites/nome_da_imagem.png')
	end

Certifica-te de que substituis "nome_da_imagem" pelo nome da imagem que queres usar para o jogador.

Irás perceber para que serve o resto do código mais à frente.

