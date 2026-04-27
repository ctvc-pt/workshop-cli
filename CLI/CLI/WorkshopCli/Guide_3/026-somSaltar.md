[color=white]
Agora vamos adicionar um som curto que toca sempre que o
skater salta.

1. DENTRO da funcao "love.load()", a seguir a musica de fundo,
adiciona:

[/color] [color=blue]
    -- Som de salto
    somSalto = love.audio.newSource("somSalto.wav", "static")
[/color] [color=white]

Para sons curtos usamos "static" (carrega tudo de uma vez para a
memoria, toca mais rapido).

2. Agora vamos toca-lo quando o skater salta. Vai a funcao
"love.keypressed(tecla)" e DENTRO do "if" do salto, depois do
"jogador.agachado = false", adiciona:

[/color] [color=blue]
        somSalto:play()
[/color] [color=white]

O bloco do salto deve ficar assim no fim:

[/color] [color=blue]
    if (tecla == "space" or tecla == "up") and jogador.noChao and not fimDeJogo then
        jogador.velocidadeY = jogador.forcaSalto
        jogador.noChao = false
        jogador.agachado = false
        somSalto:play()
    end
[/color] [color=white]

Executa o jogo (Alt + L) e carrega ESPACO para saltar com som!
[/color]
