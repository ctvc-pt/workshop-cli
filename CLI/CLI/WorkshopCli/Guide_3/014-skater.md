[color=white]
Ja tens o skater no ecra! Agora vamos adicionar pontos
e instrucoes para o jogador ver.

Adiciona o seguinte no FINAL da funcao "love.draw()",
depois de desenhares o skater:

[/color] [color=blue]
    -- Interface
    love.graphics.setColor(0, 0, 0)
    love.graphics.print("Pontos: " .. math.floor(pontuacao), 20, 20)
    love.graphics.print("ESPACO = saltar  |  BAIXO = agachar", 20, 50)
[/color] [color=white]

O ".." em Lua serve para juntar textos. "math.floor" arredonda
o numero para baixo (sem casas decimais).

Agora vamos fazer o skater mexer-se! Precisamos de gravidade
para ele poder saltar.

Adiciona o seguinte DENTRO da funcao "love.update(dt)":

[/color] [color=blue]
    -- Fisica do jogador
    jogador.velocidadeY = jogador.velocidadeY + jogador.gravidade * dt
    jogador.y = jogador.y + jogador.velocidadeY * dt

    if jogador.y + jogador.altura >= CHAO_Y then
        jogador.y = CHAO_Y - jogador.altura
        jogador.velocidadeY = 0
        jogador.noChao = true
    end
[/color] [color=white]

Como funciona a gravidade:
- "velocidadeY" aumenta com o tempo (o skater cai mais rapido)
- A posicao "y" muda conforme a velocidade
- Quando o skater chega ao chao (CHAO_Y), ele para

O "dt" (delta time) e o tempo entre cada frame. Multiplicar
por "dt" faz o jogo correr a mesma velocidade em qualquer
computador.

Executa o jogo (Alt + L). O skater ainda nao salta, mas ja
tem gravidade!
[/color]

