
3. No fim do teu código mete esta função:

    function updateEnemies(dt)
        if spawnTimer > 0 then
            spawnTimer = spawnTimer - dt
        else
            spawnEnemies()
        end
  
        for i=table.getn(enemies), 1, -1 do
            enemy=enemies[i]
            enemy.update = enemy:update(dt)
            if enemy.xPos < -enemy.width then
                table.remove(enemies, i)
            end
         end
    end

A função "updateEnemies(dt)" cuida dos inimigos no jogo. Ela faz com que os inimigos apareçam de tempos em tempos (definido em "spawnTimer") e desapareçam quando saem do ecrã do jogo. 
Tal como a função "updateMissiles(dt)", colocar esta função dentro da função "love.update(dt)".

