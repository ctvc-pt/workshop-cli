[color=white]
O teu jogo ja mostra o fundo! Vamos perceber como funciona.

Na funcao "love.load()" temos variaveis que definem o tamanho
do jogo:

[/color] [color=blue]
    LARGURA = 900
    ALTURA = 400
    CHAO_Y = 330
[/color] [color=white]

LARGURA e ALTURA sao o tamanho da janela.
CHAO_Y e a posicao vertical do chao (onde o skater anda).

Na funcao "love.draw()" o fundo e desenhado assim:

[/color] [color=blue]
    love.graphics.draw(imagemFundo, 0, 0, 0,
        LARGURA / imagemFundo:getWidth(),
        ALTURA / imagemFundo:getHeight()
    )
[/color] [color=white]

Isto estica a imagem para caber no tamanho da janela.
Os dois ultimos valores sao a escala horizontal e vertical.

Agora vamos adicionar uma linha para representar o chao.
Adiciona o seguinte no final da funcao "love.draw()",
DEPOIS do codigo do fundo:

[/color] [color=blue]
    -- Chao
    love.graphics.setColor(0, 0, 0, 0.4)
    love.graphics.rectangle("fill", 0, CHAO_Y, LARGURA, 3)
[/color] [color=white]

"setColor" muda a cor para preto semi-transparente (o 0.4).
"rectangle" desenha um retangulo fino no ecra.

Executa o jogo (Alt + L) para veres a linha do chao!
[/color]

