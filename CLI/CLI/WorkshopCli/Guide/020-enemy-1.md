[color=white]
Agora os inimigos ja aparecem, mas ainda estao parados.
Neste passo vamos ensinar cada inimigo a andar sozinho.

1. No VS Code, procura esta funcao:
   [/color] [color=blue]
    function atualizarInimigos(dt)
   [/color] [color=white]
2. Dentro dessa funcao ja existe codigo que cria inimigos novos.

3. Vai ate ao fim da funcao, mas fica antes do ultimo "end".

4. Cola este codigo nesse sitio:
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
O que este codigo faz:

- Olha para todos os inimigos que existem no jogo.
- Muda a posicao X para o inimigo andar para os lados.
- Muda a posicao Y para o inimigo andar para cima ou para baixo.
- Se um inimigo sair do ecra, ele e apagado para o jogo nao ficar lento.

Atencao: este codigo tem de ficar dentro da funcao "atualizarInimigos(dt)".
Nao o coloques dentro da funcao "draw" nem dentro da funcao "update".

Quando terminares, guarda o ficheiro e carrega em Alt + L para testar.
Se estiver certo, os inimigos vao comecar a entrar no ecra e a andar.
   [/color]
