
3. Adiciona este código no fim: 

    function verificaMissilInimigoColisao()
        for index, inimigo in ipairs(inimigos) do
            for index2, missil in ipairs(misseis) do
                if intercepta(missil.posicaoX, missil.posicaoY, missil.width, missil.height, inimigo.posicaoX, inimigo.posicaoY, inimigo.width, inimigo.height) then
                    table.remove(inimigos, index)
                    table.remove(missil, index2)
                    break
                end
            end
        end
    end

Esta função verifica a posição do míssil e dos inimigos.
Se os mísseis tocarem nos inimigos, os dois aparecem.

4. Agora, adiciona no fim da função "update(dt)" por baixo da linha "verificaJogadorInimigoColisao()" o seguinte código:

    verificaMissilInimigoColisao()

Agora corre o jogo (alt+l) e diverte-te.

