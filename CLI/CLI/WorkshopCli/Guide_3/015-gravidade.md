[color=white]
Agora tambem precisamos de atualizar a pontuacao com o tempo.

Adiciona o seguinte NO INICIO da funcao "love.update(dt)",
ANTES da fisica do jogador:

[/color] [color=blue]
    if fimDeJogo then return end

    -- Pontos sobem com o tempo
    pontuacao = pontuacao + dt * 10
[/color] [color=white]

A primeira linha "if fimDeJogo then return end" faz com que,
quando o jogo acabar, nada se atualize. Vamos usar isto mais tarde.

A pontuacao sobe automaticamente enquanto jogas. Quanto mais
tempo sobreviveres, mais pontos tens!

Executa o jogo (Alt + L) para veres a pontuacao a subir!
[/color]

