
4. Agora que já criaste o jogador, é hora de desenhá-lo no ecrã. Insere o seguinte código dentro da função "love.draw()":

	function love.draw()
 		love.graphics.draw(background, 0, 0)
  		love.graphics.draw(player.sprite, player.x, player.y)
	end


