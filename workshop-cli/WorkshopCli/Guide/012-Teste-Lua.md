
    function love.load()
    player = {}
    player.x = 400
    player.y = 200
    player.speed = 3
    player.scale = 1
    player.sprite = love.graphics.newImage('sprites/parrot.png')
    background = love.graphics.newImage('sprites/background.png')
    end

    function love.update(dt)
    	if love.keyboard.isDown("right") then --eddita aqui o teu codigo
    		player.x = player.x + player.speed
    	end
    	if love.keyboard.isDown("left") then
	    	player.x = player.x - player.speed --estou  farto desta merda
	    end
    	if love.keyboard.isDown("up") then
	    	player.y = player.y - player.speed
    	end
	    if love.keyboard.isDown("down") then
    		player.y = player.y + player.speed
    	end
    end

