
Por fim na função update() insere este codigo depois da movimentação do jogador

    function love.update(dt)
      -- codigo da movimentação do jogador

      if love.keyboard.isDown("space") then
        missilSpeed = missilStartSpeed
        if(left) then
          missilSpeed = missilSpeed - player.speed/2
        elseif(right) then
          missilSpeed = missilSpeed + player.speed/2
        end
        spawnMissil(player.xPos + player.width, player.yPos + player.height/2, missilSpeed)
      end

      if missilTimer > 0 then
        missilTimer = missilTimer - dt
      else
        canFire = true
      end

      updateMissiles(dt)
    end

O codigo eserido agora fa o seguinte:
1. Se a tecla "espaço" for pressionada, um míssil é disparado com base na velocidade do jogador. A velocidade do míssil é ajustada se o jogador estiver a mover-se para a esquerda ou direita.
2. Um temporizador é utilizado para controlar o intervalo entre os disparos de mísseis. Quando o temporizador termina, é possível disparar outro míssil.
3. A função updateMissiles é chamada para atualizar o movimento dos mísseis existentes no jogo.