
3. No fim do teu código mete esta função

  function updateMissiles(dt)
    for i=table.getn(missiles), 1, -1 do
      missil = missiles[i]
      missil.xPos = missil.xPos + dt * missil.speed
      if missil.speed < missilMaxSpeed then
        missil.speed = missil.speed + dt * 100
      end
      if missil.xPos > love.graphics.getWidth() then
        table.remove(missiles, i)
      end
    end
  end 

A função "updateMissiles(dt)" cuida dos mísseis no jogo. Ela faz com que os mísseis se movam para a direita, fiquem mais rápidos com o tempo e desapareçam quando saem do ecrã do jogo. É como se os mísseis estivessem a voar pelo jogo, ficando cada vez mais rápidos, mas desaparecendo quando vão muito longe. Assim, a função garante que os mísseis se comportem corretamente no jogo.

