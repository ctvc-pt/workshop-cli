
Vamos adicionar o que faz com que o jogador perca.

4. Adiciona na função "verificaJogadorInimigoColisao()", debaixo da linha "table.remove(inimigos, index)": 

   if vidas <= 0 then
        estadoJogo = "perder"
    end

5. E antes da linha "table.remove(inimigos, index)":

    vidas = vidas - 1

6. Adiciona no final do código todo:

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
            love.graphics.setBackgroundColor(0, 0, 0) -- volta ao fundo preto
        end
    end

E adiciona tudo o que está dentro da função "draw(dt)" debaixo da linha "love.graphics.setBackgroundColor(0, 0, 0)".

Agora quando 3 inimigos te acertarem, perdes.



