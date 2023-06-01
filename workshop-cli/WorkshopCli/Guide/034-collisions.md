
O jogo está quase acabado, mas ainda falta uma coisa importante. Como podes ver, tu não consegues acertar nos inimigos, nem os inimigos conseguem acertar em ti.
Para isso, precisamos de 2 funções para verificar colisões, que são o seguinte: 

  function checkCollisions()
    for index, enemy in ipairs(enemies) do
      if intersects(player, enemy) or intersects(enemy, player) then
        player = {xPos = 0, yPos = 0, width = 32, height = 32, speed=200, img=shipImage}
        missiles = {}
        enemies = {}
  
        canFire = true
        missilTimer = missilTimerMax
        spawnTimer = 0
      end
  
      for index2, missil in ipairs(missiles) do
        if intersects(enemy, missil) then
          table.remove(enemies, index)
          table.remove(missiles, index2)
          break
        end
      end
    end
  end
  
  function intersects(rect1, rect2)
    if rect1.xPos < rect2.xPos and rect1.xPos + rect1.width > rect2.xPos and
       rect1.yPos < rect2.yPos and rect1.yPos + rect1.height > rect2.yPos then
      return true
    else
      return false
    end
  end

Estas funções verificam a posição do jogador, dos mísseis e dos inimigos.
Caso os mísseis toquem nos inimigos, os inimigos desaparecem.
Se os inimigos tocarem no jogador, recomeça.

Coloca a função "checkCollisions()" na função "load.update(dt)".

E agora, o jogo está completo.

