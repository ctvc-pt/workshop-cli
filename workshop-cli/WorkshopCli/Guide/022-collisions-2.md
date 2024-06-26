3. 
4. Adiciona este código no fim: 

    -- Função para verificar colisões entre mísseis e inimigos
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

Com esta função quando os mísseis tocarem nos inimigos, os dois desaparecem. 

4. Como último passo, adiciona no fim da função "update(dt)" por baixo da linha "verificaJogadorInimigoColisao()" o seguinte código:

    verificaMissilInimigoColisao()

Executa o jogo com Alt + L e diverte-te! 