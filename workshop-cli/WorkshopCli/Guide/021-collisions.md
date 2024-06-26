
O jogo está a ganhar forma, mas ainda falta uma coisa importante. Como podes ver, tu não consegues acertar nos inimigos, nem os inimigos conseguem acertar em ti.

Vamos adicionar 3 funções para verificar colisões entre o jogador e os inimigos.

1. Adiciona este código no fim: 

    -- Função para verificar colisões entre o jogador e os inimigos
    function verificaJogadorInimigoColisao()
        for index, inimigo in ipairs(inimigos) do
            if intercepta(posicaoX, posicaoY, imagem:getWidth(), imagem:getHeight(), inimigo.posicaoX, inimigo.posicaoY, inimigo.width, inimigo.height) then
                table.remove(inimigos, index)
            end
        end
    end

    -- Função para verificar se dois objetos se interceptam (colidem)
    function intercepta(x1, y1, w1, h1, x2, y2, w2, h2)
        return x1 < x2 + w2 and
        x1 + w1 > x2 and
        y1 < y2 + h2 and
        y1 + h1 > y2
    end

2. Agora, adiciona no fim da função "update(dt)" por baixo da linha "atualizarInimigos(dt)" o seguinte código:

    verificaJogadorInimigoColisao()

Com estas funções que adicionaste agora, se os inimigos tocarem no jogador, o jogo recomeça. Podes testar, executa o jogo com Alt + L. 
