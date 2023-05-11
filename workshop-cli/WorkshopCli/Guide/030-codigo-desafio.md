Quando acabares a tua função update deve estar assim:

	function love.update(dt)
    	if love.keyboard.isDown("right") then
        	player.x=player.x + 3
    	end
		if love.keyboard.isDown("left") then
      		player.x=player.x - 3
		end
    	if love.keyboard.isDown("up") then
      		player.y=player.y - 3
    	end
    	if love.keyboard.isDown("down") then
    		player.y=player.y + 3
    	end
	end