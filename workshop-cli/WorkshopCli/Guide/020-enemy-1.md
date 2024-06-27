[color=white]
Vamos fazer com que os inimigos movam-se.

5. Adiciona este código à função "atualizarInimigos(dt)":
   [/color] [color=blue]
    for i = #inimigos, 1, -1 do
        local inimigo = inimigos[i]
        inimigo.posicaoX = inimigo.posicaoX + inimigo.dirX * inimigo.velocidade * dt
        inimigo.posicaoY = inimigo.posicaoY + inimigo.dirY * inimigo.velocidade * dt

        if inimigo.posicaoX < -inimigo.width or inimigo.posicaoX > love.graphics.getWidth() or inimigo.posicaoY < -inimigo.height or inimigo.posicaoY > love.graphics.getHeight() then
            table.remove(inimigos, i)
        end
    end
   [/color] [color=white]
Agora deves ter um monte de inimigos a vir em direção da tua nave(alt+L).
   [/color]