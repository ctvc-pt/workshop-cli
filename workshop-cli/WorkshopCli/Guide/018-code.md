    
    function love.load()
  		background = love.graphics.newImage('sprites/background.png')
  		player = {}
  		player.x = 400
		player.y = 200
 		player.sprite = love.graphics.newImage('sprites/parrot.png')
	end

    function love.draw()
        love.graphics.print("Hello, world!", 100, 100);
	end