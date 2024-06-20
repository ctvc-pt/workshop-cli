Adiciona este código no fim:

<span style="color:purple">
function verificaMissilInimigoColisao()
<span style="color:cyan">for index, inimigo in ipairs(inimigos) do
            for index2, missil in ipairs(misseis) do
                if intercepta(missil.posicaoX, missil.posicaoY, missil.width, missil.height, inimigo.posicaoX, inimigo.posicaoY, inimigo.width, inimigo.height) then
                    table.remove(inimigos, index)
                    table.remove(missil, index2)
                    break
                end
            end
        end
    end

</span>

Esta função verifica a posição do míssil e dos inimigos.
Se os mísseis tocarem nos inimigos, os dois aparecem.

Agora, adiciona no fim da função "update(dt)" por baixo da linha "verificaJogadorInimigoColisao()" o seguinte código:
<span style="color:cyan">verificaMissilInimigoColisao()

Agora corre o jogo (alt+l) e diverte-te.

