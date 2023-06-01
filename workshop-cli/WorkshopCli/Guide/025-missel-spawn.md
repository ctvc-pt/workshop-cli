
4. Depois do último código insere o seguinte:

function spawnMissil(x, y, speed)
    if canFire then
      missil = {xPos = x, yPos = y, width = 16, height=16, speed=speed, img = missilImage}
      table.insert(missiles, missil)
  
      canFire = false
      missilTimer = missilTimerMax
    end
  end

A função "spawnMissil(x, y, speed)" cria e lança um míssil no jogo. 

Ela verifica se podemos disparar um míssil e, se sim, cria um novo míssil com uma posição inicial, tamanho, velocidade e imagem definidos.

O míssil é adicionado ao jogo e impedimos que mais mísseis sejam disparados imediatamente. 

Depois de algum tempo, poderemos disparar outro míssil. Assim, a função controla o lançamento de mísseis no jogo.

