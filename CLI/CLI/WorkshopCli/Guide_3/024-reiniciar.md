[color=white]
Quase pronto! So falta poder reiniciar o jogo quando perdes.

Adiciona uma NOVA funcao no final do ficheiro:

[/color] [color=blue]
function reiniciarJogo()
    jogador.y = CHAO_Y - jogador.altura
    jogador.velocidadeY = 0
    jogador.noChao = true
    jogador.agachado = false

    obstaculos = {}
    timerObstaculo = 0
    intervalo = 1.4
    pontuacao = 0
    fimDeJogo = false
end
[/color] [color=white]

Esta funcao volta tudo ao estado inicial: posicao do jogador,
lista de obstaculos vazia, pontuacao a zero e fim de jogo desligado.

Agora adiciona o seguinte DENTRO da funcao "love.keypressed(tecla)",
DEPOIS do codigo de agachar:

[/color] [color=blue]
    -- Reiniciar
    if tecla == "r" and fimDeJogo then
        reiniciarJogo()
    end
[/color] [color=white]

Executa o jogo (Alt + L). Agora quando perdes, carrega R
para reiniciar e jogar outra vez!
[/color] [color=green]


        ███████ ██ ███    ███
        ██      ██ ████  ████
        █████   ██ ██ ████ ██
        ██      ██ ██  ██  ██
        ██      ██ ██      ██ 

[/color] [color=white]

Parabens! O teu jogo esta completo!
[/color]

