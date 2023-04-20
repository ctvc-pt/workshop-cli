Testa o código e vê se está tudo como esperado. O código completo deve ficar assim:


	function love.load()
	    player = {}
	    player.x = 400
	    player.y = 200
	    player.sprite = love.graphics.newImage('sprites/'nome da imagem'.png')
	    background = love.graphics.newImage('sprites/'nome da imagem'.png')
	end

	function love.update(dt)
	    if love.keyboard.isDown("right") then
	        player.x = player.x + 3
  	  end
	end

	function love.draw()
 	   love.graphics.draw(background, 0, 0)
 	   love.graphics.draw(player.sprite, player.x, player.y)
	end