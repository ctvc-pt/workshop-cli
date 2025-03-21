[color=white]
Para definir a posição de algo no ecrã recorremos ao sistema de
coordenadas do gráfico cartesiano. No entanto, em programação
é um pouco diferente, o eixo do Y aumenta para baixo em vez
de para cima.

-------------->x
|
|
|
|
V Y

Agora que temos o fundo, vamos adicionar o personagem 
principal do jogo: o pássaro!

1. Adiciona na função "load()" as variáveis do pássaro:
[/color] [color=blue]

   imagemPassaro = love.graphics.newImage("passaro.png")
   passaro = {
       x = 100,
       y = 150,
       largura = 40,
       altura = 30,
       velocidade = 0,
       gravidade = 0.5,
       salto = -6,
   }
[/color] [color=white]
Aqui definimos o tamanho do pássaro (largura = 40, altura = 30), bem como a sua posição
no ecrã (x, y).

2. Agora, na função "love.draw()", desenha o pássaro no 
tamanho correto:
[/color] [color=blue]
   love.graphics.draw(
   imagemPassaro,
   passaro.x,
   passaro.y,
   0,
   passaro.largura / imagemPassaro:getWidth(),
   passaro.altura / imagemPassaro:getHeight()
   )

[/color] [color=white]
Executa o jogo (Alt+L) e vais ver o pássaro no ecrã!
[/color] [color=red]
     ____  _____ ____    _    _____ ___ ___  
    |  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
    | | | |  _| \___ \ / _ \ | |_   | | | | |
    | |_| | |___ ___) / ___ \|  _|  | | |_| |
    |____/|_____|____/_/   \_\_|   |___\___/
[/color] [color=white]

Exprerimenta mudar a posição inicial do pássaro e vê onde ele começa na tela!

Volta a executar o jogo (Alt+L) para verificar se está tudo bem.
[/color]
