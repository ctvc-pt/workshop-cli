[color=white]
3. Adiciona este código no fim:
   [/color] [color=blue]
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
   [/color] [color=white]
Com esta função quando os mísseis tocarem nos inimigos, os dois desaparecem. 

5. Como último passo, adiciona no fim da função "update(dt)" por baixo da linha 
"verificaJogadorInimigoColisao()" o seguinte código:
   [/color] [color=blue]
    verificaMissilInimigoColisao()
   [/color] [color=white]
Executa o jogo com Alt + L e diverte-te!
   [/color]