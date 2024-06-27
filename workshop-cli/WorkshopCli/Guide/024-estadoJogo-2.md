[color=white]
Vamos adicionar o que faz com que o jogador perca.

4. Adiciona na função "verificaJogadorInimigoColisao()", debaixo da linha "table.remove(inimigos, index)":
   [/color] [color=blue]
   if vidas <= 0 then
        estadoJogo = "perder"
    end
   [/color] [color=white]
5. E antes da linha "table.remove(inimigos, index)":
   [/color] [color=blue]
    vidas = vidas - 1
   [/color] [color=white]
6. Adiciona no final do código todo:
   [/color] [color=blue]
   -- Função para reiniciar o jogo
    function love.keypressed(key)
        if key == 'r' and (estadoJogo == "perder" or estadoJogo == "vitoria") then
            -- Reinicia o jogo
            estadoJogo = "a jogar"
            pontuacao = 0
            vidas = 3
            posicaoX = 100
            posicaoY = 500
            misseis = {}
            inimigos = {}
        end
    end
   [/color] [color=white]
Agora quando 3 inimigos te acertarem, perdes.
   [/color]
