[color=white]
Ja tens obstaculos mas ainda nao ha colisao! Se o skater
tocar num banco ou gaivota, nada acontece. Vamos mudar isso.

Primeiro, precisamos de uma funcao que verifica se dois
retangulos se tocam. Adiciona no final do ficheiro:

[/color] [color=blue]
function temColisao(a, b)
    return a.x < b.x + b.largura and
           a.x + a.largura > b.x and
           a.y < b.y + b.altura and
           a.y + a.altura > b.y
end
[/color] [color=white]

Isto compara as posicoes e tamanhos de dois retangulos.
Se se sobrepuserem em X e em Y, ha colisao!

Agora precisamos de calcular a "hitbox" do jogador.
A hitbox muda quando o jogador se agacha. Adiciona:

[/color] [color=blue]
function calcularHitbox()
    local alturaAtual = jogador.altura
    local yAtual = jogador.y
    local ajusteX = 7

    if jogador.agachado and jogador.noChao then
        alturaAtual = jogador.alturaAgachar
        yAtual = CHAO_Y - alturaAtual
    end

    return {
        x = jogador.x + ajusteX,
        y = yAtual,
        largura = jogador.largura,
        altura = alturaAtual,
    }
end
[/color] [color=white]

Quando o skater se agacha, a hitbox fica mais baixa.
Isso permite que as gaivotas passem por cima!

Agora vamos verificar a colisao no "love.update(dt)".
Adiciona o seguinte NO FINAL da funcao update, DEPOIS
do movimento dos obstaculos:

[/color] [color=blue]
    -- Colisao
    local hitboxJogador = calcularHitbox()
    for _, obstaculo in ipairs(obstaculos) do
        if temColisao(hitboxJogador, obstaculo) then
            fimDeJogo = true
        end
    end
[/color] [color=white]

Executa o jogo (Alt + L). Agora se bateres num obstaculo,
o jogo para! Mas ainda nao mostra mensagem de fim...
[/color]

