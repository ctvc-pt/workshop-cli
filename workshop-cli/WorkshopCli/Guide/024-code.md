    
    function love.load()
  		background = love.graphics.newImage('sprites/background.png')
  		player = {}
  		player.x = 400
		player.y = 200
 		player.sprite = love.graphics.newImage('sprites/nome_da_imagem.png')
	end

    function love.update(dt)
        player.x = player.x + 3
    end

    function love.draw()
        love.graphics.draw(background, 0, 0)
  		love.graphics.draw(player.sprite, player.x, player.y)
	end