[color=white]
Os bancos ja funcionam! Agora vamos adicionar as gaivotas.
As gaivotas voam ao nivel da cabeca e tens de te agachar!

A gaivota e especial porque tem animacao - bate as asas!
O codigo no "love.load()" ja preparou 2 frames de animacao.

Adiciona uma NOVA funcao no final do ficheiro:

[/color] [color=blue]
function criarGaivota()
    local alturaDesenho = 70
    local escala = alturaDesenho / alturaFrameGaivota

    table.insert(obstaculos, {
        tipo = "gaivota",
        x = LARGURA + 10,
        y = CHAO_Y - 70,
        largura = 34,
        altura = 14,
        desenhoY = CHAO_Y - 92,
        escala = escala,
    })
end
[/color] [color=white]

A gaivota tem uma "hitbox" pequena (largura 34, altura 14)
para que o jogo seja justo. O "desenhoY" e a posicao visual
que pode ser diferente da hitbox.

Agora atualiza a funcao "criarObstaculo()" para tambem
criar gaivotas de vez em quando:

[/color] [color=blue]
function criarObstaculo()
    if love.math.random(1, 4) == 1 then
        criarGaivota()
    else
        criarBanco()
    end
end
[/color] [color=white]

Ha 25% de chance de aparecer uma gaivota e 75% um banco.

Agora vamos desenhar a gaivota! Na funcao "love.draw()",
dentro do ciclo "for" dos obstaculos, DEPOIS do "if" do
banco, adiciona:

[/color] [color=blue]
        if obstaculo.tipo == "gaivota" then
            local frameGaivota = gaivotaFrame1
            if math.floor(love.timer.getTime() * 8) % 2 == 1 then
                frameGaivota = gaivotaFrame2
            end

            love.graphics.draw(imagemGaivota, frameGaivota,
                obstaculo.x, obstaculo.desenhoY,
                0, obstaculo.escala, obstaculo.escala
            )
        end
[/color] [color=white]

A animacao funciona assim: 8 vezes por segundo muda entre
frame 1 e frame 2, e a gaivota bate as asas!

Executa o jogo (Alt + L). Agacha-te com seta BAIXO quando
vires uma gaivota!
[/color]

