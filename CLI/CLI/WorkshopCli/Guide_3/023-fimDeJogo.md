[color=white]
O jogo ja para quando bates num obstaculo, mas precisamos
de mostrar uma mensagem de "Fim de Jogo".

Na funcao "love.draw()", na parte da interface onde tens
os pontos, substitui o bloco todo por:

[/color] [color=blue]
    -- Interface
    love.graphics.setColor(0, 0, 0)
    love.graphics.print("Pontos: " .. math.floor(pontuacao), 20, 20)

    if fimDeJogo then
        love.graphics.printf("FIM DE JOGO", 0, 150, LARGURA, "center")
        love.graphics.printf("Pressiona R para reiniciar", 0, 185, LARGURA, "center")
    else
        love.graphics.print("ESPACO = saltar  |  BAIXO = agachar", 20, 50)
    end
[/color] [color=white]

"love.graphics.printf" e como "print" mas permite alinhar
o texto. O "center" centra o texto na janela.

Quando "fimDeJogo" e verdadeiro, mostra a mensagem de fim.
Quando e falso, mostra as instrucoes normais.

Executa o jogo (Alt + L) e deixa o skater bater num
obstaculo para veres a mensagem!
[/color]

