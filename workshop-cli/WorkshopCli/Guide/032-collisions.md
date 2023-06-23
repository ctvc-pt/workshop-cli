
O jogo está quase acabado, mas ainda falta uma coisa importante. Como podes ver, tu não consegues acertar nos inimigos, nem os inimigos conseguem acertar em ti.

1. Para isso, precisamos de 3 funções para verificar colisões. Adiciona este código no fim: 

function verificaJogadorInimigoColisao()
    for index, inimigo in ipairs(inimigos) do
        if intercepta(posicaoX, posicaoX, imagem:getWidth(), imagem:getHeight(), inimigo.posicaoX, inimigo.posicaoY, inimigo.width, inimigo.height) then
            posicaoX = 0
            posicaoX = 0
            misseis = {}
            inimigos = {}
            podeDisparar = true
            missilTempo = missilTempoMax
            geraInimigoTempo = 0
        end
    end
end

function verificaMissilInimigoColisao()
    for index, inimigo in ipairs(inimigos) do
        for index2, missil in ipairs(misseis) do
            if intercepta(missil.posicaoX, missil.posicaoY, missil.width, missil.height, inimigo.posicaoX, inimigo.posicaoY, inimigo.width, inimigo.height) then
                table.remove(inimigos, index)
                table.remove(misseis, index2)
                break
            end
        end
    end
end

function intercepta(x1, y1, w1, h1, x2, y2, w2, h2)
    return x1 < x2 + w2 and
    x1 + w1 > x2 and
    y1 < y2 + h2 and
    y1 + h1 > y2
end

2. Agora, adiciona no fim da função "update(dt)" por baixo da linha "atualizarInimigos(dt)" o seguinte código:

verificaJogadorInimigoColisao()
verificaMissilInimigoColisao()

Estas funções verificam a posição do jogador, dos mísseis e dos inimigos.
Caso os mísseis toquem nos inimigos, os inimigos desaparecem.
Se os inimigos tocarem no jogador, recomeça.

Agora corre o jogo (alt+l) e diverte-te.

