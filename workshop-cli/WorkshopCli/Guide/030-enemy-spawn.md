
4. Depois do último código insere o seguinte:

function spawnEnemies()
    y = love.math.random(0, love.graphics.getHeight() - 64)
    enemyType = love.math.random(0, 1)
    if enemyType == 0 then
      enemy = Enemy:new{yPos = y, speed = meteorSpeed, img = meteorImage, update=moveLeft}
    elseif enemyType == 1 then
      enemy = Enemy:new{yPos = y, speed = eneSpeed, img = eneImage, update=moveToPlayer}
    end
    table.insert(enemies, enemy)
  
    spawnTimer = spawnTimerMax
  end

A função "spawnEnemies()" cria e difere os inimigos no jogo. 

Ela faz com que o meteoro e o míssil apareçam aleatoriamente e organiza a informação sobre eles.

