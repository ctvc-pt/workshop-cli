[color=white]
Agora vamos fazer os obstaculos aparecerem automaticamente
e moverem-se para a esquerda!

Primeiro, adiciona uma funcao que decide qual obstaculo criar.
Adiciona no final do ficheiro:

[/color] [color=blue]
function criarObstaculo()
    criarBanco()
end
[/color] [color=white]

Por agora so cria bancos. Mais tarde vamos adicionar gaivotas.

Agora vamos fazer os obstaculos aparecerem e moverem-se.
Adiciona o seguinte na funcao "love.update(dt)", DEPOIS
da fisica do jogador:

[/color] [color=blue]
    -- Spawn de obstaculos
    timerObstaculo = timerObstaculo + dt
    if timerObstaculo >= intervalo then
        timerObstaculo = 0
        intervalo = love.math.random(12, 20) / 10
        criarObstaculo()
    end

    -- Movimento e remocao de obstaculos
    for i = #obstaculos, 1, -1 do
        local obstaculo = obstaculos[i]
        obstaculo.x = obstaculo.x - velocidade * dt
        if obstaculo.x + obstaculo.largura < 0 then
            table.remove(obstaculos, i)
        end
    end
[/color] [color=white]

Como funciona:
- O "timerObstaculo" conta o tempo ate criar um novo obstaculo
- "intervalo" e aleatorio entre 1.2 e 2.0 segundos
- Cada obstaculo move-se para a esquerda a "velocidade" pixels/seg
- Quando sai do ecra, e removido da lista

O ciclo "for" conta de tras para a frente (#obstaculos ate 1)
para evitar problemas ao remover itens da lista.

Executa o jogo (Alt + L). Agora ja veem bancos!
Salta por cima deles com ESPACO!
[/color]

